using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 系统需要权限控制的路由表
    /// </summary>
    public class SysRoute
    {
        public int ID { get; set; }

        /// <summary>
        /// 权限标签，操作时将以此为分组，方便查看(例如：用户、日志)
        /// </summary>
        [MaxLength(30)]
        public string Tag { get; set; }

        /// <summary>
        /// 是否Post请求
        /// </summary>
        public bool IsPost { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [MaxLength(30)]
        public string Route { get; set; }

        /// <summary>
        /// 路由说明
        /// </summary>
        [MaxLength(30)]
        public string Desc { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

    }
}
