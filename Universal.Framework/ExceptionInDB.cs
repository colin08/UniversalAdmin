using System;

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

            var entity = new Entity.SysLogException()
            {
                AddTime = DateTime.Now,
                Message = ex.Message,
                Source = ex.Source,
                StackTrace = ex.StackTrace
            };

            new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                BLL.BaseBLL<Entity.SysLogException> bll = new BLL.BaseBLL<Entity.SysLogException>();
                bll.Add(entity);
            })){ IsBackground = true }.Start();

        }
    }
}
