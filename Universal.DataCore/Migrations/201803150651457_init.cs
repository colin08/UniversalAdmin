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
                        Type = c.Byte(nullable: false),
                        Detail = c.String(nullable: false, maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SysUser", t => t.SysUserID, cascadeDelete: true)
                .Index(t => t.SysUserID);

            //按照某一个Id查询它及它的所有子级成员存储过程
            string SQLGetChildCusCategory = @"
                    CREATE PROCEDURE [dbo].[sp_GetChildCusCategory] (@Id int)
                    AS
                    BEGIN
                    WITH Record AS(
                        SELECT
                        Id,
                        Title,
                        PID,
                        Depth,
                        Status,
                        SortNo,
                        AddTime
                    FROM
                        CusCategory(NOLOCK)
                        WHERE Id=@Id
                        UNION ALL
                            SELECT
                        a.Id Id,
                        a.Title Title,
                        a.PID PID,
                        a.Depth Depth,
                        a.Status Status,
                        a.SortNo SortNo,
                        a.AddTime AddTime
                    FROM
                        CusCategory(NOLOCK) a JOIN Record b
                        ON a.PID=b.Id
                    )
 
                    SELECT
                        Id,
                        Title,
                        PID,
                        Depth,
                        Status,
                        SortNo,
                        AddTime
                    FROM
                        Record
                        WHERE Status=1
                        ORDER BY SortNo DESC     
                    END";
            Sql(SQLGetChildCusCategory);

            //按照某一个Id查询它及它的所有父级成员存储过程
            string SQLGetParentCusCategory = @"
                        CREATE PROCEDURE [dbo].[sp_GetParentCusCategory] (@Id int)
                        AS
                        BEGIN
                        WITH Record AS(
                            SELECT
                            Id,
                            Title,
                            PId,
                            Depth,
                            Status,
                            SortNo,
                            AddTime
                        FROM
                            CusCategory(NOLOCK)
                            WHERE Id=@Id
                            UNION ALL
                            SELECT
                            a.Id Id,
                            a.Title Title,
                            a.PId PId,
                            a.Depth Depth,
                            a.Status Status,
                            a.SortNo SortNo,
                            a.AddTime AddTime
                        FROM
                            CusCategory(NOLOCK) a JOIN Record b
                            ON a.Id=b.PId
                        )
 
                        SELECT
                            Id,
                            Title,
                            PId,
                            Depth,
                            Status,
                            SortNo,
                            AddTime
                        FROM
                            Record
                            WHERE Status=1
                            ORDER BY SortNo DESC
     
                        END";
            Sql(SQLGetParentCusCategory);

            //获取所有子类的id，以逗号分割
            string SQLFunGetChildCusCategoryStr = @"
                        CREATE FUNCTION [dbo].[fn_GetChildCusCategoryStr] (@Id int) RETURNS varchar(1000) 
                        AS
                            BEGIN
                        declare @a VARCHAR(1000);
                        set @a='';
                            WITH Record AS(
                                SELECT
                                Id,
                                PID,
		                            Status
                            FROM
                                CusCategory(NOLOCK)
                                WHERE Id=@Id
                                UNION ALL
                                    SELECT
				                        a.Id Id,
				                        a.PID PID,
				                        a.Status Status
                                    FROM
                                        CusCategory(NOLOCK) a JOIN Record b
                                        ON a.PID=b.Id
                                    )
                        SELECT @a=isnull(@a+',','')+ltrim(Id) FROM Record  WHERE Status=1  
                        return SUBSTRING(@a, 2, len(@a))
                        END
                        ";

            Sql(SQLFunGetChildCusCategoryStr);
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
            DropForeignKey("dbo.CusCategory", "PID", "dbo.CusCategory");
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
            DropIndex("dbo.CusCategory", new[] { "PID" });
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.SysLogApiAction");
            DropTable("dbo.DemoDept");
            DropTable("dbo.DemoAlbum");
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysUser");
            DropTable("dbo.Demo");
            DropTable("dbo.CusCategory");
        }
    }
}
