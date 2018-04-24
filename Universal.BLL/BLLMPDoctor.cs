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
            string sql = "select * from (select ROW_NUMBER() OVER(ORDER BY Weight Desc) as row,*from(select * from(select U.ID, U.OpenID, U.[Identity], U.Avatar, U.NickName, U.RealName, U.IDCardType, U.IDCardNumber, U.Telphone, U.Gender, U.Brithday, U.IsFullInfo, U.Weight, U.Status, U.AddTime, U.LastLoginTime,D.ClinicID, C.Title as ClinicTitle, D.TouXian, ISNULL(D.CanAdvisory,0) as CanAdvisory, ISNULL(D.AdvisoryPrice,0) as AdvisoryPrice from(select * from MPUser where [Identity] = 3) as U LEFT OUTER JOIN MPUserDoctors as D on U.ID = D.ID LEFT OUTER JOIN Clinic as C on D.ClinicID = C.ID) AS T where ID>0 " + where + ") as TT) as SS where row BETWEEN " + begin_index.ToString() + " and " + end_idnex.ToString() + "";
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
            using (var db=new DataCore.EFDBContext())
            {
                return db.MPUsers.Where(p => p.OpenID == open_id).Include(p => p.DoctorsInfo).Include(p => p.DoctorsInfo.ClinicDepartmentList).Include(p => p.DoctorsInfo.DoctorsSpecialtyList).FirstOrDefault();                
            }
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
                return db.MPUsers.Where(p => p.ID == id).Include(p => p.DoctorsInfo).Include(p=>p.DoctorsInfo.Clinic).Include(p => p.DoctorsInfo.ClinicDepartmentList).Include(p => p.DoctorsInfo.DoctorsSpecialtyList).FirstOrDefault();
            }
        }

        /// <summary>
        /// 修改医生资料
        /// </summary>
        /// <param name="model"></param>
        /// <param name="department_ids">所属部门ID，逗号分割</param>
        /// <param name="specialty_ids">特长id，逗号分割</param>
        /// <returns></returns>
        public static bool Modify(Entity.MPUser model,string specialty_ids,string department_ids,out string msg)
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
                if(entity.Telphone != model.Telphone)
                {
                    //修改i了，判断手机号是否存在
                    if(db.MPUsers.Any(p=>p.ID != model.ID &&p.Telphone == model.Telphone))
                    {
                        msg = "手机号已存在";
                        return false;
                    }

                }
                if(entity.IDCardNumber != model.IDCardNumber)
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
                if(!string.IsNullOrWhiteSpace(department_ids))
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
        public static bool SetAdvisoryStatus(string  open_id,int status)
        {
            using (var db=new DataCore.EFDBContext())
            {
                bool db_s = status == 0 ? false : true;
                var entity = db.MPUsers.Where(p => p.OpenID == open_id).Include(p => p.DoctorsInfo).FirstOrDefault();
                if (entity == null) return false;
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
                entity.DoctorsInfo.AdvisoryPrice = price;
                db.SaveChanges();
                return true;
            }
        }

    }
}
