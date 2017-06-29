namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_job_null : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CusUser", "CusUserJobID", "dbo.CusUserJob");
            DropIndex("dbo.CusUser", new[] { "CusUserJobID" });
            AlterColumn("dbo.CusUser", "CusUserJobID", c => c.Int());
            CreateIndex("dbo.CusUser", "CusUserJobID");
            AddForeignKey("dbo.CusUser", "CusUserJobID", "dbo.CusUserJob", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CusUser", "CusUserJobID", "dbo.CusUserJob");
            DropIndex("dbo.CusUser", new[] { "CusUserJobID" });
            AlterColumn("dbo.CusUser", "CusUserJobID", c => c.Int(nullable: false));
            CreateIndex("dbo.CusUser", "CusUserJobID");
            AddForeignKey("dbo.CusUser", "CusUserJobID", "dbo.CusUserJob", "ID", cascadeDelete: true);
        }
    }
}
