namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_flow_node_frist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlowNode", "is_frist", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FlowNode", "is_frist");
        }
    }
}
