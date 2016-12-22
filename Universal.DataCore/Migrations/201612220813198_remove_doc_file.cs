namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_doc_file : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DocPost", "FilePath");
            DropColumn("dbo.DocPost", "FileSize");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocPost", "FileSize", c => c.String(maxLength: 30));
            AddColumn("dbo.DocPost", "FilePath", c => c.String(maxLength: 500));
        }
    }
}
