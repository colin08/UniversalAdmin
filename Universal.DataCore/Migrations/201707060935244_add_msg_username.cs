namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_msg_username : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CusUserMessage", "AddUserName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CusUserMessage", "AddUserName");
        }
    }
}
