namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_down_log : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DownloadLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        Title = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DownloadLog", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.DownloadLog", new[] { "CusUserID" });
            DropTable("dbo.DownloadLog");
        }
    }
}
