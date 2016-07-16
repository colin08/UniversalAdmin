using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static async Task ToInDB(Exception ex)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = new DataCore.Entity.SysLogException()
                {
                    AddTime = DateTime.Now,
                    Message = ex.Message,
                    Source = ex.Source,
                    StackTrace = ex.StackTrace
                };
                
                db.SysLogExceptions.Add(entity);
                await db.SaveChangesAsync();
            }
        }
    }
}
