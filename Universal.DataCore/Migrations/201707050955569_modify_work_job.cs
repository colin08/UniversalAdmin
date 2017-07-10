namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_work_job : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkJobUserFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WorkJobUserID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.WorkJobUser", t => t.WorkJobUserID, cascadeDelete: true)
                .Index(t => t.WorkJobUserID);
            
            AddColumn("dbo.WorkJobUser", "ConfirmText", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkJobUserFile", "WorkJobUserID", "dbo.WorkJobUser");
            DropIndex("dbo.WorkJobUserFile", new[] { "WorkJobUserID" });
            DropColumn("dbo.WorkJobUser", "ConfirmText");
            DropTable("dbo.WorkJobUserFile");
        }
    }
}
