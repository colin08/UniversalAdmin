namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_doc_files : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DocPostID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DocPost", t => t.DocPostID, cascadeDelete: true)
                .Index(t => t.DocPostID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocFile", "DocPostID", "dbo.DocPost");
            DropIndex("dbo.DocFile", new[] { "DocPostID" });
            DropTable("dbo.DocFile");
        }
    }
}
