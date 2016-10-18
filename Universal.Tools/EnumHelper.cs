using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Universal.Tools
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        private static Dictionary<string, Dictionary<int, string>> _EnumList = new Dictionary<string, Dictionary<int, string>>(); //枚举缓存池
        private static Dictionary<string, Dictionary<byte, string>> _BEnumList = new Dictionary<string, Dictionary<byte, string>>(); //枚举缓存池
        private static Dictionary<string, Dictionary<long, string>> _LEnumList = new Dictionary<string, Dictionary<long, string>>(); //枚举缓存池
        
        /// <summary>
        /// 将枚举转换成Dictionary&lt;int, string&gt;
        /// Dictionary中，key为枚举项对应的int值；value为：若定义了EnumShowName属性，则取它，否则取name
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToDictionary(Type enumType)
        {
            string keyName = enumType.FullName;

            if (!_EnumList.ContainsKey(keyName))
            {
                Dictionary<int, string> list = new Dictionary<int, string>();

                foreach (int i in Enum.GetValues(enumType))
                {
                    string name = Enum.GetName(enumType, i);

                    //取显示名称
                    string showName = string.Empty;
                    object[] atts = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (atts.Length > 0) showName = ((DescriptionAttribute)atts[0]).Description;

                    list.Add(i, string.IsNullOrEmpty(showName) ? name : showName);
                }

                object syncObj = new object();

                if (!_EnumList.ContainsKey(keyName))
                {
                    lock (syncObj)
                    {
                        if (!_EnumList.ContainsKey(keyName))
                        {
                            _EnumList.Add(keyName, list);
                        }
                    }
                }
            }

            return _EnumList[keyName];
        }

        public static Dictionary<byte, string> BEnumToDictionary(Type enumType)
        {
            string keyName = enumType.FullName;

            if (!_BEnumList.ContainsKey(keyName))
            {
                Dictionary<byte, string> list = new Dictionary<byte, string>();

                foreach (byte i in Enum.GetValues(enumType))
                {
                    string name = Enum.GetName(enumType, i);

                    //取显示名称
                    string showName = string.Empty;
                    object[] atts = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (atts.Length > 0) showName = ((DescriptionAttribute)atts[0]).Description;

                    list.Add(i, string.IsNullOrEmpty(showName) ? name : showName);
                }

                object syncObj = new object();

                if (!_BEnumList.ContainsKey(keyName))
                {
                    lock (syncObj)
                    {
                        if (!_LEnumList.ContainsKey(keyName))
                        {
                            _BEnumList.Add(keyName, list);
                        }
                    }
                }
            }

            return _BEnumList[keyName];
        }

        public static Dictionary<long, string> LEnumToDictionary(Type enumType)
        {
            string keyName = enumType.FullName;

            if (!_LEnumList.ContainsKey(keyName))
            {
                Dictionary<long, string> list = new Dictionary<long, string>();

                foreach (long i in Enum.GetValues(enumType))
                {
                    string name = Enum.GetName(enumType, i);

                    //取显示名称
                    string showName = string.Empty;
                    object[] atts = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (atts.Length > 0) showName = ((DescriptionAttribute)atts[0]).Description;

                    list.Add(i, string.IsNullOrEmpty(showName) ? name : showName);
                }

                object syncObj = new object();

                if (!_LEnumList.ContainsKey(keyName))
                {
                    lock (syncObj)
                    {
                        if (!_LEnumList.ContainsKey(keyName))
                        {
                            _LEnumList.Add(keyName, list);
                        }
                    }
                }
            }

            return _LEnumList[keyName];
        }

        /// <summary>
        /// 获取枚举的备注属性数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<T>(this T value)
        {
            var memInfo = value.GetType().GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>();
            if (attributes.Any())
                return attributes.First().Description;
            return "";
        }
        /// <summary>
        /// 获取枚举值对应的显示名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="intValue">枚举项对应的int值</param>
        /// <returns></returns>
        public static string GetEnumShowName(Type enumType, int intValue)
        {
            return EnumToDictionary(enumType)[intValue];

        }

        public static string GetBEnumShowName(Type enumType, byte intValue)
        {
            return BEnumToDictionary(enumType)[intValue];
        }

        public static string GetLEnumShowName(Type enumType, long intValue)
        {
            return LEnumToDictionary(enumType)[intValue];
        }
    }
}
