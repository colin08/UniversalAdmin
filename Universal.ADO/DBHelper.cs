using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Universal.ADO
{
    /// <summary>
    /// ADO.NET数据库交互
    /// </summary>
    public static class DBHelper
    {
        //要引入using System.Configuration命名空间

        public static string GetConnStr()
        {
            return ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
        }

        /// <summary>
        ///  执行SQL语句，返回受影响的行数(用于insert,delete,update等)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSql(string strSQL)
        {
            //using: 释放资源
            //SqlConnection: 连接数据源
            //GetConnStr():用于连接数据源的字符串
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                //SqlCommand对数据源执行SQl命令
                //SqlCommand(CommandText, CommandString)：strSQL需执行的SQL命令字符串,conn指定执行SQL命令需连接的数据源
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        int rows = cmd.ExecuteNonQuery();//执行SQL语句,返回受影响的行数。如rows>0，说明执行成功
                        return rows;
                    }
                    catch (SqlException e)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(e.StackTrace);
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 执行带参数的非查询SQL
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="values">参数</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteCommand(string strSQL, params SqlParameter[] values)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        cmd.Parameters.AddRange(values);
                        int rows = cmd.ExecuteNonQuery();//执行SQL语句,返回受影响的行数。如rows>0，说明执行成功
                        return rows;
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        ///  执行查询SQL语句，返回SqlDataReader(只进记录集) ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        //CommandBehavior.CloseConnection 能够保证当SqlDataReader对象被关闭时，其依赖的连接也会被自动关闭。
                        SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        return myReader;
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return null;
                    }
                }
            }
        }

        /// <summary>
        ///  执行带参数的查询SQL语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <param name="values">参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL, params SqlParameter[] values)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        //CommandBehavior.CloseConnection 能够保证当SqlDataReader对象被关闭时，其依赖的连接也会被自动关闭。
                        cmd.Parameters.AddRange(values);
                        SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        return myReader;
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return null;
                    }
                }
            }
        }

        ///////DataReader和DataSet的区别
        ///////1.获取数据的方式:
        ///////    DataReader(在线-一直占用SqlConnection连接,其它操作就可以不可使用，占用内存较小)
        ///////    DataSet(离线-将数据一次性读入内存，然后断开连接，其它操作就可以使用,比较消耗资源)
        ///////2．获取数据的机制
        ///////    DataReader是通过IDbCommand.ExecuteReader来读取数据。
        ///////    DataSet则是通过DbDataAdapter.Fill来填充数据
        ///////3．其它区别
        ///////    DataReader读取速度快于DataSet。
        ///////    DataReader是数据提供者类，DataSet是一般性类，借助于DbDataAdapter来填充数据。
        ///////...............................等

        /// <summary>
        ///  执行查询SQL语句，返回离线记录集
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>离线记录DataSet</returns>
        public static DataSet getDataSetbySQL(string strSQL)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        DataSet ds = new DataSet();
                        SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                        myAdapter.Fill(ds);
                        return ds;
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return new DataSet();
                    }
                }
            }
        }

        /// <summary>
        ///  执行带参数的查询SQL，返回离线记录集
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataSet getDataSetbySQL(string strSQL, params SqlParameter[] values)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        DataSet ds = new DataSet();
                        cmd.Parameters.AddRange(values);
                        SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                        myAdapter.Fill(ds);
                        return ds;
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return new DataSet();
                    }
                }
            }
        }

        /// <summary>
        ///  执行查询SQL语句，返回离线记录集
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <returns>离线记录DataSet</returns>
        public static DataTable getDataTablebySQL(string strSQL)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        DataSet ds = new DataSet();
                        SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                        myAdapter.Fill(ds);
                        return ds.Tables[0];
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return new DataTable();
                    }
                }
            }
        }

        /// <summary>
        ///  执行带参数的查询SQL，返回离线记录集
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static DataTable getDataTablebySQL(string strSQL, params SqlParameter[] values)
        {
            using (SqlConnection conn = new SqlConnection(GetConnStr()))
            {
                using (SqlCommand cmd = new SqlCommand(strSQL, conn))
                {
                    try
                    {
                        conn.Open();//打开数据源连接
                        DataSet ds = new DataSet();
                        cmd.Parameters.AddRange(values);
                        SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                        myAdapter.Fill(ds);
                        return ds.Tables[0];
                    }
                    catch (SqlException ex)
                    {
                        conn.Close();//出异常,关闭数据源连接
                        System.Diagnostics.Trace.WriteLine(string.Format("执行{0}失败:{1}", strSQL, ex.Message));
                        return new DataTable();
                    }
                }
            }
        }
    }
}
