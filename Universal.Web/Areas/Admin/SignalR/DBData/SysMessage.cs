//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;

//namespace Universal.Web.Areas.Admin.SignalR
//{
//    /// <summary>
//    /// 获取系统消息
//    /// </summary>
//    public class SysMessage
//    {
//        string SqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

//        public IEnumerable<Entity.SysMessage> GetData()
//        {
//            using (var connection = new SqlConnection(SqlConnection))
//            {
//                connection.Open();
//                using (SqlCommand command = new SqlCommand(@"select ID,Content,IsRead,OpenNewTab,LinkUrl,AddTime from dbo.SysMessage where IsRead =0 order by IsRead DESC,AddTime Desc", connection))
//                {
//                    command.Notification = null;
//                    SqlDependency dependency = new SqlDependency(command);
//                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

//                    if (connection.State == ConnectionState.Closed)
//                        connection.Open();

//                    using (var reader = command.ExecuteReader())
//                        return reader.Cast<IDataRecord>()
//                            .Select(x => new Entity.SysMessage()
//                            {
//                                ID = x.GetInt32(0),
//                                Content = x.GetString(1),
//                                IsRead = x.GetBoolean(2),
//                                OpenNewTab = x.GetBoolean(3),
//                                LinkUrl = x.GetString(4),
//                                AddTime = x.GetDateTime(5)
//                            }).ToList();
//                }
//            }
//        }

//        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
//        {
//            SysMessageHub.Show();
//        }

//    }
//}