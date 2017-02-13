namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_node : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Node", "IsFactor", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectFlowNode", "IsSelect", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectFlowNode", "IsSelect");
            DropColumn("dbo.Node", "IsFactor");
        }
    }
}
