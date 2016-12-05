namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_job_meeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkJob",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                        Title = c.String(maxLength: 500),
                        Content = c.String(maxLength: 1000),
                        DoneTime = c.DateTime(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.WorkJobUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkJobID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                        IsConfirm = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .ForeignKey("dbo.WorkJob", t => t.WorkJobID, cascadeDelete: true)
                .Index(t => t.WorkJobID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.WorkMeeting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        Status = c.Byte(nullable: false),
                        Title = c.String(maxLength: 500),
                        Content = c.String(maxLength: 1000),
                        BeginTime = c.DateTime(nullable: false),
                        Location = c.String(maxLength: 100),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.WorkMeetingUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkMeetingID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                        IsConfirm = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .ForeignKey("dbo.WorkMeeting", t => t.WorkMeetingID, cascadeDelete: true)
                .Index(t => t.WorkMeetingID)
                .Index(t => t.CusUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkMeetingUser", "WorkMeetingID", "dbo.WorkMeeting");
            DropForeignKey("dbo.WorkMeetingUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.WorkMeeting", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.WorkJobUser", "WorkJobID", "dbo.WorkJob");
            DropForeignKey("dbo.WorkJobUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.WorkJob", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.WorkMeetingUser", new[] { "CusUserID" });
            DropIndex("dbo.WorkMeetingUser", new[] { "WorkMeetingID" });
            DropIndex("dbo.WorkMeeting", new[] { "CusUserID" });
            DropIndex("dbo.WorkJobUser", new[] { "CusUserID" });
            DropIndex("dbo.WorkJobUser", new[] { "WorkJobID" });
            DropIndex("dbo.WorkJob", new[] { "CusUserID" });
            DropTable("dbo.WorkMeetingUser");
            DropTable("dbo.WorkMeeting");
            DropTable("dbo.WorkJobUser");
            DropTable("dbo.WorkJob");
        }
    }
}
