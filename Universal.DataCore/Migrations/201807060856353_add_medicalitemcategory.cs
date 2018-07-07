namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_medicalitemcategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MedicalItemCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        Remark = c.String(maxLength: 255),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MedicalItemCategory");
        }
    }
}
