namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_project : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Area", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "ZongMianJiOther", c => c.String(maxLength: 200));
            AddColumn("dbo.Project", "TJYear", c => c.Int(nullable: false));
            AddColumn("dbo.Project", "TJQuarter", c => c.Int(nullable: false));
            AlterColumn("dbo.Node", "Content", c => c.String());
            AlterColumn("dbo.Project", "GaiZaoXingZhi", c => c.Int(nullable: false));
            AlterColumn("dbo.Project", "ZongMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "WuLeiQuanMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "LaoWuCunMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "FeiNongMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "KaiFaMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "RongJiLv", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "ChaiQianYongDiMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "ChaiQianJianZhuMianJi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Project", "JunJia", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProjectNode", "Content", c => c.String());
            DropColumn("dbo.Project", "QLTelphone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "QLTelphone", c => c.String(maxLength: 50));
            AlterColumn("dbo.ProjectNode", "Content", c => c.String(maxLength: 2000));
            AlterColumn("dbo.Project", "JunJia", c => c.String(maxLength: 20));
            AlterColumn("dbo.Project", "ChaiQianJianZhuMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "ChaiQianYongDiMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "RongJiLv", c => c.String(maxLength: 20));
            AlterColumn("dbo.Project", "KaiFaMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "FeiNongMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "LaoWuCunMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "WuLeiQuanMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "ZongMianJi", c => c.String(maxLength: 200));
            AlterColumn("dbo.Project", "GaiZaoXingZhi", c => c.String(maxLength: 30));
            AlterColumn("dbo.Node", "Content", c => c.String(maxLength: 2000));
            DropColumn("dbo.Project", "TJQuarter");
            DropColumn("dbo.Project", "TJYear");
            DropColumn("dbo.Project", "ZongMianJiOther");
            DropColumn("dbo.Project", "Area");
        }
    }
}
