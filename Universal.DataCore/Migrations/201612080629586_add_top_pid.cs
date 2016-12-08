namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_top_pid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flow", "FlowName", c => c.String(maxLength: 50));
            AddColumn("dbo.Flow", "TopPID", c => c.Int());
            AddColumn("dbo.ProjectNode", "TopPID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectNode", "TopPID");
            DropColumn("dbo.Flow", "TopPID");
            DropColumn("dbo.Flow", "FlowName");
        }
    }
}
