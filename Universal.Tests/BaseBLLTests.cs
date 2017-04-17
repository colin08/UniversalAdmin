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
            //var entity = db.WorkJobs.AsNoTracking().Where(p => p.ID == 1).FirstOrDefault();
            //entity.Title = "标题22";
            //entity.Content = "内容222";
            //BLLWorkJob.Modify(entity, "4");

            //添加项目
            string msg = "";
            BLL.BaseBLL<Entity.Project> bll = new BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == 4);
            List<Entity.ProjectFile> file_list = new List<Entity.ProjectFile>();
            for (int i = 0; i < 4; i++)
            {
                Entity.ProjectFile model = new Entity.ProjectFile();
                model.FileName = "害怕2.jpg";
                model.FilePath = "/uploads/avatar.jpg";
                model.FileSize = "22KB";
                if (i == 0)
                    model.Type = Entity.ProjectFileType.album;
                else
                    model.Type = Entity.ProjectFileType.file;
                model.Project = null;
                file_list.Add(model);
            }
            entity.ProjectFiles = file_list;
            entity.ApproveUserID = 1;
            entity.Area = Entity.ProjectArea.BaoAn;
            //entity.CusUserID = 1;
            //entity.FlowID = 12;
            entity.See = Entity.DocPostSee.everyone;
            entity.Title = "aaaadfd标题";
            entity.LiXiangTime = DateTime.Now.AddDays(10);
            entity.SetYear();
            entity.SetQuarter();
            //BLL.BLLProject.Modify(entity, "5", out msg);

            //BLL.BLLFlow.AddFlowNode(1, 12, 2, 137, 486, "", "#0e76a8", "", out msg);

            db.Dispose();
            Assert.AreEqual(1,1);
        }
    }
}