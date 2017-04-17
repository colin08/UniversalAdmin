namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_node_user : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NodeUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.NodeUser", "NodeID", "dbo.Node");
            DropIndex("dbo.NodeUser", new[] { "NodeID" });
            DropIndex("dbo.NodeUser", new[] { "CusUserID" });
            AddColumn("dbo.Node", "ContactsUsers", c => c.String(maxLength: 500));
            DropTable("dbo.NodeUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NodeUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NodeID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Node", "ContactsUsers");
            CreateIndex("dbo.NodeUser", "CusUserID");
            CreateIndex("dbo.NodeUser", "NodeID");
            AddForeignKey("dbo.NodeUser", "NodeID", "dbo.Node", "ID", cascadeDelete: true);
            AddForeignKey("dbo.NodeUser", "CusUserID", "dbo.CusUser", "ID", cascadeDelete: true);
        }
    }
}
