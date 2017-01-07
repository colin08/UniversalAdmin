namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_node_cate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Node", "NodeCategoryID", "dbo.NodeCategory");
            DropIndex("dbo.Node", new[] { "NodeCategoryID" });
            AlterColumn("dbo.Node", "NodeCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Node", "NodeCategoryID");
            AddForeignKey("dbo.Node", "NodeCategoryID", "dbo.NodeCategory", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Node", "NodeCategoryID", "dbo.NodeCategory");
            DropIndex("dbo.Node", new[] { "NodeCategoryID" });
            AlterColumn("dbo.Node", "NodeCategoryID", c => c.Int());
            CreateIndex("dbo.Node", "NodeCategoryID");
            AddForeignKey("dbo.Node", "NodeCategoryID", "dbo.NodeCategory", "ID");
        }
    }
}
