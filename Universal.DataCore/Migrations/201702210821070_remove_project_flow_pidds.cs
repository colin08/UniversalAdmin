namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_project_flow_pidds : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProjectFlowNode", "SortNo");
            DropColumn("dbo.ProjectFlowNode", "PIds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectFlowNode", "PIds", c => c.String(maxLength: 1000));
            AddColumn("dbo.ProjectFlowNode", "SortNo", c => c.Int(nullable: false));
        }
    }
}
