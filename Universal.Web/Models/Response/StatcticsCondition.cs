using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 统计筛选条件所需参数
    /// </summary>
    public class StatcticsCondition
    {
        public StatcticsCondition()
        {
            this.niandu = new List<SimpleEntity>();
            this.niandu.Add(new SimpleEntity(0, "所有年度"));
            this.jidu = new List<SimpleEntity>();
            this.jidu.Add(new SimpleEntity(0, "所有季度"));
            this.area = new List<SimpleEntity>();
            this.area.Add(new SimpleEntity(0, "所有区域"));
            this.gz = new List<SimpleEntity>();
            this.gz.Add(new SimpleEntity(0, "所有性质"));
        }

        /// <summary>
        /// 年
        /// </summary>
        public List<SimpleEntity> niandu { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public List<SimpleEntity> jidu { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public List<SimpleEntity> area { get; set; }

        /// <summary>
        /// 改造性质
        /// </summary>
        public List<SimpleEntity> gz { get; set; }
        
    }

    public class SimpleEntity
    {
        public SimpleEntity()
        {

        }

        public SimpleEntity(int id,string text)
        {
            this.search_id = id;
            this.show_text = text;
        }

        public int search_id { get; set; }

        public string show_text { get; set; }
    }

}