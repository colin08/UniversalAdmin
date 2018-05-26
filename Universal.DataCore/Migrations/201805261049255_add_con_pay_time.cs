namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_con_pay_time : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Consultation", "PayTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Consultation", "PayTime");
        }
    }
}
