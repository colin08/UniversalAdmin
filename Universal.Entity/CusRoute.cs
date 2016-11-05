using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 前台权限
    /// </summary>
    public class CusRoute
    {

        public int ID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [Required,MaxLength(30)]
        public string Title { get; set; }

        /// <summary>
        /// 对应控制器名称
        /// </summary>
        [Required,MaxLength(30)]
        public string ControllerName { get; set; }
        
    }
}
