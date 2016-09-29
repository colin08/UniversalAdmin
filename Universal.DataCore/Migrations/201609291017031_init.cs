namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppVersion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Platforms = c.Int(nullable: false),
                        APPType = c.Int(nullable: false),
                        MD5 = c.String(nullable: false, maxLength: 100),
                        Size = c.Long(nullable: false),
                        Version = c.String(nullable: false, maxLength: 20),
                        VersionCode = c.Int(nullable: false),
                        LogoImg = c.String(nullable: false, maxLength: 255),
                        DownUrl = c.String(nullable: false, maxLength: 255),
                        LinkUrl = c.String(maxLength: 500),
                        Content = c.String(nullable: false, maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Demo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Telphone = c.String(maxLength: 15),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ran = c.Int(nullable: false),
                        Num = c.Single(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        AddUserID = c.Int(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                        LastUpdateUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.AddUserID, cascadeDelete: false)
                .ForeignKey("dbo.SysUser", t => t.LastUpdateUserID, cascadeDelete: false)
                .Index(t => t.AddUserID)
                .Index(t => t.LastUpdateUserID);
            
            CreateTable(
                "dbo.SysUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 20),
                        NickName = c.String(nullable: false, maxLength: 30),
                        Gender = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 255),
                        Status = c.Boolean(nullable: false),
                        Avatar = c.String(),
                        SysRoleID = c.Int(nullable: false),
                        RegTime = c.DateTime(nullable: false),
                        LastLoginTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysRole", t => t.SysRoleID, cascadeDelete: true)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.SysRoleID);
            
            CreateTable(
                "dbo.SysRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 30),
                        IsAdmin = c.Boolean(nullable: false),
                        RoleDesc = c.String(nullable: false, maxLength: 255),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.RoleName, unique: true);
            
            CreateTable(
                "dbo.SysRoleRoute",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SysRoleID = c.Int(nullable: false),
                        SysRouteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysRole", t => t.SysRoleID, cascadeDelete: true)
                .ForeignKey("dbo.SysRoute", t => t.SysRouteID, cascadeDelete: true)
                .Index(t => t.SysRoleID)
                .Index(t => t.SysRouteID);
            
            CreateTable(
                "dbo.SysRoute",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Tag = c.String(maxLength: 30),
                        IsPost = c.Boolean(nullable: false),
                        Route = c.String(maxLength: 30),
                        Desc = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DemoAlbum",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DemoID = c.Int(nullable: false),
                        ImgUrl = c.String(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Demo", t => t.DemoID, cascadeDelete: true)
                .Index(t => t.DemoID);
            
            CreateTable(
                "dbo.DemoDept",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DemoID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        ImgUrl = c.String(nullable: false, maxLength: 255),
                        Num = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Demo", t => t.DemoID, cascadeDelete: true)
                .Index(t => t.DemoID);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20),
                        Telphone = c.String(maxLength: 20),
                        Email = c.String(maxLength: 50),
                        IsRead = c.Boolean(nullable: false),
                        Content = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SysLogApiAction",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Uri = c.String(maxLength: 500),
                        ControllerName = c.String(maxLength: 20),
                        ActionName = c.String(maxLength: 20),
                        ExecuteStartTime = c.DateTime(nullable: false),
                        ExecuteEndTime = c.DateTime(nullable: false),
                        ExecuteTime = c.Double(nullable: false),
                        ActionParams = c.String(),
                        HttpRequestHeaders = c.String(),
                        IP = c.String(maxLength: 20),
                        HttpMethod = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SysLogException",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 255),
                        Source = c.String(nullable: false, maxLength: 50),
                        StackTrace = c.String(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SysLogMethod",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SysUserID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Detail = c.String(nullable: false, maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.SysUserID, cascadeDelete: true)
                .Index(t => t.SysUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysLogMethod", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.Demo", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.DemoDept", "DemoID", "dbo.Demo");
            DropForeignKey("dbo.DemoAlbum", "DemoID", "dbo.Demo");
            DropForeignKey("dbo.Demo", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUser", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRoleRoute", "SysRouteID", "dbo.SysRoute");
            DropForeignKey("dbo.SysRoleRoute", "SysRoleID", "dbo.SysRole");
            DropIndex("dbo.SysLogMethod", new[] { "SysUserID" });
            DropIndex("dbo.DemoDept", new[] { "DemoID" });
            DropIndex("dbo.DemoAlbum", new[] { "DemoID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRouteID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRoleID" });
            DropIndex("dbo.SysRole", new[] { "RoleName" });
            DropIndex("dbo.SysUser", new[] { "SysRoleID" });
            DropIndex("dbo.SysUser", new[] { "UserName" });
            DropIndex("dbo.Demo", new[] { "LastUpdateUserID" });
            DropIndex("dbo.Demo", new[] { "AddUserID" });
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.SysLogApiAction");
            DropTable("dbo.Feedback");
            DropTable("dbo.DemoDept");
            DropTable("dbo.DemoAlbum");
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysUser");
            DropTable("dbo.Demo");
            DropTable("dbo.AppVersion");
        }
    }
}
