namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_project_gzxz : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "GaiZaoXingZhi", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "GaiZaoXingZhi", c => c.Int(nullable: false));
        }
    }
}
