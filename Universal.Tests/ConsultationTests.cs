using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.Tests
{
    /// <summary>
    /// 在线咨询添加测试数据
    /// </summary>
    [TestClass]
    public class ConsultationTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string msg = "";
            //添加病例类型
            //BLL.BaseBLL<Entity.ConsultationDisease> bll_cd = new BaseBLL<Entity.ConsultationDisease>();
            //for (int i = 1; i <= 5; i++)
            //{
            //    var entity = new Entity.ConsultationDisease();
            //    entity.Title = "病症类型-" + i.ToString();
            //    entity.Weight = 99 + i;
            //    bll_cd.Add(entity);
            //}


            //添加主贴
            List<Entity.ConsultationFile> main_files = new List<Entity.ConsultationFile>();
            main_files.Add(new Entity.ConsultationFile(Entity.ConsultationFileType.Image, "http://58.250.163.63:9000/demo/img/demo/tel-1.jpg"));
            main_files.Add(new Entity.ConsultationFile(Entity.ConsultationFileType.Image, "http://58.250.163.63:9000/demo/img/demo/tel-2.jpg"));
            main_files.Add(new Entity.ConsultationFile(Entity.ConsultationFileType.Image, "http://58.250.163.63:9000/demo/img/demo/tel-3.jpg"));
            main_files.Add(new Entity.ConsultationFile(Entity.ConsultationFileType.Voice, "http://58.250.163.63:9000/uploads/test.mp3"));

            List<Entity.ConsultationListFile> reply_files = new List<Entity.ConsultationListFile>();
            reply_files.Add(new Entity.ConsultationListFile(Entity.ConsultationFileType.Image, "http://58.250.163.63:9000/demo/img/demo/tel-1.jpg"));
            reply_files.Add(new Entity.ConsultationListFile(Entity.ConsultationFileType.Image, "http://58.250.163.63:9000/demo/img/demo/tel-2.jpg"));
            reply_files.Add(new Entity.ConsultationListFile(Entity.ConsultationFileType.Image, "http://58.250.163.63:9000/demo/img/demo/tel-3.jpg"));
            reply_files.Add(new Entity.ConsultationListFile(Entity.ConsultationFileType.Voice, "http://58.250.163.63:9000/uploads/test.mp3"));
            //int user_id = 1;//用户ID
            //int doc_id = 4;//医生ID
            //string content = "这里是用户询问前的病情描述这里是用户询问前的病情描述这里是用户询问前的病情描述这里是用户询问前的病情描述这里是用户询问前的病情描述这里是用户询问前的病情描述";
            //for (int i = 0; i < 25; i++)
            //{
            //    BLL.BLLConsultation.Add(user_id, doc_id, 1, "深圳", content, main_files, out msg);
            //}

            ////获取医生端数据
            //int total = 0;
            //var ss = BLL.BLLConsultation.GetDoctorsMsgList(10, 1, 4, 1, out total);
            //string reply_content = "这里是回复的内容这里是回复的内容这里是回复的内容这里是回复的内容这里是回复的内容这里是回复的内容这里是回复的内容这里是回复的内容这里是回复的内容";
            //for (int i = 0; i < 15; i++)
            //{
            //    var type = Entity.ReplayUserType.Doctor;
            //    if (i % 2 == 0) type = Entity.ReplayUserType.Doctor;
            //    BLLConsultation.AddReplay(1, reply_content, type, reply_files, out msg);
            //}

            //var ss = BLLConsultation.GetConsultationInfo(1);

            Assert.AreEqual(1, 1);
        }
    }
}
