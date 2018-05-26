using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 医生操作
    /// </summary>
    public class BLLMPDoctor
    {
        /// <summary>
        /// 获取医生分页列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="cli_id">所属医院</param>
        /// <param name="search_word">搜索关键字</param>
        /// <returns></returns>
        public static List<Entity.ViewModel.MPUserDoctors> GetPageList(int page_size, int page_index, int cli_id, string search_word, out int total)
        {
            total = 0;
            if (page_size <= 0) page_size = 10;
            if (page_index <= 0) page_index = 1;
            List<Entity.ViewModel.MPUserDoctors> result = new List<Entity.ViewModel.MPUserDoctors>();
            int begin_index = (page_index - 1) * page_size + 1;
            int end_idnex = page_index * page_size;
            string where = "";
            if (cli_id > 0) where += " and ClinicID = " + cli_id.ToString();
            if (!string.IsNullOrWhiteSpace(search_word))
                where += " and (Charindex('" + search_word + "',RealName) >0 or (Charindex('" + search_word + "',IDCardNumber) >0 or (Charindex('" + search_word + "',Telphone) >0)";

            string sql_total = "select count(1) from (select U.ID,U.RealName,U.IDCardNumber,U.Telphone,D.ClinicID from(select * from MPUser where [Identity] = 3) as U LEFT OUTER JOIN MPUserDoctors as D on U.ID = D.ID LEFT OUTER JOIN Clinic as C on D.ClinicID = C.ID) AS T where ID >0 " + where;
            string sql = "select * from (select ROW_NUMBER() OVER(ORDER BY Weight Desc) as row,* from(select * from(select U.ID, U.OpenID, U.[Identity], U.Avatar, U.NickName, U.RealName, U.IDCardType, U.IDCardNumber, U.Telphone, U.Gender, U.Brithday, U.IsFullInfo, U.Weight, U.Status, U.AddTime, U.LastLoginTime,D.ClinicID, C.Title as ClinicTitle, D.TouXian, ISNULL(D.CanAdvisory,0) as CanAdvisory, ISNULL(D.AdvisoryPrice,0) as AdvisoryPrice from(select * from MPUser where [Identity] = 3) as U LEFT OUTER JOIN MPUserDoctors as D on U.ID = D.ID LEFT OUTER JOIN Clinic as C on D.ClinicID = C.ID) AS T where ID>0 " + where + ") as TT) as SS where row BETWEEN " + begin_index.ToString() + " and " + end_idnex.ToString() + "";
            using (var db = new DataCore.EFDBContext())
            {
                total = db.Database.SqlQuery<int>(sql_total).ToList()[0];
                result = db.Database.SqlQuery<Entity.ViewModel.MPUserDoctors>(sql).ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取医生实体
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static Entity.MPUser GetModel(string open_id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.MPUsers.Where(p => p.OpenID == open_id).Include(p => p.DoctorsInfo).Include(p => p.DoctorsInfo.ClinicDepartmentList).Include(p => p.DoctorsInfo.DoctorsSpecialtyList).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取咨询-医生详情中的医生Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.ViewModel.DoctorSearch GetDoctorSearchModel(int id)
        {
            if (id <= 0) return null;
            using (var db = new DataCore.EFDBContext())
            {
                string strSql = "select T.*,C.ClinicAreaID as area_id,C.Title as clinic_name,dbo.fn_GetDoctorSpecialtyNames(T.ID) as specialty_names,dbo.fn_GetDoctorDepIds(T.ID) as dep_ids,dbo.fn_GetDoctorDepNames(T.ID) as dep_names from (select U.ID as id,U.AVatar as avatar,U.RealName as name,U.Weight as weight,ISNULL(D.ClinicID, 0) as clinic_id,CONVERT(bit, ISNULL(D.CanAdvisory, 0)) as can_adv,ISNULL(D.AdvisoryPrice, 0) as adv_price,ISNULL(D.TouXian, '') as touxian,ISNULL(D.ShowMe, '') as show_me from(select * from MPUser where ID=" + id.ToString() + " and Status = 1) as U LEFT JOIN MPUserDoctors as D on U.ID = D.ID) as T LEFT JOIN(select* from Clinic where Status = 1) as C on T.clinic_id = C.ID";
                var model = db.Database.SqlQuery<Entity.ViewModel.DoctorSearch>(strSql).FirstOrDefault();
                if(model != null)
                {
                    if (model.dep_names.Length > 1)
                    {
                        model.dep_names = model.dep_names.Substring(1, model.dep_names.Length - 3);
                    }

                    foreach (var sp in model.specialty_names.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(sp)) continue;
                        model.specialt_list.Add(sp);
                    }
                }
                return model;
            }
        }

        /// <summary>
        /// 获取在线咨询页面中的医生数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="area_id">区域ID</param>
        /// <param name="hospital_id">医院ID</param>
        /// <param name="dep_id">部门ID</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        public static Entity.ViewModel.AdvisoryIndex GetAdvisoryIndex(int page_size, int page_index, int area_id, int hospital_id, int dep_id, string keyword)
        {
            Entity.ViewModel.AdvisoryIndex result = new Entity.ViewModel.AdvisoryIndex();
            if (page_size <= 0) page_size = 10;
            if (page_index <= 0) page_index = 1;
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            string strWhere = " where id > 0";
            if (area_id > 0)
            {
                strWhere += " and area_id =" + area_id.ToString();
            }
            if (hospital_id > 0)
            {
                strWhere += " and clinic_id = " + hospital_id.ToString();
            }
            if (dep_id > 0)
            {
                strWhere += " and CHARINDEX('," + dep_id.ToString() + ",', dep_ids) > 0";
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strWhere += " and (CHARINDEX('" + keyword + "', name) > 0 or CHARINDEX('" + keyword + "', clinic_name)>0 or CHARINDEX('" + keyword + "', specialty_names)>0 or CHARINDEX('" + keyword + "', dep_names)>0)";
            }

            string sql_total = "select count(1) from (select T.*,ISNULL(C.ClinicAreaID, 0) as area_id,ISNULL(C.Title, '') as clinic_name,dbo.fn_GetDoctorSpecialtyNames(T.ID) as specialty_names,dbo.fn_GetDoctorDepIds(T.ID) as dep_ids,dbo.fn_GetDoctorDepNames(T.ID) as dep_names from(select U.ID as id, U.AVatar as avatar, U.RealName as name, U.Weight as weight, ISNULL(D.ClinicID, 0) as clinic_id, CONVERT(bit, ISNULL(D.CanAdvisory, 0)) as can_adv, ISNULL(D.AdvisoryPrice, 0) as adv_price, ISNULL(D.TouXian, '') as touxian, '' as show_me from(select * from MPUser where IsFullInfo=1 and [Identity] = 3 and Status = 1) as U LEFT JOIN MPUserDoctors as D on U.ID = D.ID) as T LEFT JOIN(select* from Clinic where Status = 1) as C on T.clinic_id = C.ID) as T2 " + strWhere;
            string sql_list = "select * from(select ROW_NUMBER() OVER(ORDER BY Weight Desc) as row,*from(select * from(select T.*, ISNULL(C.ClinicAreaID, 0) as area_id, ISNULL(C.Title, '') as clinic_name, dbo.fn_GetDoctorSpecialtyNames(T.ID) as specialty_names, dbo.fn_GetDoctorDepIds(T.ID) as dep_ids, dbo.fn_GetDoctorDepNames(T.ID) as dep_names from(select U.ID as id, U.AVatar as avatar, U.RealName as name, U.Weight as weight, ISNULL(D.ClinicID, 0) as clinic_id, CONVERT(bit, ISNULL(D.CanAdvisory, 0)) as can_adv, ISNULL(D.AdvisoryPrice, 0) as adv_price, ISNULL(D.TouXian, '') as touxian, '' as show_me from(select * from MPUser where IsFullInfo=1 and [Identity] = 3 and Status = 1) as U LEFT JOIN MPUserDoctors as D on U.ID = D.ID) as T LEFT JOIN(select * from Clinic where Status = 1) as C on T.clinic_id = C.ID) as T2 " + strWhere + ") as T3) as TT where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString();

            string sql_area = "select ID as id,Title as title from ClinicArea";
            string sql_hospital = "select ID as id,Title as title,ClinicAreaID from Clinic";
            if (area_id > 0) sql_hospital += " where ClinicAreaID =" + area_id.ToString();
            string sql_dep = "select ID as id,Title as title,ClinicID from ClinicDepartment";
            if (hospital_id > 0) sql_dep += " where ClinicID =" + hospital_id.ToString();
            using (var db = new DataCore.EFDBContext())
            {
                result.doctors_total = db.Database.SqlQuery<int>(sql_total).FirstOrDefault();
                result.doctors_list = db.Database.SqlQuery<Entity.ViewModel.DoctorSearch>(sql_list).ToList();
                foreach (var item in result.doctors_list)
                {
                    if(item.dep_names.Length > 1)
                    {
                        item.dep_names = item.dep_names.Substring(1, item.dep_names.Length - 3);
                    }

                    foreach (var sp in item.specialty_names.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(sp)) continue;
                        item.specialt_list.Add(sp);
                    }
                }

                //不搜索时才显示地区医院数据
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    result.area_list = db.Database.SqlQuery<Entity.ViewModel.AdvisoryIndexDep>(sql_area).ToList();
                    result.area_list.Insert(0, new Entity.ViewModel.AdvisoryIndexDep(0, "全国"));
                    result.hospital_list = db.Database.SqlQuery<Entity.ViewModel.AdvisoryIndexDep>(sql_hospital).ToList();
                    result.hospital_list.Insert(0, new Entity.ViewModel.AdvisoryIndexDep(0, "全部医院"));
                    //筛选医院后才显示科室
                    if (hospital_id > 0) result.dep_list = db.Database.SqlQuery<Entity.ViewModel.AdvisoryIndexDep>(sql_dep).ToList();
                    result.dep_list.Insert(0, new Entity.ViewModel.AdvisoryIndexDep(0, "全部科室"));
                }
            }

            return result;
        }

        /// <summary>
        /// 获取医生实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.MPUser GetModel(int id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.MPUsers.Where(p => p.ID == id).Include(p => p.DoctorsInfo).Include(p => p.DoctorsInfo.Clinic).Include(p => p.DoctorsInfo.ClinicDepartmentList).Include(p => p.DoctorsInfo.DoctorsSpecialtyList).FirstOrDefault();
            }
        }

        /// <summary>
        /// 修改医生资料
        /// </summary>
        /// <param name="model"></param>
        /// <param name="department_ids">所属部门ID，逗号分割</param>
        /// <param name="specialty_ids">特长id，逗号分割</param>
        /// <returns></returns>
        public static bool Modify(Entity.MPUser model, string specialty_ids, string department_ids, out string msg)
        {
            msg = "";
            if (model == null) return false;
            if (model.ID <= 0) return false;
            if (model.DoctorsInfo == null) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.MPUsers.Where(p => p.ID == model.ID)
                    .Include(p => p.DoctorsInfo)
                    .Include(p => p.DoctorsInfo.DoctorsSpecialtyList)
                    .Include(p => p.DoctorsInfo.ClinicDepartmentList)
                    .FirstOrDefault();
                if (entity == null) return false;
                if (entity.Telphone != model.Telphone)
                {
                    //修改i了，判断手机号是否存在
                    if (db.MPUsers.Any(p => p.ID != model.ID && p.Telphone == model.Telphone))
                    {
                        msg = "手机号已存在";
                        return false;
                    }

                }
                if (entity.IDCardNumber != model.IDCardNumber)
                {
                    if (db.MPUsers.Any(p => p.ID != model.ID && p.IDCardNumber == model.IDCardNumber))
                    {
                        msg = "身份证号码已存在";
                        return false;
                    }
                }
                entity.Avatar = model.Avatar;
                entity.Brithday = model.Brithday;
                entity.Gender = model.Gender;
                entity.IDCardNumber = model.IDCardNumber;
                entity.IsFullInfo = true;
                entity.NickName = model.NickName;
                entity.RealName = model.RealName;
                entity.Telphone = model.Telphone;
                entity.Weight = model.Weight;

                //医生信息
                if (entity.DoctorsInfo == null) entity.DoctorsInfo = new Entity.MPUserDoctors();
                entity.DoctorsInfo.AdvisoryPrice = model.DoctorsInfo.AdvisoryPrice;
                entity.DoctorsInfo.CanAdvisory = model.DoctorsInfo.CanAdvisory;
                entity.DoctorsInfo.ClinicID = model.DoctorsInfo.ClinicID;
                entity.DoctorsInfo.ShowMe = model.DoctorsInfo.ShowMe;
                entity.DoctorsInfo.TouXian = model.DoctorsInfo.TouXian;

                //科室
                entity.DoctorsInfo.ClinicDepartmentList = null;
                if (!string.IsNullOrWhiteSpace(department_ids))
                {
                    var department_id_list = Array.ConvertAll<string, int>(department_ids.Split(','), int.Parse);
                    entity.DoctorsInfo.ClinicDepartmentList = db.ClinicDepartments.Where(p => department_id_list.Contains(p.ID)).ToList();
                }
                //特长
                entity.DoctorsInfo.DoctorsSpecialtyList = null;
                if (!string.IsNullOrWhiteSpace(specialty_ids))
                {
                    var specialty_id_list = Array.ConvertAll<string, int>(specialty_ids.Split(','), int.Parse);
                    entity.DoctorsInfo.DoctorsSpecialtyList = db.DoctorsSpecialtys.Where(p => specialty_id_list.Contains(p.ID)).ToList();
                }

                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 设置是否开启咨询
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="status">0:关闭，1：开启</param>
        /// <returns></returns>
        public static bool SetAdvisoryStatus(string open_id, int status)
        {
            using (var db = new DataCore.EFDBContext())
            {
                bool db_s = status == 0 ? false : true;
                var entity = db.MPUsers.Where(p => p.OpenID == open_id).Include(p => p.DoctorsInfo).FirstOrDefault();
                if (entity == null) return false;
                if (entity.DoctorsInfo == null)
                {
                    entity.DoctorsInfo = new Entity.MPUserDoctors();
                    entity.DoctorsInfo.AdvisoryPrice = 0;
                }
                entity.DoctorsInfo.CanAdvisory = db_s;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="price">0:关闭，1：开启</param>
        /// <returns></returns>
        public static bool SetAdvisoryPrice(string open_id, decimal price)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.MPUsers.Where(p => p.OpenID == open_id).Include(p => p.DoctorsInfo).FirstOrDefault();
                if (entity == null) return false;
                if (entity.DoctorsInfo == null)
                {
                    entity.DoctorsInfo = new Entity.MPUserDoctors();
                    entity.DoctorsInfo.CanAdvisory = false;
                }
                entity.DoctorsInfo.AdvisoryPrice = price;
                db.SaveChanges();
                return true;
            }
        }

    }
}
