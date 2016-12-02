namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_notice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CusNoticeUser", "CusNoticeID", "dbo.CusNotice");
            DropForeignKey("dbo.CusNoticeUser", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.CusNoticeUser", new[] { "CusNoticeID" });
            DropIndex("dbo.CusNoticeUser", new[] { "CusUserID" });
            AddColumn("dbo.CusNotice", "See", c => c.Int(nullable: false));
            AddColumn("dbo.CusNotice", "TOID", c => c.String(maxLength: 1000));
            DropTable("dbo.CusNoticeUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CusNoticeUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusNoticeID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.CusNotice", "TOID");
            DropColumn("dbo.CusNotice", "See");
            CreateIndex("dbo.CusNoticeUser", "CusUserID");
            CreateIndex("dbo.CusNoticeUser", "CusNoticeID");
            AddForeignKey("dbo.CusNoticeUser", "CusUserID", "dbo.CusUser", "ID", cascadeDelete: true);
            AddForeignKey("dbo.CusNoticeUser", "CusNoticeID", "dbo.CusNotice", "ID", cascadeDelete: true);
        }
    }
}
