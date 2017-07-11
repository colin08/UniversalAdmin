using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 商户信息
    /// </summary>
    public class SysMerchant
    {
        public SysMerchant()
        {
            this.Remark = "";
            this.AddTime = DateTime.Now;
            this.Status = true;
            this.LastLoginTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "商户名称"), Index(IsUnique = true), MaxLength(50), Required(ErrorMessage = "商户名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "商户说明"), MaxLength(1000)]
        public string Remark { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 仅超级商家使用(超级商家指我们)
        /// </summary>
        public bool IsSuperMch { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 商户下用户最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 获取访问链接
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="des3_key">3DES加密Key</param>
        /// <returns></returns>
        public string GetLinkUrl(string domain,string des3_key)
        {
            if (this.ID <= 0 || string.IsNullOrWhiteSpace(des3_key) || string.IsNullOrWhiteSpace(domain))
                return domain;
            Tools.Crypto3DES des = new Tools.Crypto3DES(des3_key);
            string m = des.DESEnCode(this.ID.ToString());
            string link = "{0}/Admin/Home/Login?m={1}";
            return string.Format(link, domain, m);
        }
    }
}
