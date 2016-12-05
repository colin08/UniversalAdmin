using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Tests
{
    [TestClass()]
    public class BaseBLLTests
    {
        [TestMethod()]
        public void BLLMethod()
        {
            //string msg = "";
            //BLLDocCategory.Sort("1,15,17,16,18", out msg);
            var db = new DataCore.EFDBContext();
            var entity = db.WorkJobs.AsNoTracking().Where(p => p.ID == 1).FirstOrDefault();
            entity.Title = "标题22";
            entity.Content = "内容222";
            BLLWorkJob.Modify(entity, "4");
            db.Dispose();
            Assert.AreEqual(1,1);
        }
    }
}