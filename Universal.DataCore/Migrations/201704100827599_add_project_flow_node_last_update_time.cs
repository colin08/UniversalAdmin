namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project_flow_node_last_update_time : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectFlowNode", "LastUpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectFlowNode", "LastUpdateTime");
        }
    }
}
