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
    /// 医学通识标签
    /// </summary>
    public class BLLNewsTag
    {
        /// <summary>
        /// 获取标签实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.NewsTag GetModel(int id,out List<int> category_ids)
        {
            category_ids = new List<int>();
            using (var db=new DataCore.EFDBContext())
            {
                var entity = db.NewsTags.Where(p => p.ID == id).Include(p => p.NewsCategoryList).AsNoTracking().FirstOrDefault();
                if (entity == null) return null;
                if(entity.NewsCategoryList != null)
                {
                    foreach (var item in entity.NewsCategoryList)
                    {
                        if(item.Status) category_ids.Add(item.ID);
                    }
                }
                return entity;
            }
        }

        /// <summary>
        /// 添加一个标签
        /// </summary>
        /// <param name="title"></param>
        /// <param name="weight"></param>
        /// <param name="category_ids"></param>
        /// <returns></returns>
        public static int AddTags(string title,int weight,string category_ids)
        {
            if (string.IsNullOrWhiteSpace(title) || weight <= 0 || string.IsNullOrWhiteSpace(category_ids)) return 0;
            using (var db=new DataCore.EFDBContext())
            {
                var entity = new Entity.NewsTag();
                entity.Weight = weight;
                entity.Title = title;
                var cids_list = Array.ConvertAll<string, int>(category_ids.Split(','), int.Parse);
                var category_list = db.NewsCategorys.Where(p => cids_list.Contains(p.ID)).ToList();
                entity.NewsCategoryList = new List<Entity.NewsCategory>();
                foreach (var item in category_list)
                {
                    if (item.Status) entity.NewsCategoryList.Add(item);
                }
                db.NewsTags.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="title"></param>
        /// <param name="weight"></param>
        /// <param name="category_ids"></param>
        /// <returns></returns>
        public static bool Modify(int id,string title,int weight,string category_ids)
        {
            if (string.IsNullOrWhiteSpace(title) || id<=0 || weight <= 0 || string.IsNullOrWhiteSpace(category_ids)) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.NewsTags.Where(p => p.ID == id).Include(p => p.NewsCategoryList).FirstOrDefault();
                if (entity == null) return false;
                entity.Weight = weight;
                entity.Title = title;
                entity.NewsCategoryList = null;
                var cids_list = Array.ConvertAll<string, int>(category_ids.Split(','), int.Parse);
                var category_list = db.NewsCategorys.Where(p => cids_list.Contains(p.ID)).ToList();
                entity.NewsCategoryList = new List<Entity.NewsCategory>();
                foreach (var item in category_list)
                {
                    if (item.Status) entity.NewsCategoryList.Add(item);
                }
                db.SaveChanges();
                return true;
            }
        }


        /// <summary>
        /// 根据分类ID获取下面的标签
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public static List<Entity.ViewModel.NewsTags> GetListByCategoryID(int category_id)
        {
            List<Entity.ViewModel.NewsTags> result = new List<Entity.ViewModel.NewsTags>();
            using (var db=new DataCore.EFDBContext())
            {
                if (category_id > 0)
                {
                    var db_list = db.NewsCategorys.Where(p => p.ID == category_id).Include(p => p.NewsTags).AsNoTracking().FirstOrDefault();
                    if(db_list != null)
                    {
                        if(db_list.NewsTags != null)
                        {
                            foreach (var item in db_list.NewsTags)
                            {
                                result.Add(new Entity.ViewModel.NewsTags(item.ID, item.Title, item.Weight, category_id));
                            }
                        }
                    }
                }
                else
                {
                    var db_list = db.NewsTags.OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                    foreach (var item in db_list)
                    {
                        result.Add(new Entity.ViewModel.NewsTags(item.ID, item.Title, item.Weight, 0));
                    }
                }

            }
            return result;
        }

    }
}
