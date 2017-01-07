namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_from_doc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "FlowID", c => c.Int());
            AlterColumn("dbo.Project", "LiXiangTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Project", "LiXiangTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Project", "FlowID", c => c.Int(nullable: false));
        }
    }
}
