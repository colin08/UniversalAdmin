using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Novacode;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Universal.Tests
{
    [TestClass]
    public class ImportDocxTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            int project_id = 116;

            var project = BLL.BLLProject.GetModel(project_id);
            string flow_name = "";
            if(project.FlowID != null)
            {
                int flow_id = Tools.TypeHelper.ObjectToInt(project.FlowID);
                var entity_flow = new BLL.BaseBLL<Entity.Flow>().GetModel(p => p.ID == flow_id);
                if (entity_flow != null)
                    flow_name = entity_flow.Title;
            }
            System.Text.StringBuilder project_users = new System.Text.StringBuilder();
            foreach (var item in project.ProjectUsers)
            {
                if (item.CusUser != null)
                    project_users.Append(item.CusUser.NickName+",");
            }
            if (project_users.Length > 0)
                project_users.Remove(project_users.Length - 1,1);

            var template = @"C:\test\template.docx";
            var file_path = @"C:\test\new.docx";

            DocX doc;
            doc = DocX.Load(template);
            if (doc.Tables != null && doc.Tables.Count > 0)
            {
                Table table_basic = doc.Tables[0];
                table_basic.Rows[1].Cells[1].Paragraphs[0].Append(project.Title);
                table_basic.Rows[1].Cells[3].Paragraphs[0].Append(project.ApproveUser.NickName);
                table_basic.Rows[2].Cells[1].Paragraphs[0].Append(flow_name);
                table_basic.Rows[3].Cells[1].Paragraphs[0].Append(project_users.ToString());

                Table table_project = doc.Tables[1];
                table_project.Rows[2].Cells[1].Paragraphs[0].Append(Tools.EnumHelper.GetEnumShowName(typeof(Entity.ProjectArea), (int)project.Area));
                table_project.Rows[2].Cells[3].Paragraphs[0].Append(project.GaiZaoXingZhi);
                table_project.Rows[3].Cells[1].Paragraphs[0].Append(project.ZhongDiHao);
                table_project.Rows[3].Cells[3].Paragraphs[0].Append(project.ShenBaoZhuTi);
                table_project.Rows[4].Cells[1].Paragraphs[0].Append(project.GengXinDanYuanYongDiMianJi.ToString());
                table_project.Rows[4].Cells[3].Paragraphs[0].Append(project.ZongMianJi + "㎡ " + project.ZongMianJiOther);
                table_project.Rows[5].Cells[1].Paragraphs[0].Append(project.WuLeiQuanMianJi + "㎡");
                table_project.Rows[5].Cells[3].Paragraphs[0].Append(project.LaoWuCunMianJi + "㎡");
                table_project.Rows[6].Cells[1].Paragraphs[0].Append(project.FeiNongMianJi + "㎡");
                table_project.Rows[6].Cells[3].Paragraphs[0].Append(project.KaiFaMianJi + "㎡");
                table_project.Rows[7].Cells[1].Paragraphs[0].Append(project.RongJiLv.ToString());
                table_project.Rows[7].Cells[3].Paragraphs[0].Append(project.TuDiShiYongQuan);
                table_project.Rows[8].Cells[1].Paragraphs[0].Append(project.JianSheGuiHuaZheng);
                table_project.Rows[8].Cells[3].Paragraphs[0].Append(project.ChaiQianYongDiMianJi + " ㎡");
                table_project.Rows[9].Cells[1].Paragraphs[0].Append(project.ChaiQianJianZhuMianJi + " ㎡");
                string lixiang_time = "";
                if (project.LiXiangTime != null)
                    lixiang_time = Tools.TypeHelper.ObjectToDateTime(project.LiXiangTime).ToShortDateString();
                table_project.Rows[11].Cells[1].Paragraphs[0].Append(lixiang_time);
                if (project.ZhuanXiangTime != null)
                    lixiang_time = Tools.TypeHelper.ObjectToDateTime(project.ZhuanXiangTime).ToShortDateString();
                table_project.Rows[11].Cells[3].Paragraphs[0].Append(lixiang_time);
                if (project.ZhuTiTime != null)
                    lixiang_time = Tools.TypeHelper.ObjectToDateTime(project.ZhuTiTime).ToShortDateString();
                table_project.Rows[12].Cells[1].Paragraphs[0].Append(lixiang_time);
                if (project.YongDiTime != null)
                    lixiang_time = Tools.TypeHelper.ObjectToDateTime(project.YongDiTime).ToShortDateString();
                table_project.Rows[12].Cells[3].Paragraphs[0].Append(lixiang_time);
                if (project.KaiPanTime != null)
                    lixiang_time = Tools.TypeHelper.ObjectToDateTime(project.KaiPanTime).ToShortDateString();
                table_project.Rows[13].Cells[1].Paragraphs[0].Append(lixiang_time);
                table_project.Rows[15].Cells[1].Paragraphs[0].Append(project.FenChengBiLi);
                table_project.Rows[15].Cells[3].Paragraphs[0].Append(project.JunJia.ToString() + " (单位：万元)");

                //拆迁模板
                var db_stage_list = BLL.BLLProjectStage.GetAllStage(project_id);
                Table table_stage_basic = doc.Tables[2];

                int stage_total = db_stage_list.Count;
                if (stage_total == 0)
                    doc.Tables[2].Remove();
                
                //先循环添加基础表格
                for (int i = 0; i < stage_total -1; i++)
                {
                    table_stage_basic.SetBorder(TableBorderType.Bottom, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black));
                    table_stage_basic.SetBorder(TableBorderType.InsideH, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black));
                    table_stage_basic.SetBorder(TableBorderType.InsideV, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black));
                    table_stage_basic.SetBorder(TableBorderType.Left, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black));
                    table_stage_basic.SetBorder(TableBorderType.Right, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black));
                    table_stage_basic.SetBorder(TableBorderType.Top, new Border(BorderStyle.Tcbs_single, BorderSize.one, 0, System.Drawing.Color.Black));
                    doc.InsertTable(table_stage_basic);
                }

                for (int i = 0; i < stage_total; i++)
                {
                    Table table_stage_template = doc.Tables[2 + i];
                    table_stage_template.Rows[0].Cells[0].Paragraphs[0].Append(db_stage_list[i].title);
                    table_stage_template.Rows[2].Cells[1].Paragraphs[0].Append(db_stage_list[i].begin_time);
                    table_stage_template.Rows[2].Cells[3].Paragraphs[0].Append(db_stage_list[i].ChaiZhanDiMianJi + " ㎡");
                    table_stage_template.Rows[3].Cells[1].Paragraphs[0].Append(db_stage_list[i].ZongHuShu);
                    table_stage_template.Rows[3].Cells[3].Paragraphs[0].Append(db_stage_list[i].ChaiJianZhuMianJi + " ㎡");
                    table_stage_template.Rows[4].Cells[1].Paragraphs[0].Append(db_stage_list[i].YiQYHuShu);
                    table_stage_template.Rows[4].Cells[3].Paragraphs[0].Append(db_stage_list[i].ChaiBuChangMianJi + " ㎡");
                    table_stage_template.Rows[5].Cells[1].Paragraphs[0].Append(db_stage_list[i].WeiQYHuShu);
                    table_stage_template.Rows[5].Cells[3].Paragraphs[0].Append(db_stage_list[i].ChaiBuChangjinE);
                    table_stage_template.Rows[6].Cells[1].Paragraphs[0].Append(db_stage_list[i].ZhanDiMianJi + " ㎡");
                    table_stage_template.Rows[7].Cells[1].Paragraphs[0].Append(db_stage_list[i].JiDiMianJi + " ㎡");
                    table_stage_template.Rows[8].Cells[1].Paragraphs[0].Append(db_stage_list[i].KongDiMianJi + " ㎡");
                    table_stage_template.Rows[9].Cells[1].Paragraphs[0].Append(db_stage_list[i].YiQYMianJi + " ㎡");
                    table_stage_template.Rows[10].Cells[1].Paragraphs[0].Append(db_stage_list[i].WeiQYMianJi+ " ㎡");
                }
            }
            doc.SaveAs(file_path);
        }
    }
}
