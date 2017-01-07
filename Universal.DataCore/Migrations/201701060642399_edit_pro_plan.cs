namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_pro_plan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "ApproveUserID", "dbo.CusUser");
            DropForeignKey("dbo.WorkPlan", "ApproveUserID", "dbo.CusUser");
            DropIndex("dbo.Project", new[] { "ApproveUserID" });
            DropIndex("dbo.WorkPlan", new[] { "ApproveUserID" });
            AlterColumn("dbo.Project", "ApproveUserID", c => c.Int());
            AlterColumn("dbo.WorkPlan", "ApproveUserID", c => c.Int());
            CreateIndex("dbo.Project", "ApproveUserID");
            CreateIndex("dbo.WorkPlan", "ApproveUserID");
            AddForeignKey("dbo.Project", "ApproveUserID", "dbo.CusUser", "ID");
            AddForeignKey("dbo.WorkPlan", "ApproveUserID", "dbo.CusUser", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkPlan", "ApproveUserID", "dbo.CusUser");
            DropForeignKey("dbo.Project", "ApproveUserID", "dbo.CusUser");
            DropIndex("dbo.WorkPlan", new[] { "ApproveUserID" });
            DropIndex("dbo.Project", new[] { "ApproveUserID" });
            AlterColumn("dbo.WorkPlan", "ApproveUserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Project", "ApproveUserID", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkPlan", "ApproveUserID");
            CreateIndex("dbo.Project", "ApproveUserID");
            AddForeignKey("dbo.WorkPlan", "ApproveUserID", "dbo.CusUser", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Project", "ApproveUserID", "dbo.CusUser", "ID", cascadeDelete: true);
        }
    }
}
