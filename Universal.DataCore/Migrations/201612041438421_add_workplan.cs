namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_workplan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkPlan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        WeekText = c.String(maxLength: 200),
                        NowJob = c.String(maxLength: 1000),
                        NextPlan = c.String(maxLength: 1000),
                        IsApprove = c.Boolean(nullable: false),
                        ApproveTime = c.DateTime(),
                        DoneTime = c.DateTime(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkPlan", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.WorkPlan", new[] { "CusUserID" });
            DropTable("dbo.WorkPlan");
        }
    }
}
