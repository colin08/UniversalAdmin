using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Tools
{
    /// <summary>
    /// 生成短域名
    /// </summary>
    public class GenerateUrl
    {
        private static string Number = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 压缩ID标识
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string Short(long n)
        {
            string result = string.Empty;
            int l = Number.Length;
            while (n / l >= 1)
            {
                result = Number[(int)(n % l)] + result;
                n /= l;
            }
            result = Number[(int)n] + result;
            return result;
        }

        /// <summary>
        /// 还原ID标识
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long UnShort(string s)
        {
            long result = 0;
            if (!string.IsNullOrWhiteSpace(s))
            {
                s = s.Trim();
                int l = s.Length;
                int m = Number.Length;
                for (int x = 0; x < l; x++)
                {
                    result += Number.IndexOf(s[l - 1 - x]) * (long)Math.Pow(m, x);
                }
            }
            return result;
        }

        /// <summary>
        /// 生成短域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string[] ShortUrl(string url)
        {
            //可以自定义生成MD5加密字符传前的混合KEY
            string key = "LitDev";
            //要使用生成URL的字符
            string[] chars = new string[]{
                              "a","b","c","d","e","f","g","h",
                              "i","j","k","l","m","n","o","p",
                              "q","r","s","t","u","v","w","x",
                              "y","z","0","1","2","3","4","5",
                              "6","7","8","9","A","B","C","D",
                              "E","F","G","H","I","J","K","L",
                              "M","N","O","P","Q","R","S","T",
                              "U","V","W","X","Y","Z"
                            };

            //对传入网址进行MD5加密
            string hex = GetMD5Hash(key + url);
            string[] resUrl = new string[4];
            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加
                    outChars += chars[index];
                    //每次循环按位右移5位
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组
                resUrl[i] = outChars;
            }
            return resUrl;

        }



        private static string GetMD5Hash(string str)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
