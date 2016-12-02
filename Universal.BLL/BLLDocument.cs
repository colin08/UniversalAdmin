﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 秘籍数据操作
    /// </summary>
    public class BLLDocument
    {
        /// <summary>
        /// 删除秘籍
        /// </summary>
        /// <returns></returns>
        public static bool Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return false;

            //删除秘籍的同时，删除收藏
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll_fav = new BaseBLL<Entity.CusUserDocFavorites>();
            bll_fav.DelBy(p => id_list.Contains(p.DocPostID));

            BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
            bll.DelBy(p => id_list.Contains(p.ID));

            return true;
        }

        /// <summary>
        /// 获取登录用户可见的秘籍
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id"></param>
        /// <param name="search_title"></param>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public static List<Entity.DocPost> GetPowerPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title, int category_id)
        {

            /** 设计原图
             ---根据秘籍设置的权限，获取用户可见的秘籍
            declare @user_department_id int,@user_id int,@user_department_str nvarchar(20),@user_id_str nvarchar(20)
            set @user_id = 1
            select @user_department_id = CusDepartmentID from CusUser where id = @user_id
            set @user_department_str = ','+ Cast(@user_department_id as nvarchar(20))+','
            set @user_id_str = ','+cast(@user_id as nvarchar(20)) +','
            print @user_department_str
            print @user_id_str

            select * from (
            SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row,* FROM (select * from  [dbo].[DocPost] where DocCategoryID =1 and CHARINDEX(N'权限',Title) > 0 ) as S where See = 0 or CHARINDEX((case See when 2 then @user_id_str when 1 then @user_department_str end),
	            (case CusUserID when @user_id then (case See when 2 then @user_id_str when 1 then @user_department_str end) end)+TOID)> 0
            ) as T 

            --select * from (
            --SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row,* FROM [dbo].[DocPost] where DocCategoryID =1 and See = 0 or CHARINDEX((case See when 2 then @user_id_str when 1 then @user_department_str end),
            --	(case CusUserID when @user_id then (case See when 2 then @user_id_str when 1 then @user_department_str end) end)+TOID)> 0
            --) as T  where CHARINDEX(N'权限',Title) > 0

            --update DocPost set TOID = ',1,4,' where ID = 4
             * */

            rowCount = 0;

            List<Entity.DocPost> response_entity = new List<Entity.DocPost>();
            if (user_id == 0)
                return response_entity;

            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            var db = new DataCore.EFDBContext();
            string sql = "";
            string sql_total = "";
            int user_department_id = 0;
            string user_department_str = "";
            string user_id_str = "";
            var entity_user = db.CusUsers.Find(user_id);

            if (entity_user == null)
                return response_entity;
            else
            {
                user_department_id = entity_user.CusDepartmentID;
                user_department_str = "," + user_department_id + ",";
                user_id_str = "," + user_id + ",";
            }
            
            if (!string.IsNullOrWhiteSpace(search_title))
            {
                sql = "select * from(SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row, * FROM(select * from[dbo].[DocPost] where  CHARINDEX(N'"+ search_title + "', Title) > 0) as S where See = 0 or CHARINDEX((case See when 2 then '"+ user_id_str + "' when 1 then '"+ user_department_str + "' end),(case CusUserID when "+user_id+" then(case See when 2 then '"+ user_id_str + "' when 1 then '"+ user_department_str + "' end) end) + TOID)> 0) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) FROM(select * from[dbo].[DocPost] where  CHARINDEX(N'" + search_title + "', Title) > 0) as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + TOID)> 0";
            }

            if (category_id > 0)
            {
                sql = "select * from(SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row, * FROM(select * from[dbo].[DocPost] where  DocCategoryID ="+category_id.ToString()+") as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + TOID)> 0) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) FROM(select * from[dbo].[DocPost] where  DocCategoryID =" + category_id.ToString() + ") as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + TOID)> 0";
            }

            if (!string.IsNullOrWhiteSpace(search_title) && category_id > 0)
            {
                sql = "select * from(SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row, * FROM(select * from[dbo].[DocPost] where  DocCategoryID =" + category_id.ToString() + " and CHARINDEX(N'" + search_title + "', Title) > 0) as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + TOID)> 0) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) FROM(select * from[dbo].[DocPost] where  DocCategoryID =" + category_id.ToString() + " and CHARINDEX(N'" + search_title + "', Title) > 0) as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + TOID)> 0";
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
