namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_project_flow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectFlow", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectFlowNode", "ProjectFlowID", "dbo.ProjectFlow");
            DropIndex("dbo.ProjectFlowNode", new[] { "ProjectFlowID" });
            DropIndex("dbo.ProjectFlow", new[] { "ProjectID" });
            AddColumn("dbo.ProjectFlowNode", "ProjectID", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectFlowNode", "ProjectID");
            AddForeignKey("dbo.ProjectFlowNode", "ProjectID", "dbo.Project", "ID", cascadeDelete: true);
            DropColumn("dbo.ProjectFlowNode", "ProjectFlowID");
            DropTable("dbo.ProjectFlow");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectFlow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ProjectFlowNode", "ProjectFlowID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProjectFlowNode", "ProjectID", "dbo.Project");
            DropIndex("dbo.ProjectFlowNode", new[] { "ProjectID" });
            DropColumn("dbo.ProjectFlowNode", "ProjectID");
            CreateIndex("dbo.ProjectFlow", "ProjectID");
            CreateIndex("dbo.ProjectFlowNode", "ProjectFlowID");
            AddForeignKey("dbo.ProjectFlowNode", "ProjectFlowID", "dbo.ProjectFlow", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectFlow", "ProjectID", "dbo.Project", "ID", cascadeDelete: true);
        }
    }
}
