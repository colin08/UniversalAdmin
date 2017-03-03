namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_flow_node_compact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlowNodeCompact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FlowID = c.Int(nullable: false),
                        NodeID = c.Int(nullable: false),
                        IsFrist = c.Boolean(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlowNodeCompact", "NodeID", "dbo.Node");
            DropForeignKey("dbo.FlowNodeCompact", "FlowID", "dbo.Flow");
            DropIndex("dbo.FlowNodeCompact", new[] { "NodeID" });
            DropIndex("dbo.FlowNodeCompact", new[] { "FlowID" });
            DropTable("dbo.FlowNodeCompact");
        }
    }
}
