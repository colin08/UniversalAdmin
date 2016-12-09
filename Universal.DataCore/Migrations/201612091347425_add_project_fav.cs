namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project_fav : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CusUserProjectFavorites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.CusUserID)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CusUserProjectFavorites", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.CusUserProjectFavorites", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.CusUserProjectFavorites", new[] { "ProjectID" });
            DropIndex("dbo.CusUserProjectFavorites", new[] { "CusUserID" });
            DropTable("dbo.CusUserProjectFavorites");
        }
    }
}
