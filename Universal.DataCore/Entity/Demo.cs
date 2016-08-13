using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 演示实体，主要是测试相册功能
    /// </summary>
    public class Demo :BaseAdminEntity
    {
        public int ID { get; set; }

        [MaxLength(50,ErrorMessage ="标题最大长度50"),Display(Name = "标题")]
        [Required(ErrorMessage ="标题不能为空")]
        public string Title { get; set; }

        [MaxLength(15,ErrorMessage ="最大长度15"),Display(Name ="手机号")]
        [RegularExpression(@"^(13|15|18|17)[0-9]{9}$",ErrorMessage ="非法手机号")]
        public string Telphone { get; set; }

        [Display(Name ="价格"),Required(ErrorMessage ="请输入价格")]
        public decimal Price { get; set; }

        [Display(Name ="范围值"),Required(ErrorMessage ="范围值不能为空")]
        [Range(10,100,ErrorMessage ="范围值再10~100之间")]
        public int Ran { get; set; }
        
        [Display(Name ="数据大小"),Required(ErrorMessage ="请输入数据大小")]
        public float Num { get; set; }
        
        [Display(Name = "相册字符串")]
        [NotMapped]
        public string StrAlbums
        {
            get
            {
                if (this.Albums != null)
                {
                    System.Text.StringBuilder str_albums = new System.Text.StringBuilder();
                    foreach (var item in this.Albums)
                    {
                        str_albums.Append(item.ImgUrl + ",");
                    }
                    if(str_albums.Length > 0)
                    {
                        str_albums.Remove(str_albums.Length - 1,1);
                    }
                    return str_albums.ToString();
                }
                else
                    return "";
            }
            set
            {
                Albums = new List<DemoAlbum>();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    int weight = 0;
                    foreach (var item in value.Split(','))
                    {
                        weight++;
                        Albums.Add(new DemoAlbum() { DemoID = this.ID, ImgUrl = item, Weight = weight });
                    }
                }
            }
        }

        public virtual ICollection<DemoAlbum> Albums { get; set; }

        public virtual List<DemoDept> Depts { get; set; }

        public Demo()
        {
            Albums = new List<DemoAlbum>();
            Depts = new List<DemoDept>();
        }

    }
}
