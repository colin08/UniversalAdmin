namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_node_category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NodeCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        Remark = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Node", "NodeCategoryID", c => c.Int());
            CreateIndex("dbo.Node", "NodeCategoryID");
            AddForeignKey("dbo.Node", "NodeCategoryID", "dbo.NodeCategory", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Node", "NodeCategoryID", "dbo.NodeCategory");
            DropIndex("dbo.Node", new[] { "NodeCategoryID" });
            DropColumn("dbo.Node", "NodeCategoryID");
            DropTable("dbo.NodeCategory");
        }
    }
}
