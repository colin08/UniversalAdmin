namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_work_plan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkPlan", "ApproveStatus", c => c.Byte(nullable: false));
            AddColumn("dbo.WorkPlan", "ApproveRemark", c => c.String(maxLength: 500));
            DropColumn("dbo.WorkPlan", "IsApprove");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkPlan", "IsApprove", c => c.Boolean(nullable: false));
            DropColumn("dbo.WorkPlan", "ApproveRemark");
            DropColumn("dbo.WorkPlan", "ApproveStatus");
        }
    }
}
