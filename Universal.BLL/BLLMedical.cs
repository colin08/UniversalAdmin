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
    /// 体检套餐操作类
    /// </summary>
    public class BLLMedical
    {

        /// <summary>
        /// 添加一个套餐
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        public static int Add(Entity.Medical entity,string item_ids)
        {
            if (entity == null) return 0;
            using (var db = new DataCore.EFDBContext())
            {
                entity.MedicalItems = new List<Entity.MedicalItem>();
                entity.Desc = entity.Desc.Replace("_thumb", "");
                db.Medicals.Add(entity);
                foreach (var item in item_ids.Split(','))
                {
                    int item_id = Tools.TypeHelper.ObjectToInt(item,0);
                    var entity_temp = db.MedicalItems.Find(item_id);
                    if(entity_temp != null)
                    {
                        entity.MedicalItems.Add(entity_temp);
                    }
                }
                db.SaveChanges();
            }
            return entity.ID;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="item_ids"></param>
        /// <returns></returns>
        public static bool Modify(Entity.Medical entity,string item_ids)
        {

            if (entity == null) return false;
            using (var db = new DataCore.EFDBContext())
            {
                entity.MedicalItems = new List<Entity.MedicalItem>();
                var model = db.Medicals.Where(p => p.ID == entity.ID).Include(p => p.MedicalItems).FirstOrDefault();
                if (model == null) return false;
                model.Status = entity.Status;
                model.ImgUrl = entity.ImgUrl;
                model.Price = entity.Price;
                model.YPrice = entity.YPrice;
                model.Title = entity.Title;
                model.Weight = entity.Weight;
                model.Desc = entity.Desc.Replace("_thumb", "");
                model.MedicalItems = new List<Entity.MedicalItem>();
                foreach (var item in item_ids.Split(','))
                {
                    int item_id = Tools.TypeHelper.ObjectToInt(item, 0);
                    var entity_temp = db.MedicalItems.Find(item_id);
                    if (entity_temp != null)
                    {
                        model.MedicalItems.Add(entity_temp);
                    }
                }

                db.SaveChanges();
            }
            return true;
        }


        /// <summary>
        /// 批量禁用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DisEnble(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return false;
            using (var db = new DataCore.EFDBContext())
            {
                string strSql = "update Medical set Status=2 where id in(" + ids + ")";
                db.Database.ExecuteSqlCommand(strSql);
                return true;
            }
        }

        /// <summary>
        /// 获取所有的体检项
        /// </summary>
        /// <param name="SZMList"></param>
        /// <returns></returns>
        public static List<Entity.MedicalItem> LoadAllSelectSZMList(out List<string> SZMList)
        {
            SZMList = new List<string>();
            using (var db=new DataCore.EFDBContext())
            {
                var db_list = db.MedicalItems.Where(p => p.Status).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                SZMList = db.Database.SqlQuery<string>("select SZM from  [dbo].[MedicalItem] group by SZM").ToList();
                return db_list;
            }
        }

    }
}
