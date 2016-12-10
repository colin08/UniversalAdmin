namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_flow1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flow", "FlowType", c => c.Int(nullable: false));
            AddColumn("dbo.Flow", "JoinFlow", c => c.String());
            AddColumn("dbo.ProjectFlowNode", "JoinFlow", c => c.String());
            AddColumn("dbo.ProjectFlowNode", "BeginTime", c => c.DateTime());
            AddColumn("dbo.ProjectFlowNode", "EndTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectFlowNode", "EndTime");
            DropColumn("dbo.ProjectFlowNode", "BeginTime");
            DropColumn("dbo.ProjectFlowNode", "JoinFlow");
            DropColumn("dbo.Flow", "JoinFlow");
            DropColumn("dbo.Flow", "FlowType");
        }
    }
}
