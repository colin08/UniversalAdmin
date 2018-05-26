namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_C_pay_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Consultation", "PayType", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Consultation", "PayType");
        }
    }
}
