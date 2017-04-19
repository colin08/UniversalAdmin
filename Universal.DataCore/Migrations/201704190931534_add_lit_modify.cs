namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_lit_modify : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectStage", "BeginTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectStage", "BeginTime", c => c.DateTime(nullable: false));
        }
    }
}
