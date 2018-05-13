using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class BLLVerificationCode
    {
        /// <summary>
        /// 添加验证码
        /// </summary>
        /// <param name="telphone"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Add(string telphone, Entity.VerificationCodeType type, out string msg)
        {
            msg = "验证码发送成功";
            if (string.IsNullOrWhiteSpace(telphone)) { msg = "手机号不能为空"; return false; }
            if (!ValidateHelper.IsMobile(telphone)) { msg = "非法手机号"; return false; }
            string code = WebHelper.GenerateRandomIntNumber(4);
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.VerificationCodes.Where(p => p.Type == type && p.Telphone == telphone).FirstOrDefault();
                if (entity != null)
                {
                    //已有验证码，判断发送时间
                    if (WebHelper.DateTimeDiff(entity.AddTime, DateTime.Now, "as") > 60)
                    {
                        //距离上次发送已超过60分钟，则重新设置验证码和时间
                        entity.AddTime = DateTime.Now;
                        entity.Code = code;
                    }
                    else
                    {
                        msg = "请一分钟后重新获取";
                        return false;
                    }
                }
                else
                {
                    //没有，添加
                    entity = new Entity.VerificationCode();
                    entity.Type = type;
                    entity.Code = code;
                    entity.Telphone = telphone;
                    db.VerificationCodes.Add(entity);
                }
                db.SaveChanges();

                //发送操作
                string sms_text = "";
                switch (type)
                {
                    case Entity.VerificationCodeType.FullInfo:
                        sms_text = "您正在进行资料完善操作，验证码:{0},三十分钟内有效。";
                        break;
                    default:
                        break;
                }
                var status = SMSHelper.SendVer(telphone, string.Format(sms_text, code));
                if (!status) { msg = "验证码发送失败"; return false; };
            }
            return true;
        }

        /// <summary>
        /// 验证操作
        /// </summary>
        /// <returns></returns>
        public static bool VerCode(string telphone, Entity.VerificationCodeType type, string code, out string msg)
        {
            msg = "验证成功";
            if (string.IsNullOrWhiteSpace(telphone)) { msg = "手机号不能为空"; return false; }
            if (!ValidateHelper.IsMobile(telphone)) { msg = "非法手机号"; return false; }
            if (string.IsNullOrWhiteSpace(code)) { msg = "验证码不能为空"; return false; }
            if (code.Length != 4) { msg = "验证码格式非法"; return false; }
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.VerificationCodes.Where(p => p.Type == type && p.Telphone == telphone).FirstOrDefault();
                if(entity == null) { msg = "还没有发送验证码";return false; }
                if(entity.Code != code) { msg = "验证码错误"; return false; }
                if(WebHelper.DateTimeDiff(entity.AddTime,DateTime.Now,"am") > 30) { msg = "验证码已过期"; return false; }
                db.VerificationCodes.Remove(entity);
                db.SaveChanges();
            }
            return true;
        }

    }
}
