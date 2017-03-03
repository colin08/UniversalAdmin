namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project_flow_frist_one : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectFlowNode", "IsFrist", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectFlowNode", "IsFrist");
        }
    }
}
