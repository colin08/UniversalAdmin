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
        /// 添加收藏
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Add(int id,int user_id,Entity.CusUserFavoritesType type)
        {
            using (var db = new DataCore.EFDBContext())
            {
                switch (type)
                {
                    case Entity.CusUserFavoritesType.project:
                        if (!db.DocPosts.Any(p => p.ID == id))
                            return false;
                        break;
                    case Entity.CusUserFavoritesType.docment:
                        //TODO 项目收藏判断未做
                        break;
                    default:
                        break;
                }

                var entity = new Entity.CusUserFavorites();
                entity.CusUserID = user_id;
                entity.TOID = id;
                entity.Type = type;
                db.CusUserFavorites.Add(entity);
                db.SaveChanges();

            };
            return true;
        }
    }
}
