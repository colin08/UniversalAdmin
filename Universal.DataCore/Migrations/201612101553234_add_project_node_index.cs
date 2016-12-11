namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project_node_index : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectFlowNode", "Index", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectFlowNode", "Index");
        }
    }
}
