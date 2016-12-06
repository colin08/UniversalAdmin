namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_message : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CusUserMessage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Content = c.String(maxLength: 500),
                        LinkID = c.String(maxLength: 10),
                        IsRead = c.Boolean(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CusUserMessage", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.CusUserMessage", new[] { "CusUserID" });
            DropTable("dbo.CusUserMessage");
        }
    }
}
