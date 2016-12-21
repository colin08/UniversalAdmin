namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_docpost_clomn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocPost", "Content", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocPost", "Content");
        }
    }
}
