using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    public class BllCusUserFavorites
    {
        /// <summary>
        /// 添加秘籍收藏
        /// </summary>
        /// <param name="id">秘籍ID</param>
        /// <param name="type"用户ID></param>
        /// <returns></returns>
        public static bool AddDocFav(int id, int user_id, out string msg)
        {
            using (var db = new DataCore.EFDBContext())
            {
                if (!db.DocPosts.Any(p => p.ID == id))
                {
                    msg = "秘籍不存在";
                    return false;
                }
                if (!db.CusUsers.Any(p => p.ID == user_id))
                {
                    msg = "用户不存在";
                    return false;
                }
                if (db.CusUserDocFavorites.Any(p => p.CusUserID == user_id && p.DocPostID == id))
                {
                    msg = "已经收藏过了";
                    return false;
                }

                var entity = new Entity.CusUserDocFavorites();
                entity.CusUserID = user_id;
                entity.DocPostID = id;
                db.CusUserDocFavorites.Add(entity);
                db.SaveChanges();

            };
            msg = "收藏成功";
            return true;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id">哪个用户的收藏</param>
        /// <param name="search_title">搜索标题</param>
        /// <param name="category_id">所属分类</param>
        /// <returns></returns>
        public static List<Entity.DocPost> GetPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title, int category_id)
        {
            rowCount = 0;
            List<Entity.DocPost> response_entity = new List<Entity.DocPost>();
            if (user_id == 0)
                return response_entity;

            var db = new DataCore.EFDBContext();
            string sql = "";
            string sql_total = "";
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            if (!string.IsNullOrWhiteSpace(search_title))
            {
                sql = "select ID,Title,See,DocCategoryID,CusUserID,FilePath,FileSize,AddTime,LastUpdateTime,TOID from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row,* from (select doc.*,fav.AddTime as FavTime from CusUserDocFavorites as fav left join DocPost as doc on fav.DocPostID = doc.ID where fav.CusUserID =" + user_id.ToString() + ") as T where T.Title like '%" + search_title + "%') AS Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select doc.*,fav.AddTime as FavTime from CusUserDocFavorites as fav left join DocPost as doc on fav.DocPostID = doc.ID where fav.CusUserID =" + user_id.ToString() + ") as T where T.Title like '%" + search_title + "%'";
            }

            if (category_id > 0)
            {
                sql = "select ID,Title,See,DocCategoryID,CusUserID,FilePath,FileSize,AddTime,LastUpdateTime,TOID from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row,* from (select doc.*,fav.AddTime as FavTime from CusUserDocFavorites as fav left join DocPost as doc on fav.DocPostID = doc.ID where fav.CusUserID =" + user_id.ToString() + ") as T where T.DocCategoryID = " + category_id + ") AS Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select doc.*,fav.AddTime as FavTime from CusUserDocFavorites as fav left join DocPost as doc on fav.DocPostID = doc.ID where fav.CusUserID =" + user_id.ToString() + ") as T where T.DocCategoryID = " + category_id + "";
            }

            if (!string.IsNullOrWhiteSpace(search_title) && category_id > 0)
            {
                sql = "select ID,Title,See,DocCategoryID,CusUserID,FilePath,FileSize,AddTime,LastUpdateTime,TOID from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row,* from (select doc.*,fav.AddTime as FavTime from CusUserDocFavorites as fav left join DocPost as doc on fav.DocPostID = doc.ID where fav.CusUserID =" + user_id.ToString() + ") as T where T.DocCategoryID = " + category_id + " and T.Title like '%" + search_title + "%') AS Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select doc.*,fav.AddTime as FavTime from CusUserDocFavorites as fav left join DocPost as doc on fav.DocPostID = doc.ID where fav.CusUserID =" + user_id.ToString() + ") as T where T.DocCategoryID = " + category_id + " and T.Title like '%" + search_title + "%'";
            }
            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.DocPost>(sql).ToList();
            foreach (var item in response_entity)
            {
                var entity = db.CusUsers.Find(item.CusUserID);
                if (entity == null)
                {
                    entity = new Entity.CusUser();
                }
                item.CusUser = entity;
            }
            db.Dispose();
            return response_entity;
        }

    }
}
