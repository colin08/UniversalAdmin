namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_project : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "ApproveStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.Project", "ApproveRemark", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "ApproveRemark");
            DropColumn("dbo.Project", "ApproveStatus");
        }
    }
}
