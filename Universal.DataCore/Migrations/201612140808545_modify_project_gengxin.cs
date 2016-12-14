namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_project_gengxin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "GengXinDanYuanYongDiMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "GengXinDanYuanYongDiMianJi");
        }
    }
}
