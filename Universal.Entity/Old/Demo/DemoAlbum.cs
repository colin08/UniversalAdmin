//using System;
//using System.ComponentModel.DataAnnotations;

//namespace Universal.Entity
//{
//    public class DemoAlbum
//    {
//        public int ID { get; set; }

//        /// <summary>
//        /// 外键
//        /// </summary>
//        public int DemoID { get; set; }

//        /// <summary>
//        /// 关联的信息
//        /// </summary>
//        public virtual Demo Demo { get; set; }
        
//        [Required(ErrorMessage ="不能为空")]
//        public string ImgUrl { get; set; }
        
//        /// <summary>
//        /// 升序排列
//        /// </summary>
//        [Display(Name ="权重"),Required]
//        public int Weight { get; set; }

//    }
//}
