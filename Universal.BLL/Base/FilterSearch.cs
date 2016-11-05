using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{

    /// <summary>
    /// 过滤约束
    /// </summary>
    public enum FilterSearchContract
    {
        等于,
        不等于,
        大于,
        大于等于,
        小于,
        小于等于,
        like,
        notlike
    }

    public class FilterSearch
    {
        /// <summary>
        /// 过滤的关键字
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 过滤的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 过滤的约束
        /// </summary>
        public FilterSearchContract Contract { get; set; }

        //public FilterSearch()
        //{

        //}

        public FilterSearch()
        {

        }

        public FilterSearch(string key,string value, FilterSearchContract contract)
        {
            this.Key = key;
            this.Value = value;
            this.Contract = contract;
        }

    }
}
