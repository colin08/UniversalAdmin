namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CaseShow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Type = c.Byte(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        ImgUrl = c.String(maxLength: 300),
                        Time = c.String(nullable: false, maxLength: 20),
                        Address = c.String(nullable: false, maxLength: 20),
                        Summary = c.String(maxLength: 500),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        Content = c.String(),
                        AddTime = c.DateTime(nullable: false),
                        AddUserID = c.Int(),
                        LastUpdateTime = c.DateTime(nullable: false),
                        LastUpdateUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.AddUserID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.SysUser", t => t.LastUpdateUserID)
                .Index(t => t.CategoryID)
                .Index(t => t.AddUserID)
                .Index(t => t.LastUpdateUserID);
            
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
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        CallName = c.String(nullable: false, maxLength: 100),
                        PID = c.Int(),
                        Depth = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        ImgUrl = c.String(maxLength: 300),
                        Summary = c.String(maxLength: 300),
                        Remark = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                        AddUserID = c.Int(),
                        LastUpdateTime = c.DateTime(nullable: false),
                        LastUpdateUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.AddUserID)
                .ForeignKey("dbo.SysUser", t => t.LastUpdateUserID)
                .ForeignKey("dbo.Category", t => t.PID)
                .Index(t => t.PID)
                .Index(t => t.AddUserID)
                .Index(t => t.LastUpdateUserID);
            
            CreateTable(
                "dbo.TeamWork",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ImgUrl = c.String(nullable: false, maxLength: 300),
                        ImgUrl2 = c.String(nullable: false, maxLength: 300),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
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
                "dbo.FutureVision",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ErTitle = c.String(maxLength: 100),
                        ImgUrl = c.String(maxLength: 300),
                        Content = c.String(nullable: false, maxLength: 2000),
                        Content2 = c.String(maxLength: 2000),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
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
                "dbo.HomeBanner",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        LinkType = c.Int(nullable: false),
                        LinkVal = c.String(nullable: false, maxLength: 500),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        ImgUrl = c.String(maxLength: 300),
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
                "dbo.Honour",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        ImgUrl = c.String(maxLength: 300),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
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
                "dbo.JoinUSCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ImgUrl = c.String(nullable: false, maxLength: 300),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
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
                "dbo.JoinUS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JoinUSCategoryID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 30),
                        Address = c.String(nullable: false, maxLength: 30),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        Content = c.String(),
                        TimeOut = c.DateTime(),
                        AddTime = c.DateTime(nullable: false),
                        AddUserID = c.Int(),
                        LastUpdateTime = c.DateTime(nullable: false),
                        LastUpdateUserID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.AddUserID)
                .ForeignKey("dbo.JoinUSCategory", t => t.JoinUSCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.SysUser", t => t.LastUpdateUserID)
                .Index(t => t.JoinUSCategoryID)
                .Index(t => t.AddUserID)
                .Index(t => t.LastUpdateUserID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Type = c.Byte(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ImgUrl = c.String(maxLength: 300),
                        Weight = c.Int(nullable: false),
                        Source = c.String(maxLength: 30),
                        Author = c.String(maxLength: 30),
                        Summary = c.String(maxLength: 300),
                        Content = c.String(),
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
                "dbo.TimeLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ImgUrl = c.String(maxLength: 300),
                        Content = c.String(nullable: false, maxLength: 2000),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
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
                "dbo.TeamWorkCaseShow",
                c => new
                    {
                        TeamWork_ID = c.Int(nullable: false),
                        CaseShow_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeamWork_ID, t.CaseShow_ID })
                .ForeignKey("dbo.TeamWork", t => t.TeamWork_ID, cascadeDelete: true)
                .ForeignKey("dbo.CaseShow", t => t.CaseShow_ID, cascadeDelete: true)
                .Index(t => t.TeamWork_ID)
                .Index(t => t.CaseShow_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeLine", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.TimeLine", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysLogMethod", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.News", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.News", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.JoinUS", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.JoinUS", "JoinUSCategoryID", "dbo.JoinUSCategory");
            DropForeignKey("dbo.JoinUS", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.JoinUSCategory", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.JoinUSCategory", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.Honour", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.Honour", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.HomeBanner", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.HomeBanner", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.FutureVision", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.FutureVision", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.CusCategory", "PID", "dbo.CusCategory");
            DropForeignKey("dbo.TeamWork", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.TeamWorkCaseShow", "CaseShow_ID", "dbo.CaseShow");
            DropForeignKey("dbo.TeamWorkCaseShow", "TeamWork_ID", "dbo.TeamWork");
            DropForeignKey("dbo.TeamWork", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.CaseShow", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.CaseShow", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.Category", "PID", "dbo.Category");
            DropForeignKey("dbo.Category", "LastUpdateUserID", "dbo.SysUser");
            DropForeignKey("dbo.Category", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.CaseShow", "AddUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUser", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRoleRoute", "SysRouteID", "dbo.SysRoute");
            DropForeignKey("dbo.SysRoleRoute", "SysRoleID", "dbo.SysRole");
            DropIndex("dbo.TeamWorkCaseShow", new[] { "CaseShow_ID" });
            DropIndex("dbo.TeamWorkCaseShow", new[] { "TeamWork_ID" });
            DropIndex("dbo.TimeLine", new[] { "LastUpdateUserID" });
            DropIndex("dbo.TimeLine", new[] { "AddUserID" });
            DropIndex("dbo.SysLogMethod", new[] { "SysUserID" });
            DropIndex("dbo.News", new[] { "LastUpdateUserID" });
            DropIndex("dbo.News", new[] { "AddUserID" });
            DropIndex("dbo.JoinUS", new[] { "LastUpdateUserID" });
            DropIndex("dbo.JoinUS", new[] { "AddUserID" });
            DropIndex("dbo.JoinUS", new[] { "JoinUSCategoryID" });
            DropIndex("dbo.JoinUSCategory", new[] { "LastUpdateUserID" });
            DropIndex("dbo.JoinUSCategory", new[] { "AddUserID" });
            DropIndex("dbo.Honour", new[] { "LastUpdateUserID" });
            DropIndex("dbo.Honour", new[] { "AddUserID" });
            DropIndex("dbo.HomeBanner", new[] { "LastUpdateUserID" });
            DropIndex("dbo.HomeBanner", new[] { "AddUserID" });
            DropIndex("dbo.FutureVision", new[] { "LastUpdateUserID" });
            DropIndex("dbo.FutureVision", new[] { "AddUserID" });
            DropIndex("dbo.CusCategory", new[] { "PID" });
            DropIndex("dbo.TeamWork", new[] { "LastUpdateUserID" });
            DropIndex("dbo.TeamWork", new[] { "AddUserID" });
            DropIndex("dbo.Category", new[] { "LastUpdateUserID" });
            DropIndex("dbo.Category", new[] { "AddUserID" });
            DropIndex("dbo.Category", new[] { "PID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRouteID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRoleID" });
            DropIndex("dbo.SysRole", new[] { "RoleName" });
            DropIndex("dbo.SysUser", new[] { "SysRoleID" });
            DropIndex("dbo.SysUser", new[] { "UserName" });
            DropIndex("dbo.CaseShow", new[] { "LastUpdateUserID" });
            DropIndex("dbo.CaseShow", new[] { "AddUserID" });
            DropIndex("dbo.CaseShow", new[] { "CategoryID" });
            DropTable("dbo.TeamWorkCaseShow");
            DropTable("dbo.TimeLine");
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.News");
            DropTable("dbo.JoinUS");
            DropTable("dbo.JoinUSCategory");
            DropTable("dbo.Honour");
            DropTable("dbo.HomeBanner");
            DropTable("dbo.FutureVision");
            DropTable("dbo.CusCategory");
            DropTable("dbo.TeamWork");
            DropTable("dbo.Category");
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysUser");
            DropTable("dbo.CaseShow");
        }
    }
}
