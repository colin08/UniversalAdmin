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
                var entity_fav = db.CusUserDocFavorites.Where(p => p.CusUserID == user_id && p.DocPostID == id).FirstOrDefault();
                if(entity_fav == null)
                {
                    //添加
                    msg = "收藏成功";
                    var entity = new Entity.CusUserDocFavorites();
                    entity.CusUserID = user_id;
                    entity.DocPostID = id;
                    db.CusUserDocFavorites.Add(entity);
                }
                else
                {
                    //删除
                    msg = "取消收藏成功";
                    db.CusUserDocFavorites.Remove(entity_fav);
                }                
                db.SaveChanges();

            };
            return true;
        }

        /// <summary>
        /// 添加项目收藏
        /// </summary>
        /// <param name="id">项目ID</param>
        /// <param name="type">用户ID></param>
        /// <returns></returns>
        public static bool AddProjectFav(int id, int user_id, out string msg)
        {
            using (var db = new DataCore.EFDBContext())
            {
                if (!db.Projects.Any(p => p.ID == id))
                {
                    msg = "项目不存在";
                    return false;
                }
                if (!db.CusUsers.Any(p => p.ID == user_id))
                {
                    msg = "用户不存在";
                    return false;
                }
                var entity_fav = db.CusUserProjectFavorites.Where(p => p.CusUserID == user_id && p.ProjectID == id).FirstOrDefault();
                if (entity_fav == null)
                {
                    //添加
                    msg = "收藏成功";
                    var entity = new Entity.CusUserProjectFavorites();
                    entity.CusUserID = user_id;
                    entity.ProjectID = id;
                    db.CusUserProjectFavorites.Add(entity);
                }else
                {
                    msg = "取消收藏成功";
                    db.CusUserProjectFavorites.Remove(entity_fav);
                }                
                db.SaveChanges();

            };
            return true;
        }

        /// <summary>
        /// 获取秘籍收藏分页列表
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id">哪个用户的收藏</param>
        /// <param name="search_title">搜索标题</param>
        /// <param name="category_id">所属分类</param>
        /// <returns></returns>
        public static List<Entity.DocPost> GetDocPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title, int category_id)
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

        /// <summary>
        /// 获取项目收藏分页列表
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id">哪个用户的收藏</param>
        /// <param name="search_title">搜索标题</param>
        /// <param name="category_id">所属分类</param>
        /// <returns></returns>
        public static List<Entity.Project> GetProjectPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title)
        {
            rowCount = 0;
            List<Entity.Project> response_entity = new List<Entity.Project>();
            if (user_id == 0)
                return response_entity;

            var db = new DataCore.EFDBContext();
            string sql = "";
            string sql_total = "";
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            if (!string.IsNullOrWhiteSpace(search_title))
            {
                sql = "select * from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row, *from( select W.*, U.NickName, u.Telphone from(select pro.*, fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID = " + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID = u.ID) as T where T.NickName like '%" + search_title + "%' or T.Title like '%" + search_title + "%') AS Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select W.*,U.NickName,u.Telphone from (select pro.Title,pro.CusUserID,pro.ApproveUserID,See,TOID,pro.AddTime,LastUpdateTime,fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID = " + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID =u.ID) as T where T.NickName like '%" + search_title + "%' or T.Title like '%" + search_title + "%'";
            }else
            {
                sql = "select * from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row,* from (select W.*,U.NickName,u.Telphone from (select pro.*,fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID =" + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID =u.ID) as T) as Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select W.*,U.NickName,u.Telphone from (select pro.Title,pro.CusUserID,pro.ApproveUserID,See,TOID,pro.AddTime,LastUpdateTime,fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID =" + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID =u.ID) as T";
            }
            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.Project>(sql).ToList();
            foreach (var item in response_entity)
            {
                var entity = db.CusUsers.AsNoTracking().Where(p => p.ID == item.CusUserID).FirstOrDefault();
                if (entity == null)
                {
                    entity = new Entity.CusUser();
                }
                item.CusUser = entity;
                item.ProjectFiles = db.ProjectFiles.AsNoTracking().Where(p => p.ProjectID == item.ID).ToList();
                //获取当前进行的节点信息
                //如果项目已完成，则直接显示已完成，否则查找最后正在进行的节点，如果查找不到，则查找初始节点，标识未未开始
                var entity_node = new Entity.Node();
                if (!item.Status)
                {
                    entity_node = db.Nodes.SqlQuery("select N.* from (select top 1 * from ProjectFlowNode where ProjectID = " + item.ID.ToString() + " and Status = 1 and IsStart = 1 order by[Index] DESC) as F left JOIN Node as N on F.NodeID = N.ID").FirstOrDefault();
                    if (entity_node == null)
                    {
                        entity_node = db.Nodes.SqlQuery("select N.* from (select top 1 * from ProjectFlowNode where ProjectID = " + item.ID.ToString() + " order by[Index] ASC) as F left JOIN Node as N on F.NodeID = N.ID").FirstOrDefault();
                    }

                }
                if (entity_node == null)
                {
                    entity_node = new Entity.Node();
                    entity_node.Title = "无节点";
                    entity_node.Content = "无节点";
                }

                item.NowNode = entity_node;
            }
            db.Dispose();
            return response_entity;
        }
        
    }
}
