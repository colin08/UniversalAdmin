namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_favorites : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CusUserFavorites", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.CusUserFavorites", new[] { "CusUserID" });
            CreateTable(
                "dbo.CusUserDocFavorites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        DocPostID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .ForeignKey("dbo.DocPost", t => t.DocPostID, cascadeDelete: false)
                .Index(t => t.CusUserID)
                .Index(t => t.DocPostID);
            
            AlterColumn("dbo.DocPostPower", "TOID", c => c.String(maxLength: 500));
            DropTable("dbo.CusUserFavorites");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CusUserFavorites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        TOID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.CusUserDocFavorites", "DocPostID", "dbo.DocPost");
            DropForeignKey("dbo.CusUserDocFavorites", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.CusUserDocFavorites", new[] { "DocPostID" });
            DropIndex("dbo.CusUserDocFavorites", new[] { "CusUserID" });
            AlterColumn("dbo.DocPostPower", "TOID", c => c.Int(nullable: false));
            DropTable("dbo.CusUserDocFavorites");
            CreateIndex("dbo.CusUserFavorites", "CusUserID");
            AddForeignKey("dbo.CusUserFavorites", "CusUserID", "dbo.CusUser", "ID", cascadeDelete: true);
        }
    }
}
