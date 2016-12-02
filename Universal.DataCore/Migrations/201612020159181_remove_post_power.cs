namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_post_power : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocPostPower", "DocPostID", "dbo.DocPost");
            DropIndex("dbo.DocPostPower", new[] { "DocPostID" });
            AddColumn("dbo.DocPost", "TOID", c => c.String(maxLength: 1000));
            DropTable("dbo.DocPostPower");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocPostPower",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DocPostID = c.Int(nullable: false),
                        TOID = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.DocPost", "TOID");
            CreateIndex("dbo.DocPostPower", "DocPostID");
            AddForeignKey("dbo.DocPostPower", "DocPostID", "dbo.DocPost", "ID", cascadeDelete: true);
        }
    }
}
