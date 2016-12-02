using System;
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

        

    }
}
