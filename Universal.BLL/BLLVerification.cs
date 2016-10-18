using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 验证码处理类
    /// </summary>
    public class BLLVerification
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="telphone">手机号</param>
        /// <param name="guid">标识</param>
        /// <param name="type">验证码类别</param>
        /// <returns></returns>
        public static bool Send(string telphone,Guid guid,Entity.CusVerificationType type,out string msg)
        {
            msg = "OK";
            if(string.IsNullOrWhiteSpace(telphone) || guid == null)
            {
                msg = "手机号或标识不能为空";
                return false;
            }
            if(!Tools.ValidateHelper.IsMobile(telphone))
            {
                msg = "非法手机号";
                return false;
            }

            var code = Tools.WebHelper.GenerateRandomIntNumber(6);
            string body = "您的验证码是：" + code + ",30分钟内有效，请及时验证.";
            if(!Tools.SMSHelper.SendVer(telphone,body))
            {
                msg = "短信发送失败";
                return false;
            }

            BaseBLL<Entity.CusVerification> bll = new BaseBLL<Entity.CusVerification>();
            bll.DelBy(p => p.Guid == guid);
            var entity = new Entity.CusVerification();
            entity.AddTime = DateTime.Now;
            entity.Code = code;
            entity.Guid = guid;
            entity.Type = type;
            bll.Add(entity);
            return true;
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="guid">标识</param>
        /// <param name="type">类型</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public static bool Check(Guid guid,Entity.CusVerificationType type,string code)
        {
            if (string.IsNullOrWhiteSpace(code) || guid == null)
                return false;

            BaseBLL<Entity.CusVerification> bll = new BaseBLL<Entity.CusVerification>();
            var entity = bll.GetModel(p => p.Guid == guid && p.Type == type);
            if (entity == null)
                return false;

            if ((entity.AddTime - DateTime.Now).Minutes > 30)
                return false;

            if (entity.Code != code.Trim())
                return false;
            else
            {
                bll.Del(entity);
                return true;
            }

        }
    }
}
