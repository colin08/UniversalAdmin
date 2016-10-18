﻿using System;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 异常数据库添加到数据库
    /// </summary>
    public static class ExceptionInDB
    {
        /// <summary>
        /// 异步添加异常日志到数据库
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        public static void ToInDB(Exception ex)
        {
            Tools.WebSiteModel model = Tools.ConfigHelper.LoadConfig<Tools.WebSiteModel>(Tools.ConfigFileEnum.SiteConfig);
            if (!model.LogExceptionInDB)
                return;
            string msg = ex.Message;
            if(msg.Length > 255)
               msg = msg.Substring(0, 255);
            var entity = new Entity.SysLogException()
            {
                AddTime = DateTime.Now,
                Message = msg,
                Source = ex.Source,
                StackTrace = ex.StackTrace
            };

            BLL.BaseBLL<Entity.SysLogException> bll = new BLL.BaseBLL<Entity.SysLogException>();
            bll.Add(entity);
        }
    }
}
