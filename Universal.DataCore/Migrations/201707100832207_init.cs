namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SysDbBack",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DbName = c.String(nullable: false, maxLength: 30),
                        BackName = c.String(nullable: false, maxLength: 50),
                        BackType = c.Byte(nullable: false),
                        FilePath = c.String(maxLength: 255),
                        Remark = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                        AddUserID = c.Int(),
                        LastUpdateTime = c.DateTime(nullable: false),
                        LastUpdateUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.AddUserID)
                .ForeignKey("dbo.SysUser", t => t.LastUpdateUserID)
                .Index(t => t.AddUserID)
                .Index(t => t.LastUpdateUserID);
            
            CreateTable(
                "dbo.SysUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SysMerchantID = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20),
                        NickName = c.String(nullable: false, maxLength: 30),
                        Gender = c.Byte(nullable: false),
                        Password = c.String(nullable: false, maxLength: 255),
                        Status = c.Boolean(nullable: false),
                        Avatar = c.String(),
                        SysRoleID = c.Int(nullable: false),
                        RegTime = c.DateTime(nullable: false),
                        LastLoginTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysMerchant", t => t.SysMerchantID, cascadeDelete: true)
                .ForeignKey("dbo.SysRole", t => t.SysRoleID, cascadeDelete: true)
                .Index(t => t.SysMerchantID)
                .Index(t => t.SysRoleID);
            
            CreateTable(
                "dbo.SysMerchant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Remark = c.String(maxLength: 1000),
                        Status = c.Boolean(nullable: false),
                        IsSuperMch = c.Boolean(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastLoginTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Title, unique: true);
            
            CreateTable(
                "dbo.SysRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SysMerchantID = c.Int(nullable: false),
                        RoleName = c.String(nullable: false, maxLength: 30),
                        IsAdmin = c.Boolean(nullable: false),
                        RoleDesc = c.String(nullable: false, maxLength: 255),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysMerchant", t => t.SysMerchantID, cascadeDelete: false)
                .Index(t => t.SysMerchantID);
            
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
                        IsSuperMch = c.Boolean(nullable: false),
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
                        SysMerchantID = c.Int(nullable: false),
                        SysUserID = c.Int(nullable: false),
                        Type = c.Byte(nullable: false),
                        Detail = c.String(nullable: false, maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysMerchant", t => t.SysMerchantID, cascadeDelete: false)
                .ForeignKey("dbo.SysUser", t => t.SysUserID, cascadeDelete: true)
                .Index(t => t.SysMerchantID)
                .Index(t => t.SysUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysLogMethod", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysLogMethod", "SysMerchantID", "dbo.SysMerchant");
            DropForeignKey("dbo.SysDbBack", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysDbBack", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUser", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRoleRoute", "SysRouteID", "dbo.SysRoute");
            DropForeignKey("dbo.SysRoleRoute", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRole", "SysMerchantID", "dbo.SysMerchant");
            DropForeignKey("dbo.SysUser", "SysMerchantID", "dbo.SysMerchant");
            DropIndex("dbo.SysLogMethod", new[] { "SysUserID" });
            DropIndex("dbo.SysLogMethod", new[] { "SysMerchantID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRouteID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRoleID" });
            DropIndex("dbo.SysRole", new[] { "SysMerchantID" });
            DropIndex("dbo.SysMerchant", new[] { "Title" });
            DropIndex("dbo.SysUser", new[] { "SysRoleID" });
            DropIndex("dbo.SysUser", new[] { "SysMerchantID" });
            DropIndex("dbo.SysDbBack", new[] { "LastUpdateUserID" });
            DropIndex("dbo.SysDbBack", new[] { "AddUserID" });
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.SysLogApiAction");
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysMerchant");
            DropTable("dbo.SysUser");
            DropTable("dbo.SysDbBack");
        }
    }
}
