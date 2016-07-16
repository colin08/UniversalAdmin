using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    

    /// <summary>
    ///  后台控制器权限管理扩展属性
    /// </summary>
    public class AdminPermissionAttribute : System.Attribute
    {

        /// <summary>
        /// 权限标签，操作时将以此为分组，方便查看(例如：用户、日志)
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 操作说明
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Tag">权限标签，操作时将以此为分组，方便查看(例如：用户、日志)</param>
        /// <param name="Desc">说明</param>
        public AdminPermissionAttribute(string Tag,string Desc)
        {
            this.Tag = Tag;
            this.Desc = Desc;
        }

    }
}
