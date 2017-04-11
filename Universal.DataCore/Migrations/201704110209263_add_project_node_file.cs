namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project_node_file : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectFlowNodeFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectFlowNodeID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProjectFlowNode", t => t.ProjectFlowNodeID, cascadeDelete: true)
                .Index(t => t.ProjectFlowNodeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectFlowNodeFile", "ProjectFlowNodeID", "dbo.ProjectFlowNode");
            DropIndex("dbo.ProjectFlowNodeFile", new[] { "ProjectFlowNodeID" });
            DropTable("dbo.ProjectFlowNodeFile");
        }
    }
}
