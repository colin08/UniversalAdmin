namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_tt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkJobFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkJobID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkJob", t => t.WorkJobID, cascadeDelete: true)
                .Index(t => t.WorkJobID);
            
            CreateTable(
                "dbo.WorkMeetingFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkMeetingID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkMeeting", t => t.WorkMeetingID, cascadeDelete: true)
                .Index(t => t.WorkMeetingID);
            
            CreateTable(
                "dbo.WorkPlanItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkPlanID = c.Int(nullable: false),
                        Title = c.String(maxLength: 255),
                        Content = c.String(maxLength: 1000),
                        WantTaget = c.String(maxLength: 255),
                        DoneTime = c.String(maxLength: 100),
                        Status = c.Byte(nullable: false),
                        Remark = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkPlan", t => t.WorkPlanID, cascadeDelete: true)
                .Index(t => t.WorkPlanID);
            
            AddColumn("dbo.Project", "LastUpdateUserName", c => c.String(maxLength: 30));
            AddColumn("dbo.ProjectFlowNode", "Remark", c => c.String(maxLength: 500));
            AddColumn("dbo.WorkMeeting", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkPlan", "ApproveUserID", c => c.Int(nullable: false));
            AddColumn("dbo.WorkPlan", "BeginTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkPlan", "EndTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.WorkPlan", "ApproveUserID");
            AddForeignKey("dbo.WorkPlan", "ApproveUserID", "dbo.CusUser", "ID", cascadeDelete: false);
            DropColumn("dbo.WorkPlan", "NowJob");
            DropColumn("dbo.WorkPlan", "NextPlan");
            DropColumn("dbo.WorkPlan", "DoneTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkPlan", "DoneTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkPlan", "NextPlan", c => c.String(maxLength: 1000));
            AddColumn("dbo.WorkPlan", "NowJob", c => c.String(maxLength: 1000));
            DropForeignKey("dbo.WorkPlanItem", "WorkPlanID", "dbo.WorkPlan");
            DropForeignKey("dbo.WorkPlan", "ApproveUserID", "dbo.CusUser");
            DropForeignKey("dbo.WorkMeetingFile", "WorkMeetingID", "dbo.WorkMeeting");
            DropForeignKey("dbo.WorkJobFile", "WorkJobID", "dbo.WorkJob");
            DropIndex("dbo.WorkPlan", new[] { "ApproveUserID" });
            DropIndex("dbo.WorkPlanItem", new[] { "WorkPlanID" });
            DropIndex("dbo.WorkMeetingFile", new[] { "WorkMeetingID" });
            DropIndex("dbo.WorkJobFile", new[] { "WorkJobID" });
            DropColumn("dbo.WorkPlan", "EndTime");
            DropColumn("dbo.WorkPlan", "BeginTime");
            DropColumn("dbo.WorkPlan", "ApproveUserID");
            DropColumn("dbo.WorkMeeting", "EndTime");
            DropColumn("dbo.ProjectFlowNode", "Remark");
            DropColumn("dbo.Project", "LastUpdateUserName");
            DropTable("dbo.WorkPlanItem");
            DropTable("dbo.WorkMeetingFile");
            DropTable("dbo.WorkJobFile");
        }
    }
}
