namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_something : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectNode", "SourceFlowID", c => c.Int(nullable: false));
            AlterColumn("dbo.Flow", "TOID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectNode", "TOID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectNode", "TOID", c => c.String());
            AlterColumn("dbo.Flow", "TOID", c => c.String());
            DropColumn("dbo.ProjectNode", "SourceFlowID");
        }
    }
}
