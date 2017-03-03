namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newflow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlowNode", "SortNo", c => c.Int(nullable: false));
            AddColumn("dbo.FlowNode", "PIds", c => c.String(maxLength: 1000));
            AddColumn("dbo.ProjectFlowNode", "SortNo", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectFlowNode", "PIds", c => c.String(maxLength: 1000));
            DropColumn("dbo.ProjectFlowNode", "Index");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectFlowNode", "Index", c => c.Int(nullable: false));
            DropColumn("dbo.ProjectFlowNode", "PIds");
            DropColumn("dbo.ProjectFlowNode", "SortNo");
            DropColumn("dbo.FlowNode", "PIds");
            DropColumn("dbo.FlowNode", "SortNo");
        }
    }
}
