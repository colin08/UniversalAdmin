using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 医学通识标签
    /// </summary>
    public class NewsTags
    {
        public NewsTags() { }

        public NewsTags(int id,string title,int weight,int category_id)
        {
            this.ID = id;
            this.Title = title;
            this.Weight = weight;
            this.CategoryID = category_id;
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public int Weight { get; set; }

        public int CategoryID { get; set; }

    }
}
