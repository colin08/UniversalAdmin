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
                        Platforms = c.Byte(nullable: false),
                        APPType = c.Byte(nullable: false),
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
                "dbo.CusDepartmentAdmin",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusDepartmentID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusDepartment", t => t.CusDepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .Index(t => t.CusDepartmentID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.CusDepartment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        PID = c.Int(),
                        Depth = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusDepartment", t => t.PID)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.CusUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Telphone = c.String(nullable: false, maxLength: 20),
                        NickName = c.String(maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 255),
                        IsAdmin = c.Boolean(nullable: false),
                        CusDepartmentID = c.Int(nullable: false),
                        CusUserJobID = c.Int(nullable: false),
                        Gender = c.Byte(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Avatar = c.String(),
                        Brithday = c.DateTime(),
                        ShorNum = c.String(maxLength: 20),
                        Email = c.String(),
                        AboutMe = c.String(maxLength: 500),
                        RegTime = c.DateTime(nullable: false),
                        LastLoginTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusDepartment", t => t.CusDepartmentID, cascadeDelete: true)
                .ForeignKey("dbo.CusUserJob", t => t.CusUserJobID, cascadeDelete: true)
                .Index(t => t.Telphone, unique: true)
                .Index(t => t.CusDepartmentID)
                .Index(t => t.CusUserJobID);
            
            CreateTable(
                "dbo.CusUserJob",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CusUserRoute",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        CusRouteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusRoute", t => t.CusRouteID, cascadeDelete: true)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID)
                .Index(t => t.CusRouteID);
            
            CreateTable(
                "dbo.CusRoute",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        ControllerName = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CusNotice",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false, unicode: false, storeType: "text"),
                        CusUserID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.CusNoticeUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusNoticeID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusNotice", t => t.CusNoticeID, cascadeDelete: true)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .Index(t => t.CusNoticeID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.CusUserFavorites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CusUserID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        TOID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.CusVerification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        Code = c.String(nullable: false, maxLength: 10),
                        Type = c.Byte(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DocCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        PID = c.Int(),
                        Depth = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DocCategory", t => t.PID)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.DocPostPower",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DocPostID = c.Int(nullable: false),
                        TOID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DocPost", t => t.DocPostID, cascadeDelete: true)
                .Index(t => t.DocPostID);
            
            CreateTable(
                "dbo.DocPost",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        See = c.Int(nullable: false),
                        DocCategoryID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .ForeignKey("dbo.DocCategory", t => t.DocCategoryID, cascadeDelete: true)
                .Index(t => t.DocCategoryID)
                .Index(t => t.CusUserID);
            
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
                        Route = c.String(maxLength: 30),
                        Desc = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);

            //按照某一个Id查询它及它的所有子级成员存储过程
            string SQLGetChildDepartments = @"
                    CREATE PROCEDURE [dbo].[sp_GetChildDepartments] (@Id int)
                    AS
                    BEGIN
                    WITH Record AS(
                        SELECT
                        Id,
                        Title,
                        PID,
                        Depth,
                        Status,
                        Priority,
                        AddTime
                    FROM
                        CusDepartment(NOLOCK)
                        WHERE Id=@Id
                        UNION ALL
                            SELECT
                        a.Id Id,
                        a.Title Title,
                        a.PID PID,
                        a.Depth Depth,
                        a.Status Status,
                        a.Priority Priority,
                        a.AddTime AddTime
                    FROM
                        CusDepartment(NOLOCK) a JOIN Record b
                        ON a.PID=b.Id
                    )
 
                    SELECT
                        Id,
                        Title,
                        PID,
                        Depth,
                        Status,
                        Priority,
                        AddTime
                    FROM
                        Record
                        WHERE Status=1
                        ORDER BY Priority DESC     
                    END";
            Sql(SQLGetChildDepartments);

            //按照某一个Id查询它及它的所有父级成员存储过程
            string SQLGetParentDepartments = @"
                        CREATE PROCEDURE [dbo].[sp_GetParentDepartments] (@Id int)
                        AS
                        BEGIN
                        WITH Record AS(
                            SELECT
                            Id,
                            Title,
                            PId,
                            Depth,
                            Status,
                            Priority,
                            AddTime
                        FROM
                            CusDepartment(NOLOCK)
                            WHERE Id=@Id
                            UNION ALL
                            SELECT
                            a.Id Id,
                            a.Title Title,
                            a.PId PId,
                            a.Depth Depth,
                            a.Status Status,
                            a.Priority Priority,
                            a.AddTime AddTime
                        FROM
                            CusDepartment(NOLOCK) a JOIN Record b
                            ON a.Id=b.PId
                        )
 
                        SELECT
                            Id,
                            Title,
                            PId,
                            Depth,
                            Status,
                            Priority,
                            AddTime
                        FROM
                            Record
                            WHERE Status=1
                            ORDER BY Priority DESC
     
                        END";
            Sql(SQLGetParentDepartments);

            //获取所有子类的id，以逗号分割
            string SQLFunGetChildDepartmentStr = @"
                        CREATE FUNCTION [dbo].[fn_GetChildDepartmentStr] (@Id int) RETURNS varchar(1000) 
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
                                CusDepartment(NOLOCK)
                                WHERE Id=@Id
                                UNION ALL
                                    SELECT
				                        a.Id Id,
				                        a.PID PID,
				                        a.Status Status
                                    FROM
                                        CusDepartment(NOLOCK) a JOIN Record b
                                        ON a.PID=b.Id
                                    )

                        SELECT @a=isnull(@a+',','')+ltrim(Id) FROM Record  WHERE Status=1  
                        return SUBSTRING(@a, 2, len(@a))
                        END
                        ";

            Sql(SQLFunGetChildDepartmentStr);

            string SQLProcGetCusRouteExists = @"
                    CREATE PROCEDURE [dbo].[sp_GetCusRouteExists] (@user_id int)
                    as
                    begin
                    select id,title,(case when aa > 0 then 1 else 0 end) as is_check from (select a.ID as id,a.Title as title,(select count(1) from CusUserRoute where CusUserID=@user_id and CusRouteID=a.ID) as aa   from CusRoute as a) as T
                    end
                    ";
            Sql(SQLProcGetCusRouteExists);


            //按照某一个Id查询它及它的所有子级成员存储过程
            string SQLGetChildDocCategory = @"
                    CREATE PROCEDURE [dbo].[sp_GetChildCategorys] (@Id int)
                    AS
                    BEGIN
                    WITH Record AS(
                        SELECT
                        Id,
                        Title,
                        PID,
                        Depth,
                        Status,
                        Priority,
                        AddTime
                    FROM
                        DocCategory(NOLOCK)
                        WHERE Id=@Id
                        UNION ALL
                            SELECT
                        a.Id Id,
                        a.Title Title,
                        a.PID PID,
                        a.Depth Depth,
                        a.Status Status,
                        a.Priority Priority,
                        a.AddTime AddTime
                    FROM
                        DocCategory(NOLOCK) a JOIN Record b
                        ON a.PID=b.Id
                    )
 
                    SELECT
                        Id,
                        Title,
                        PID,
                        Depth,
                        Status,
                        Priority,
                        AddTime
                    FROM
                        Record
                        WHERE Status=1
                        ORDER BY Priority DESC     
                    END";
            Sql(SQLGetChildDocCategory);

            //按照某一个Id查询它及它的所有父级成员存储过程
            string SQLGetParentCategory = @"
                        CREATE PROCEDURE [dbo].[sp_GetParentCategorys] (@Id int)
                        AS
                        BEGIN
                        WITH Record AS(
                            SELECT
                            Id,
                            Title,
                            PId,
                            Depth,
                            Status,
                            Priority,
                            AddTime
                        FROM
                            DocCategory(NOLOCK)
                            WHERE Id=@Id
                            UNION ALL
                            SELECT
                            a.Id Id,
                            a.Title Title,
                            a.PId PId,
                            a.Depth Depth,
                            a.Status Status,
                            a.Priority Priority,
                            a.AddTime AddTime
                        FROM
                            DocCategory(NOLOCK) a JOIN Record b
                            ON a.Id=b.PId
                        )
 
                        SELECT
                            Id,
                            Title,
                            PId,
                            Depth,
                            Status,
                            Priority,
                            AddTime
                        FROM
                            Record
                            WHERE Status=1
                            ORDER BY Priority DESC
     
                        END";
            Sql(SQLGetParentCategory);

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysLogMethod", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUser", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRoleRoute", "SysRouteID", "dbo.SysRoute");
            DropForeignKey("dbo.SysRoleRoute", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.DocPostPower", "DocPostID", "dbo.DocPost");
            DropForeignKey("dbo.DocPost", "DocCategoryID", "dbo.DocCategory");
            DropForeignKey("dbo.DocPost", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.DocCategory", "PID", "dbo.DocCategory");
            DropForeignKey("dbo.CusUserFavorites", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.CusNotice", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.CusNoticeUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.CusNoticeUser", "CusNoticeID", "dbo.CusNotice");
            DropForeignKey("dbo.CusDepartmentAdmin", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.CusUserRoute", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.CusUserRoute", "CusRouteID", "dbo.CusRoute");
            DropForeignKey("dbo.CusUser", "CusUserJobID", "dbo.CusUserJob");
            DropForeignKey("dbo.CusUser", "CusDepartmentID", "dbo.CusDepartment");
            DropForeignKey("dbo.CusDepartment", "PID", "dbo.CusDepartment");
            DropForeignKey("dbo.CusDepartmentAdmin", "CusDepartmentID", "dbo.CusDepartment");
            DropIndex("dbo.SysRoleRoute", new[] { "SysRouteID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRoleID" });
            DropIndex("dbo.SysRole", new[] { "RoleName" });
            DropIndex("dbo.SysUser", new[] { "SysRoleID" });
            DropIndex("dbo.SysUser", new[] { "UserName" });
            DropIndex("dbo.SysLogMethod", new[] { "SysUserID" });
            DropIndex("dbo.DocPost", new[] { "CusUserID" });
            DropIndex("dbo.DocPost", new[] { "DocCategoryID" });
            DropIndex("dbo.DocPostPower", new[] { "DocPostID" });
            DropIndex("dbo.DocCategory", new[] { "PID" });
            DropIndex("dbo.CusUserFavorites", new[] { "CusUserID" });
            DropIndex("dbo.CusNoticeUser", new[] { "CusUserID" });
            DropIndex("dbo.CusNoticeUser", new[] { "CusNoticeID" });
            DropIndex("dbo.CusNotice", new[] { "CusUserID" });
            DropIndex("dbo.CusUserRoute", new[] { "CusRouteID" });
            DropIndex("dbo.CusUserRoute", new[] { "CusUserID" });
            DropIndex("dbo.CusUser", new[] { "CusUserJobID" });
            DropIndex("dbo.CusUser", new[] { "CusDepartmentID" });
            DropIndex("dbo.CusUser", new[] { "Telphone" });
            DropIndex("dbo.CusDepartment", new[] { "PID" });
            DropIndex("dbo.CusDepartmentAdmin", new[] { "CusUserID" });
            DropIndex("dbo.CusDepartmentAdmin", new[] { "CusDepartmentID" });
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysUser");
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.SysLogApiAction");
            DropTable("dbo.Feedback");
            DropTable("dbo.DocPost");
            DropTable("dbo.DocPostPower");
            DropTable("dbo.DocCategory");
            DropTable("dbo.CusVerification");
            DropTable("dbo.CusUserFavorites");
            DropTable("dbo.CusNoticeUser");
            DropTable("dbo.CusNotice");
            DropTable("dbo.CusRoute");
            DropTable("dbo.CusUserRoute");
            DropTable("dbo.CusUserJob");
            DropTable("dbo.CusUser");
            DropTable("dbo.CusDepartment");
            DropTable("dbo.CusDepartmentAdmin");
            DropTable("dbo.AppVersion");
        }
    }
}
