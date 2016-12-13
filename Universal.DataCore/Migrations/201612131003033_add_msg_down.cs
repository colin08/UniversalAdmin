namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_msg_down : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CusUserMessage", "IsDone", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CusUserMessage", "IsDone");
        }
    }
}
