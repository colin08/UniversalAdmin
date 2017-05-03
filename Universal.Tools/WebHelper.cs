using System;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Security;
using System.Net.Cache;
using System.Diagnostics;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Universal.Tools
{
    /// <summary>
    /// Web帮助类
    /// </summary>
    public class WebHelper
    {
        //浏览器列表
        private static string[] _browserlist = new string[] { "ie", "chrome", "mozilla", "netscape", "firefox", "opera", "konqueror" };
        //搜索引擎列表
        private static string[] _searchenginelist = new string[] { "baidu", "google", "360", "sogou", "bing", "msn", "sohu", "soso", "sina", "163", "yahoo", "jikeu" };
        //meta正则表达式
        private static Regex _metaregex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        //防SQL注入正则表达式
        private static Regex _sqlkeywordregex = new Regex(@"(select|insert|delete|from|count\(|drop|table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec|master|net|local|group|administrators|user|or|and|-|;|,|\(|\)|\[|\]|\{|\}|%|@|\*|!|\')", RegexOptions.IgnoreCase);

        //随机数
        private static char[] constant =   
      {   
        '0','1','2','3','4','5','6','7','8','9',  
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };

        /// <summary>
        /// 整形随机数
        /// </summary>
        private static int[] constantInt = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public static string ConvertMilliseconds(double milliseconds)
        {
            if (milliseconds <= 0)
                return "刚刚";
            //秒
            double miao = milliseconds / 1000;
            if (miao < 60)
                return (int)miao +"秒";

            double fenzhong = miao / 60;
            if (fenzhong < 60)
                return (int)fenzhong + "分钟";
            double xiaoshi = fenzhong / 60;
            if (xiaoshi < 60)
                return xiaoshi.ToString("0.0") + "小时";

            double tian = xiaoshi / 24;
            return (int)tian + "天";
        }

        /// <summary>
        /// 把时间改为几个月,几天前,几小时前,几分钟前,或几秒前
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else
            {
                if (span.TotalDays > 30)
                {
                    return
                    "1个月前";
                }
                else
                {
                    if (span.TotalDays > 14)
                    {
                        return
                        "2周前";
                    }
                    else
                    {
                        if (span.TotalDays > 7)
                        {
                            return
                            "1周前";
                        }
                        else
                        {
                            if (span.TotalDays > 1)
                            {
                                return
                                string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
                            }
                            else
                            {
                                if (span.TotalHours > 1)
                                {
                                    return
                                    string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
                                }
                                else
                                {
                                    if (span.TotalMinutes > 1)
                                    {
                                        return
                                        string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
                                    }
                                    else
                                    {
                                        if (span.TotalSeconds >= 1)
                                        {
                                            return
                                            string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
                                        }
                                        else
                                        {
                                            return
                                            "1秒前";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //C#中使用TimeSpan计算两个时间的差值
        //可以反加两个日期之间任何一个时间单位。
        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }


        //说明：
        /**/
        /*1.DateTime值类型代表了一个从公元0001年1月1日0点0分0秒到公元9999年12月31日23点59分59秒之间的具体日期时刻。因此，你可以用DateTime值类型来描述任何在想象范围之内的时间。一个DateTime值代表了一个具体的时刻
        2.TimeSpan值包含了许多属性与方法，用于访问或处理一个TimeSpan值
        下面的列表涵盖了其中的一部分：
        Add：与另一个TimeSpan值相加。 
        Days:返回用天数计算的TimeSpan值。 
        Duration:获取TimeSpan的绝对值。 
        Hours:返回用小时计算的TimeSpan值 
        Milliseconds:返回用毫秒计算的TimeSpan值。 
        Minutes:返回用分钟计算的TimeSpan值。 
        Negate:返回当前实例的相反数。 
        Seconds:返回用秒计算的TimeSpan值。 
        Subtract:从中减去另一个TimeSpan值。 
        Ticks:返回TimeSpan值的tick数。 
        TotalDays:返回TimeSpan值表示的天数。 
        TotalHours:返回TimeSpan值表示的小时数。 
        TotalMilliseconds:返回TimeSpan值表示的毫秒数。 
        TotalMinutes:返回TimeSpan值表示的分钟数。 
        TotalSeconds:返回TimeSpan值表示的秒数。
        */

        /**/
        /// <summary>
        /// 日期比较
        /// </summary>
        /// <param name="today">当前日期</param>
        /// <param name="writeDate">输入日期</param>
        /// <param name="n">比较天数</param>
        /// <returns>大于天数返回true，小于返回false</returns>
        private bool CompareDate(string today, string writeDate, int n)
        {
            DateTime Today = Convert.ToDateTime(today);
            DateTime WriteDate = Convert.ToDateTime(writeDate);
            WriteDate = WriteDate.AddDays(n);
            if (Today >= WriteDate)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 字节转换为GB，不足G的返回MB，保留小数点后1位
        /// </summary>
        /// <returns></returns>
        public static string ByteToGB(long data)
        {
            if (data <= 0)
                return "0";

            string val = "0";
            float mb = data / 1024f / 1024f;
            if (mb < 1024)
                val = mb.ToString("F1")+"MB";
            else
                val = (mb / 1024f).ToString("F1")+"GB";

            return val;
        }


        /// <summary>
        /// 根据URL下载图片
        /// </summary>
        /// <param name="pic_url">图片的网络URL</param>
        /// <param name="file_path">保存的相对路径,结尾要有/符号</param>
        /// <param name="file_name">保存的文件名，包含后缀</param>
        public static void DownPicFromUrl(string pic_url,string file_path,string file_name)
        {
            string io_path = IOHelper.GetMapPath(file_path);
            if (!Directory.Exists(io_path))
            {
                Directory.CreateDirectory(io_path);
            }

            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
                wc.DownloadFile(pic_url, io_path + file_name);
            }
        }

        ///// <summary>
        ///// 根据经纬度获取城市区名字
        ///// </summary>
        ///// <param name="jing"></param>
        ///// <param name="wei"></param>
        ///// <returns></returns>
        //public static Dictionary<string, string> GetLocationNameByLan(string jing,string wei)
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();

        //    string json = GetRequestData("http://lbs.juhe.cn/api/getaddressbylngb?lngx=" + jing + "&lngy=" + wei + "", "GET", "");
        //    JsonData jd = JsonMapper.ToObject(json);
        //    if (jd["resultcode"].ToString() == "1")
        //    {
        //        JsonData jd_row = jd["row"];
        //        if(jd_row["status"].ToString() == "OK")
        //        {
        //            JsonData jd_result = jd_row["result"];
        //            dic.Add("province", jd_result["addressComponent"]["province"].ToString());
        //            dic.Add("city", jd_result["addressComponent"]["city"].ToString());
        //            dic.Add("district", jd_result["addressComponent"]["district"].ToString());
        //        }
        //    }
        //    return dic;

        //}
        
        /// <summary>
        /// 时间戳转为C#格式时间
        /// <param name="defTime">如果转换失败，则返回的默认的时间</param>
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp,DateTime defTime)
        {
            try
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000000");
                TimeSpan toNow = new TimeSpan(lTime); 
                return dtStart.Add(toNow);
            }
            catch
            {
                return defTime;
            }
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name=”time”></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="starTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="action">取值类别,d=天，h=小时，m=分钟，s=秒，ad=总天数，ah=总小时，am=总分钟，as=总秒</param>
        /// <returns></returns>
        public static double DateTimeDiff(DateTime starTime,DateTime endTime,string action)
        {
            if (endTime < starTime)
                return 0;
            TimeSpan ts = endTime - starTime;
            switch (action)
            {
                case "d"://天
                    return ts.Days;
                case "h"://小时
                    return ts.Hours;
                case "m": //分钟
                    return ts.Minutes;
                case "s"://秒
                    return ts.Seconds;
                case "ad"://总天数
                    return ts.TotalDays;
                case "ah"://总小时
                    return ts.TotalHours;
                case "am"://总分钟
                    return double.Parse(ts.TotalMinutes.ToString("F2"));
                case "as": //总秒
                    return ts.TotalSeconds;
                default:
                    return 0;
            }
        }


        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="Length">长度</param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 生成整形随机数
        /// </summary>
        /// <param name="Length">长度</param>
        /// <returns></returns>
        public static string GenerateRandomIntNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constantInt[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        #region 编码

        /// <summary>
        /// HTML解码
        /// </summary>
        /// <returns></returns>
        public static string HtmlDecode(string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// HTML编码
        /// </summary>
        /// <returns></returns>
        public static string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <returns></returns>
        public static string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="Encoding">编码方式：GB2312</param>
        /// <returns></returns>
        public static string UrlEncode(string s,string Encoding)
        {
            return HttpUtility.UrlEncode(s,System.Text.Encoding.GetEncoding(Encoding));
        }

        #endregion

        #region Cookie

        /// <summary>
        /// 删除指定名称的Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        public static void DeleteCookie(string name)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获得指定名称的Cookie值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookie(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
                return cookie.Value;

            return string.Empty;
        }

        /// <summary>
        /// 获得指定名称的Cookie中特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetCookie(string name, string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null && cookie.HasKeys)
            {
                string v = cookie[key];
                if (v != null)
                    return v;
            }

            return string.Empty;
        }

        /// <summary>
        /// 设置指定名称的Cookie的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
                cookie.Value = value;
            else
                cookie = new HttpCookie(name, value);

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置指定名称的Cookie的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string value, double expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);

            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCookie(string name, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);

            cookie[key] = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string name, string key, string value, double expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                cookie = new HttpCookie(name);

            cookie[key] = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        #region 客户端信息

        /// <summary>
        /// 是否是get请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod == "GET";
        }

        /// <summary>
        /// 是否是post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod == "POST";
        }

        /// <summary>
        /// 是否是Ajax请求
        /// </summary>
        /// <returns></returns>
        public static bool IsAjax()
        {
            return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        /// 获得查询字符串中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetQueryString(string key, string defaultValue)
        {
            string value = HttpContext.Current.Request.QueryString[key];
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            else
                return defaultValue;
        }

        /// <summary>
        /// 获得查询字符串中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetQueryString(string key)
        {
            return GetQueryString(key, "");
        }

        /// <summary>
        /// 获得查询字符串中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetQueryInt(string key, int defaultValue)
        {
            return TypeHelper.StringToInt(HttpContext.Current.Request.QueryString[key], defaultValue);
        }

        /// <summary>
        /// 获得查询字符串中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static int GetQueryInt(string key)
        {
            return GetQueryInt(key, 0);
        }

        /// <summary>
        /// 获得表单中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetFormString(string key, string defaultValue)
        {
            string value = HttpContext.Current.Request.Form[key];
            if (!string.IsNullOrWhiteSpace(value))
                return value;
            else
                return defaultValue;
        }

        /// <summary>
        /// 获得表单中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetFormString(string key)
        {
            return GetFormString(key, "");
        }

        /// <summary>
        /// 获得表单中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetFormInt(string key, int defaultValue)
        {
            return TypeHelper.StringToInt(HttpContext.Current.Request.Form[key], defaultValue);
        }

        /// <summary>
        /// 获得表单中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static int GetFormInt(string key)
        {
            return GetFormInt(key, 0);
        }

        /// <summary>
        /// 获得请求中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetRequestString(string key, string defaultValue)
        {
            if (HttpContext.Current.Request.Form[key] != null)
                return GetFormString(key, defaultValue);
            else
                return GetQueryString(key, defaultValue);
        }

        /// <summary>
        /// 获得请求中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetRequestString(string key)
        {
            if (HttpContext.Current.Request.Form[key] != null)
                return GetFormString(key);
            else
                return GetQueryString(key);
        }

        /// <summary>
        /// 获得请求中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetRequestInt(string key, int defaultValue)
        {
            if (HttpContext.Current.Request.Form[key] != null)
                return GetFormInt(key, defaultValue);
            else
                return GetQueryInt(key, defaultValue);
        }

        /// <summary>
        /// 获得请求中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static int GetRequestInt(string key)
        {
            if (HttpContext.Current.Request.Form[key] != null)
                return GetFormInt(key);
            else
                return GetQueryInt(key);
        }


        /// <summary>
        /// 获得上次请求的url
        /// </summary>
        /// <returns></returns>
        public static string GetUrlReferrer()
        {
            Uri uri = HttpContext.Current.Request.UrlReferrer;
            if (uri == null)
                return string.Empty;

            return uri.ToString();
        }

        /// <summary>
        /// 获得请求的主机部分
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        /// 获得请求的url
        /// </summary>
        /// <returns></returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        /// 获得请求的原始url
        /// </summary>
        /// <returns></returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 获得请求的ip
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();

            if (string.IsNullOrEmpty(ip) || !ValidateHelper.IsIP(ip))
                ip = "127.0.0.1";
            return ip;
        }

        /// <summary>
        /// 获得请求的浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserType()
        {
            string type = HttpContext.Current.Request.Browser.Type;
            if (string.IsNullOrEmpty(type) || type == "unknown")
                return "未知";

            return type.ToLower();
        }

        /// <summary>
        /// 获得请求的浏览器名称
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserName()
        {
            string name = HttpContext.Current.Request.Browser.Browser;
            if (string.IsNullOrEmpty(name) || name == "unknown")
                return "未知";

            return name.ToLower();
        }

        /// <summary>
        /// 获得请求的浏览器版本
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserVersion()
        {
            string version = HttpContext.Current.Request.Browser.Version;
            if (string.IsNullOrEmpty(version) || version == "unknown")
                return "未知";

            return version;
        }

        /// <summary>
        /// 获得请求客户端的操作系统类型
        /// </summary>
        /// <returns></returns>
        public static string GetOSType()
        {
            string type = "未知";
            string userAgent = HttpContext.Current.Request.UserAgent;

            if (userAgent.Contains("NT 6.1"))
            {
                type = "Windows 7";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                type = "Windows XP";
            }
            else if (userAgent.Contains("NT 6.2"))
            {
                type = "Windows 8";
            }
            else if (userAgent.Contains("android"))
            {
                type = "Android";
            }
            else if (userAgent.Contains("iPhone"))
            {
                type = "IPhone";
            }
            else if (userAgent.Contains("Mac"))
            {
                type = "Mac";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                type = "Windows Vista";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                type = "Windows 2003";
            }
            else if (userAgent.Contains("NT 5.0"))
            {
                type = "Windows 2000";
            }
            else if (userAgent.Contains("98"))
            {
                type = "Windows 98";
            }
            else if (userAgent.Contains("95"))
            {
                type = "Windows 95";
            }
            else if (userAgent.Contains("Me"))
            {
                type = "Windows Me";
            }
            else if (userAgent.Contains("NT 4"))
            {
                type = "Windows NT4";
            }
            else if (userAgent.Contains("Unix"))
            {
                type = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                type = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                type = "SunOS";
            }

            return type;
        }

        /// <summary>
        /// 获得请求客户端的操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetOSName()
        {
            string name = HttpContext.Current.Request.Browser.Platform;
            if (string.IsNullOrEmpty(name))
                return "未知";

            return name;
        }

        /// <summary>
        /// 判断是否是浏览器请求
        /// </summary>
        /// <returns></returns>
        public static bool IsBrowser()
        {
            string name = GetBrowserName();
            foreach (string item in _browserlist)
            {
                if (name.Contains(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是移动设备请求
        /// </summary>
        /// <returns></returns>
        public static bool IsMobile()
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
                return true;

            bool isTablet = false;
            if (bool.TryParse(HttpContext.Current.Request.Browser["IsTablet"], out isTablet) && isTablet)
                return true;

            return false;
        }

        /// <summary>
        /// 判断是否是搜索引擎爬虫请求
        /// </summary>
        /// <returns></returns>
        public static bool IsCrawler()
        {
            bool result = HttpContext.Current.Request.Browser.Crawler;
            if (!result)
            {
                string referrer = GetUrlReferrer();
                if (referrer.Length > 0)
                {
                    foreach (string item in _searchenginelist)
                    {
                        if (referrer.Contains(item))
                            return true;
                    }
                }
            }
            return result;
        }

        #endregion

        #region Http

        /// <summary>
        /// 获得参数列表
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static NameValueCollection GetParmList(string data)
        {
            NameValueCollection parmList = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(data))
            {
                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    int startIndex = i;
                    int endIndex = -1;
                    while (i < length)
                    {
                        char c = data[i];
                        if (c == '=')
                        {
                            if (endIndex < 0)
                                endIndex = i;
                        }
                        else if (c == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key;
                    string value;
                    if (endIndex >= 0)
                    {
                        key = data.Substring(startIndex, endIndex - startIndex);
                        value = data.Substring(endIndex + 1, (i - endIndex) - 1);
                    }
                    else
                    {
                        key = data.Substring(startIndex, i - startIndex);
                        value = string.Empty;
                    }
                    parmList[key] = value;
                    if ((i == (length - 1)) && (data[i] == '&'))
                        parmList[key] = string.Empty;
                }
            }
            return parmList;
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送数据</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string postData)
        {
            return GetRequestData(url, "post", postData);
        }

        /// <summary>
        /// 判断当前字符串是否存在SQL注入
        /// </summary>
        /// <returns></returns>
        public static bool IsSafeSqlString(string s)
        {
            if (s != null && _sqlkeywordregex.IsMatch(s))
                return false;
            return true;
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="postData">发送数据</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string method, string postData)
        {
            return GetRequestData(url, method, postData, Encoding.UTF8);
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="postData">发送数据</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string method, string postData, Encoding encoding)
        {
            return GetRequestData(url, method, postData, encoding, 20000);
        }


        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }


        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="postData">发送数据</param>
        /// <param name="encoding">编码</param>
        /// <param name="timeout">超时值</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string method, string postData, Encoding encoding, int timeout)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
                url = "http://" + url;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.Trim().ToLower();
            request.Timeout = timeout;
            request.AllowAutoRedirect = true;
            request.ContentType = "text/html";
            request.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

            try
            {
                if (!string.IsNullOrEmpty(postData) && request.Method == "post")
                {
                    byte[] buffer = encoding.GetBytes(postData);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (encoding == null)
                    {
                        MemoryStream stream = new MemoryStream();
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                            new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(stream, 10240);
                        else
                            response.GetResponseStream().CopyTo(stream, 10240);

                        byte[] RawResponse = stream.ToArray();
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        Match meta = _metaregex.Match(temp);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        return encoding.GetString(RawResponse);
                    }
                    else
                    {
                        StreamReader reader = null;
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            using (reader = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                try
                                {
                                    return reader.ReadToEnd();
                                }
                                catch
                                {
                                    return "close";
                                }

                            }
                        }
                    }
                }

            }
            catch
            {
                return "error";
            }
        }

        #endregion

        #region .NET

        /// <summary>
        /// 获得当前应用程序的信任级别
        /// </summary>
        /// <returns></returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            AspNetHostingPermissionLevel trustLevel = AspNetHostingPermissionLevel.None;
            //权限列表
            AspNetHostingPermissionLevel[] levelList = new AspNetHostingPermissionLevel[] {
                                                                                            AspNetHostingPermissionLevel.Unrestricted,
                                                                                            AspNetHostingPermissionLevel.High,
                                                                                            AspNetHostingPermissionLevel.Medium,
                                                                                            AspNetHostingPermissionLevel.Low,
                                                                                            AspNetHostingPermissionLevel.Minimal 
                                                                                            };

            foreach (AspNetHostingPermissionLevel level in levelList)
            {
                try
                {
                    //通过执行Demand方法检测是否抛出SecurityException异常来设置当前应用程序的信任级别
                    new AspNetHostingPermission(level).Demand();
                    trustLevel = level;
                    break;
                }
                catch 
                {
                    continue;
                }
            }
            return trustLevel;
        }

        /// <summary>
        /// 修改web.config文件
        /// </summary>
        /// <returns></returns>
        private static bool TryWriteWebConfig()
        {
            try
            {
                File.SetLastWriteTimeUtc(IOHelper.GetMapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改global.asax文件
        /// </summary>
        /// <returns></returns>
        private static bool TryWriteGlobalAsax()
        {
            try
            {
                File.SetLastWriteTimeUtc(IOHelper.GetMapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 重启应用程序
        /// </summary>
        public static void RestartAppDomain()
        {
            if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)//如果当前信任级别大于Medium，则通过卸载应用程序域的方式重启
            {
                HttpRuntime.UnloadAppDomain();
                TryWriteGlobalAsax();
            }
            else//通过修改web.config方式重启应用程序
            {
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new Exception("修改web.config文件重启应用程序");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new Exception("修改global.asax文件重启应用程序");
                }
            }

        }

        #endregion
    }
}
