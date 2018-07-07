namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_medical_item : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MedicalItem", "MedicalItemCategoryID", c => c.Int(nullable: false,defaultValue:1));
            CreateIndex("dbo.MedicalItem", "MedicalItemCategoryID");
            AddForeignKey("dbo.MedicalItem", "MedicalItemCategoryID", "dbo.MedicalItemCategory", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedicalItem", "MedicalItemCategoryID", "dbo.MedicalItemCategory");
            DropIndex("dbo.MedicalItem", new[] { "MedicalItemCategoryID" });
            DropColumn("dbo.MedicalItem", "MedicalItemCategoryID");
        }
    }
}
