namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_project_flow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Pieces", c => c.String(maxLength: 100));
            AddColumn("dbo.FlowNode", "Piece", c => c.Int(nullable: false));
            AddColumn("dbo.Flow", "Pieces", c => c.String());
            AddColumn("dbo.ProjectFlowNode", "Piece", c => c.Int(nullable: false));
            DropColumn("dbo.Flow", "JoinFlow");
            DropColumn("dbo.ProjectFlowNode", "JoinFlow");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectFlowNode", "JoinFlow", c => c.String());
            AddColumn("dbo.Flow", "JoinFlow", c => c.String());
            DropColumn("dbo.ProjectFlowNode", "Piece");
            DropColumn("dbo.Flow", "Pieces");
            DropColumn("dbo.FlowNode", "Piece");
            DropColumn("dbo.Project", "Pieces");
        }
    }
}
