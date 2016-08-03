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
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void TreeTest()
        {
            var db = new DataCore.EFDBContext();
            List<Universal.Web.Areas.Admin.Models.ViewModelTree> list = new List<Web.Areas.Admin.Models.ViewModelTree>();
            var query = db.SysRoutes.GroupBy(p => p.Tag).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                int pid = TypeHelper.ObjectToInt(WebHelper.GenerateRandomIntNumber(4));
                Universal.Web.Areas.Admin.Models.ViewModelTree model = new Web.Areas.Admin.Models.ViewModelTree();
                model.id = pid;
                model.name = query[i].Key;
                model.open = i < 4 ? true : false;
                model.pId = 0;
                list.Add(model);
                foreach (var item in query[i].ToList())
                {
                    Universal.Web.Areas.Admin.Models.ViewModelTree model2 = new Web.Areas.Admin.Models.ViewModelTree();
                    model2.id = item.ID;
                    model2.name =  item.Desc;
                    model2.open = false;
                    model2.pId = pid;
                    list.Add(model2);
                }
                
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            db.Dispose();
            Assert.AreEqual(1,1);
        }
    }
}
