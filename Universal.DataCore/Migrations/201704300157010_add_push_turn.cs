namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_push_turn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CusUserPushTurn",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        OnStr = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CusUserPushTurn", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.CusUserPushTurn", new[] { "CusUserID" });
            DropTable("dbo.CusUserPushTurn");
        }
    }
}
