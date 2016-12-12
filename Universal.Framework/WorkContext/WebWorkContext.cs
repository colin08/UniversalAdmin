using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class WebWorkContext:BaseWorkContext
    {
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public Entity.CusUser UserInfo;

        /// <summary>
        /// 管理首页
        /// </summary>
        public string ManagerHome { get; set; }

        /// <summary>
        /// 消息数量
        /// </summary>
        public int MessageTotal { get; set; }

        /// <summary>
        /// 未读消息列表
        /// </summary>
        public List<Entity.CusUserMessage> UnReadMessageTopList { get; set; }

        /// <summary>
        /// 用户收藏列表
        /// </summary>
        public List<BLL.Model.LayoutFavorites> UserFavoritesList { get; set; }
        
    }
}
