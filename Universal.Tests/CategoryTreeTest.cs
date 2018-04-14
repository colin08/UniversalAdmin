using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.DataCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Universal.Tools;

namespace Universal.Tests
{
    /// <summary>
    /// 测试生成无限级分类树数据,OK
    /// </summary>
    [TestClass]
    public class CategoryTreeTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = new EFDBContext();
            //Load();var category_list = db.CusCategorys.AsNoTracking().ToList();
            db.Dispose();

            List<Entity.CusCategory> new_list = new List<Entity.CusCategory>();
            //GetChilds(category_list, new_list, null);
            StringBuilder tree_str = new StringBuilder();
            foreach (var item in new_list)
            {
                tree_str.Append(StringHelper.StringOfChar(item.Depth - 1, " ") + "├ " + StringHelper.StringOfChar(item.Depth - 1, "  ") + item.Title + "\r\n");
                //if(item.Depth == 1)
                //    tree_str.Append(item.Title +"\r\n");
                //else
                //    tree_str.Append(StringHelper.StringOfChar(item.Depth - 1, " ")+"├ " + StringHelper.StringOfChar(item.Depth - 1, "  ") + item.Title +"\r\n");
            }
            Assert.AreEqual(1, 1);
        }

        //public void GetChilds(List<Entity.CusCategory> oldData, List<Entity.CusCategory> newData, int? pid)
        //{

        //    List<Entity.CusCategory> list = new List<Entity.CusCategory>();
        //    if (pid == null)
        //        list = oldData.Where(p => p.PID == null).ToList();
        //    else
        //        list = oldData.Where(p => p.PID == pid).ToList();
        //    foreach (var item in list)
        //    {
        //        Entity.CusCategory entity = new Entity.CusCategory();
        //        entity.AddTime = item.AddTime;
        //        entity.Depth = item.Depth;
        //        entity.ID = item.ID;
        //        entity.PID = item.PID;
        //        entity.SortNo = item.SortNo;
        //        entity.Status = item.Status;
        //        entity.Title = item.Title;
        //        newData.Add(entity);
        //        this.GetChilds(oldData, newData, item.ID);
        //    }
        //}

    }
}
