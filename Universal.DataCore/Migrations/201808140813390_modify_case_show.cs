namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_case_show : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CaseShow", "ErTitle", c => c.String(maxLength: 200));
            AddColumn("dbo.CaseShow", "FuWu", c => c.String(maxLength: 200));
            AddColumn("dbo.News", "SourceLinkUrl", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "SourceLinkUrl");
            DropColumn("dbo.CaseShow", "FuWu");
            DropColumn("dbo.CaseShow", "ErTitle");
        }
    }
}
