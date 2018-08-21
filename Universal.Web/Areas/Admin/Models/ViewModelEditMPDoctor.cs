using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 编辑医生信息
    /// </summary>
    public class ViewModelEditMPDoctor
    {
        public ViewModelEditMPDoctor()
        {
            this.touxian = "医师";
        }

        public int id { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [MaxLength(100),Required(ErrorMessage ="真实姓名必填")]
        public string real_name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(13),Required(ErrorMessage ="手机号必填")]
        public string telphone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Entity.MPUserGenderType Gender { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 所属诊所
        /// </summary>
        [Required(ErrorMessage ="请选择所属诊所")]
        public int clinic_id { get; set; }

        /// <summary>
        /// 所属科室
        /// </summary>
        [Required(ErrorMessage ="请选择所属科室")]
        public string dep_ids { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        public string touxian { get; set; }

        /// <summary>
        /// 特长标签
        /// </summary>
        [Required(ErrorMessage ="请选择特长标签")]
        public string shanchang { get; set; }

        /// <summary>
        /// 医师简介
        /// </summary>
        [Required(ErrorMessage ="请填写医师简介")]
        public string show_me { get; set; }

        /// <summary>
        /// 是否可以资讯
        /// </summary>
        public bool can_adv { get; set; }

        /// <summary>
        /// 资讯价格
        /// </summary>
        public decimal adv_price { get; set; }

    }
}