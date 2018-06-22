namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CusCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        PID = c.Int(),
                        Depth = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        SortNo = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusCategory", t => t.PID)
                .Index(t => t.PID);
            
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
                        Type = c.Byte(nullable: false),
                        Detail = c.String(nullable: false, maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.SysUserID, cascadeDelete: true)
                .Index(t => t.SysUserID);
            
            CreateTable(
                "dbo.SysUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
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
                        Route = c.String(maxLength: 100),
                        Desc = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysLogMethod", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUser", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRoleRoute", "SysRouteID", "dbo.SysRoute");
            DropForeignKey("dbo.SysRoleRoute", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.CusCategory", "PID", "dbo.CusCategory");
            DropIndex("dbo.SysRoleRoute", new[] { "SysRouteID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRoleID" });
            DropIndex("dbo.SysRole", new[] { "RoleName" });
            DropIndex("dbo.SysUser", new[] { "SysRoleID" });
            DropIndex("dbo.SysUser", new[] { "UserName" });
            DropIndex("dbo.SysLogMethod", new[] { "SysUserID" });
            DropIndex("dbo.CusCategory", new[] { "PID" });
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysUser");
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.CusCategory");
        }
    }
}
