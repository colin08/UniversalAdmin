namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_flow : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Flow", "NodeID", "dbo.Node");
            DropForeignKey("dbo.Flow", "PID", "dbo.Flow");
            DropForeignKey("dbo.ProjectNode", "PID", "dbo.ProjectNode");
            DropForeignKey("dbo.ProjectNode", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectNodeFile", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.ProjectNodeFile", "ProjectNodeID", "dbo.ProjectNode");
            DropForeignKey("dbo.ProjectNodeUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.ProjectNodeUser", "ProjectNodeID", "dbo.ProjectNode");
            DropIndex("dbo.Flow", new[] { "NodeID" });
            DropIndex("dbo.Flow", new[] { "PID" });
            DropIndex("dbo.ProjectNode", new[] { "ProjectID" });
            DropIndex("dbo.ProjectNode", new[] { "PID" });
            DropIndex("dbo.ProjectNodeFile", new[] { "ProjectNodeID" });
            DropIndex("dbo.ProjectNodeFile", new[] { "CusUserID" });
            DropIndex("dbo.ProjectNodeUser", new[] { "ProjectNodeID" });
            DropIndex("dbo.ProjectNodeUser", new[] { "CusUserID" });
            CreateTable(
                "dbo.FlowNode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FlowID = c.Int(nullable: false),
                        NodeID = c.Int(nullable: false),
                        Top = c.Int(nullable: false),
                        Left = c.Int(nullable: false),
                        ProcessTo = c.String(),
                        Color = c.String(),
                        ICON = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Flow", t => t.FlowID, cascadeDelete: true)
                .ForeignKey("dbo.Node", t => t.NodeID, cascadeDelete: true)
                .Index(t => t.FlowID)
                .Index(t => t.NodeID);
            
            CreateTable(
                "dbo.ProjectFlowNode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectFlowID = c.Int(nullable: false),
                        NodeID = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        IsStart = c.Boolean(nullable: false),
                        IsEnd = c.Boolean(nullable: false),
                        Top = c.Int(nullable: false),
                        Left = c.Int(nullable: false),
                        ProcessTo = c.String(),
                        Color = c.String(),
                        ICON = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Node", t => t.NodeID, cascadeDelete: true)
                .ForeignKey("dbo.ProjectFlow", t => t.ProjectFlowID, cascadeDelete: true)
                .Index(t => t.ProjectFlowID)
                .Index(t => t.NodeID);
            
            CreateTable(
                "dbo.ProjectFlow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            AddColumn("dbo.Flow", "CusUserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Flow", "Title", c => c.String(maxLength: 50));
            CreateIndex("dbo.Flow", "CusUserID");
            AddForeignKey("dbo.Flow", "CusUserID", "dbo.CusUser", "ID", cascadeDelete: true);
            DropColumn("dbo.Flow", "NodeID");
            DropColumn("dbo.Flow", "FlowName");
            DropColumn("dbo.Flow", "PID");
            DropColumn("dbo.Flow", "TopPID");
            DropColumn("dbo.Flow", "TOID");
            DropColumn("dbo.Flow", "Depth");
            DropColumn("dbo.Flow", "Priority");
            DropTable("dbo.ProjectNode");
            DropTable("dbo.ProjectNodeFile");
            DropTable("dbo.ProjectNodeUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProjectNodeUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectNodeID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectNodeFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectNodeID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                        CusUserID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectNode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        PID = c.Int(),
                        TopPID = c.Int(),
                        Title = c.String(maxLength: 255),
                        Location = c.String(maxLength: 500),
                        Content = c.String(),
                        TOID = c.Int(nullable: false),
                        SourceFlowID = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        IsStart = c.Boolean(nullable: false),
                        IsEnd = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Flow", "Priority", c => c.Int(nullable: false));
            AddColumn("dbo.Flow", "Depth", c => c.Int(nullable: false));
            AddColumn("dbo.Flow", "TOID", c => c.Int(nullable: false));
            AddColumn("dbo.Flow", "TopPID", c => c.Int());
            AddColumn("dbo.Flow", "PID", c => c.Int());
            AddColumn("dbo.Flow", "FlowName", c => c.String(maxLength: 50));
            AddColumn("dbo.Flow", "NodeID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProjectFlowNode", "ProjectFlowID", "dbo.ProjectFlow");
            DropForeignKey("dbo.ProjectFlow", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectFlowNode", "NodeID", "dbo.Node");
            DropForeignKey("dbo.FlowNode", "NodeID", "dbo.Node");
            DropForeignKey("dbo.FlowNode", "FlowID", "dbo.Flow");
            DropForeignKey("dbo.Flow", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.ProjectFlow", new[] { "ProjectID" });
            DropIndex("dbo.ProjectFlowNode", new[] { "NodeID" });
            DropIndex("dbo.ProjectFlowNode", new[] { "ProjectFlowID" });
            DropIndex("dbo.Flow", new[] { "CusUserID" });
            DropIndex("dbo.FlowNode", new[] { "NodeID" });
            DropIndex("dbo.FlowNode", new[] { "FlowID" });
            AlterColumn("dbo.Flow", "Title", c => c.String(maxLength: 30));
            DropColumn("dbo.Flow", "CusUserID");
            DropTable("dbo.ProjectFlow");
            DropTable("dbo.ProjectFlowNode");
            DropTable("dbo.FlowNode");
            CreateIndex("dbo.ProjectNodeUser", "CusUserID");
            CreateIndex("dbo.ProjectNodeUser", "ProjectNodeID");
            CreateIndex("dbo.ProjectNodeFile", "CusUserID");
            CreateIndex("dbo.ProjectNodeFile", "ProjectNodeID");
            CreateIndex("dbo.ProjectNode", "PID");
            CreateIndex("dbo.ProjectNode", "ProjectID");
            CreateIndex("dbo.Flow", "PID");
            CreateIndex("dbo.Flow", "NodeID");
            AddForeignKey("dbo.ProjectNodeUser", "ProjectNodeID", "dbo.ProjectNode", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectNodeUser", "CusUserID", "dbo.CusUser", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectNodeFile", "ProjectNodeID", "dbo.ProjectNode", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectNodeFile", "CusUserID", "dbo.CusUser", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectNode", "ProjectID", "dbo.Project", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectNode", "PID", "dbo.ProjectNode", "ID");
            AddForeignKey("dbo.Flow", "PID", "dbo.Flow", "ID");
            AddForeignKey("dbo.Flow", "NodeID", "dbo.Node", "ID", cascadeDelete: true);
        }
    }
}
