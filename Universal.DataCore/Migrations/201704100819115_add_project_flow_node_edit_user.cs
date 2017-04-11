namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project_flow_node_edit_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectFlowNode", "EditUserId", c => c.Int(nullable: false,defaultValue:1));
            CreateIndex("dbo.ProjectFlowNode", "EditUserId");
            AddForeignKey("dbo.ProjectFlowNode", "EditUserId", "dbo.CusUser", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectFlowNode", "EditUserId", "dbo.CusUser");
            DropIndex("dbo.ProjectFlowNode", new[] { "EditUserId" });
            DropColumn("dbo.ProjectFlowNode", "EditUserId");
        }
    }
}
