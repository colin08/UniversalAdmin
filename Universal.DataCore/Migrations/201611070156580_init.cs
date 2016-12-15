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

            //根据部门ID获取主管、自动向上级查找
            string fn_GetWorkPlanApproveUserIdsStr = @"CREATE FUNCTION fn_GetWorkPlanApproveUserIds(@department_id int)
                        RETURNS varchar(100)
                        AS
                        BEGIN
                        declare @a VARCHAR(100);
                        set @a='';

                        WITH Record AS(
                            SELECT
                            Id,
                            PId
                        FROM
                            CusDepartment(NOLOCK)
                            WHERE Id=@department_id
                            UNION ALL
                            SELECT
                            a.Id Id,
                            a.PId PId
                        FROM
                            CusDepartment(NOLOCK) a JOIN Record b
                            ON a.Id=b.PId
                        ),AAAAAA as 
                        (
                        select (
                        select (Cast(CusUserID as nvarchar(10)) +',')  from(
                        select TOP 1 ID FROM(
                        SELECT
                            R.Id,
                            R.PId,
		                        A.CusUserID
                        FROM
                            Record as R LEFT JOIN CusDepartmentAdmin as A on R.Id = A.CusDepartmentID
                        ) as T where CusUserID is not null GROUP BY ID ) as ZZ left join CusDepartmentAdmin as SS on ZZ.Id = SS.CusDepartmentID
                        for xml path('')
                        ) as ids
                        ) 

                        select @a = ids from AAAAAA
                        if(@a is null)
	                        set @a= ''
                        ELSE
	                        set @a = SUBSTRING(@a,0, len(@a))
                        return @a
                        END";
            Sql(fn_GetWorkPlanApproveUserIdsStr);

            string fn_SplitString = @"
                        ---分割字符串
                        Create function [dbo].[fn_SplitString]
                        (@str nvarchar(max),@split varchar(10))
                        returns @t Table (c1 varchar(100))
                        as
                        begin
	                        if charindex(@split,@str) = 0
	                        begin
		                        insert @t(c1) values(@str)
		                        return
	                        end

                            declare @i int
                            declare @s int
                            set @i=1
                            set @s=1
                            while(@i>0)
                            begin    
                                set @i=charindex(@split,@str,@s)
                                if(@i>0)
                                begin
                                    insert @t(c1) values(substring(@str,@s,@i-@s))
                                end   
                                else begin
                                    insert @t(c1) values(substring(@str,@s,len(@str)-@s+1))
                                end
                                set @s = @i + 1   
                            end
                            return
                        end
                        ";
            Sql(fn_SplitString);

            string sp_StatcticsDomain = @"
                    ---面积统计
                    ---exec sp_StatcticsDomain ''
                    CREATE proc [dbo].[sp_StatcticsDomain]
                    @project_id_str nvarchar(1000) --项目str
                    as
                    begin

                    declare @y_data nvarchar(2000) ---x轴数据
                    declare @x_data nvarchar(100) ----y轴数据
                    declare @mianji DECIMAL --临时面积变量
                    set @y_data = ''
                    set @x_data = ''


                    Declare @i_mianji int ---循环面积临时表需要用的变量
                    Declare @count_mianji int  ---要循环面积分类的数量
                    declare @title_mianji nvarchar(300)  ---循环面积的标题
                    declare @field_mianji nvarchar(50) ---面积查询的字段
                    declare @mianji_sql nvarchar(2000) ---面积查询的sql
                    set @count_mianji =0
                    set @title_mianji = ''


                    ---面积分类
                    if object_id(N'#tb_mianji',N'U') is not null
                    begin
	                    drop table #tb_mianji
                    end
                    ELSE
                    BEGIN
	                    create table #tb_mianji(
		                    OID int identity(1,1),---自增ID
		                    FieldName nvarchar(50), ---查询的字段名字
		                    Title nvarchar(300)
	                    )
                    END

                    insert into #tb_mianji VALUES('GengXinDanYuanYongDiMianJi','更新单元用地面积');
                    insert into #tb_mianji VALUES('KaiFaMianJi','开发建设用地面积');
                    insert into #tb_mianji VALUES('WuLeiQuanMianJi','五类权属用地面积');
                    insert into #tb_mianji VALUES('LaoWuCunMianJi','老屋村用地面积');
                    select @count_mianji = COUNT(1) from #tb_mianji

                    declare @i_project int --循环项目用的临时变量
                    declare @count_project int --循环项目的数量
                    declare @id_project int ---循环项目的id
                    declare @title_project nvarchar(300) --循环项目的标题

                    --查询的项目
                    if object_id(N'#tb_project',N'U') is not null
                    begin
	                    drop table #tb_project
                    end
                    ELSE
                    BEGIN
	                    create table #tb_project(
		                    OID int identity(1,1),---自增ID
		                    ID int, ---数据ID
		                    Title nvarchar(300)
	                    )
                    END

                    if len(@project_id_str) > 0
                    BEGIN
	                    insert into #tb_project select ID,Title from Project where ID in(select * from dbo.fn_SplitString(@project_id_str,','));
                    END
                    else
                    begin
	                    insert into #tb_project select ID,Title from Project;
                    end

                    select @count_project = COUNT(1) from #tb_project

                    ----循环面积分类
                    set @i_mianji = 0
                    if @count_project >0
                    begin
                    while @i_mianji < @count_mianji
                    begin
	                    select @title_mianji = Title,@field_mianji=FieldName from #tb_mianji where OID = @i_mianji +1
	                    set @y_data +='{ name: '''+@title_mianji+''', data: ['
	
	                    --循环项目
	                    set @i_project =0
	                    while @i_project < @count_project
	                    BEGIN
			                    select @id_project = ID,@title_project = Title from #tb_project where OID = @i_project +1	
			                    set @mianji = 0;
			                    set @mianji_sql ='select @mianji= '+@field_mianji+' from Project where ID = '+cast(@id_project as nvarchar(10));
			                    exec sp_executesql @mianji_sql,N'@mianji DECIMAL OUTPUT',@mianji output 
			                    set @y_data += CAST(ISNULL(@mianji, 0)as nvarchar(10)) +','
			                    print @id_project
			                    --生成x轴数据
			                    if @i_mianji = 0
			                    begin
				                    set @x_data += ''''+@title_project+''''+','
			                    end
			
			
			                    set @i_project = @i_project+1
	                    END
	                    set @y_data = LEFT(@y_data,LEN(@y_data)-1)
	                    set @y_data += ']},'	
	                    set @i_mianji = @i_mianji +1
                    end
                    end
                    if LEN(@y_data)>0
                    begin
	                    set @y_data = LEFT(@y_data,LEN(@y_data)-1)
                    end
                    if LEN(@x_data) > 0
                    begin
	                    set @x_data = LEFT(@x_data,LEN(@x_data)-1)
                    end

                    select @x_data as x_data,@y_data as y_data
                    drop table #tb_mianji
                    drop table 	#tb_project
                    end";
            Sql(sp_StatcticsDomain);

            string fn_ProjectHaveNode = @"
                ---判断项目是否有某个节点
                CREATE function [dbo].[fn_ProjectHaveNode](@project_id int,@node_id int)
                RETURNS int
                as
                begin
	                declare @total INT
	                set @total =0
	                select  @total=count(1) from Project as P left JOIN ProjectFlowNode as N on P.ID = N.ProjectID where P.ID = @project_id and N.NodeID = @node_id and N.IsEnd=1
	                return @total
                end

                ";
            Sql(fn_ProjectHaveNode);

            string sp_StatcticsProjectTotal = @"
                        ---项目数量统计
                        ---exec sp_StatcticsProjectTotal 1,2,0,0
                        CREATE proc [dbo].[sp_StatcticsProjectTotal]
                        @jidu int,---季度参数
                        @area int,---区域参数
                        @gz int, ----改造性质
                        @node_id int ---项目节点
                        as
                        begin

                        declare @y_data nvarchar(2000) ---x轴数据
                        declare @x_data nvarchar(100) ----y轴数据
                        declare @total int --临时数量变量
                        set @y_data = ''
                        set @x_data = ''


                        Declare @i_year int ---循环年度需要用的变量
                        Declare @count_year int  ---要年度的数量
                        DECLARE @sqls nvarchar(2000) ---查询的sql
                        declare @sqlWhere nvarchar(500) ---查询的where
                        declare @sqlfild nvarchar(200)
                        declare @title_year int  ---年份名称
                        declare @total_year int
                        set @count_year =0
                        set @sqls =''
                        set @sqlWhere=''

                        ---面积分类
                        if object_id(N'#tb_year',N'U') is not null
                        begin
	                        drop table #tb_year
                        end
                        ELSE
                        BEGIN
	                        create table #tb_year(
		                        OID int identity(1,1),---自增ID
		                        tjyear int ---年份
	                        )
                        END
                        insert into #tb_year select TJYear from Project Group BY TJYear ORDER BY TJYear ASC
                        select @count_year = COUNT(1) from #tb_year
                        print '数量：'+cast(@count_year as varchar(10))
                        set @y_data +='{ name: ''数量'', data: ['

                        ----循环面积分类
                        set @i_year = 0
                        if @count_year >0
                        begin
                        while @i_year < @count_year
                        begin
	                        select @title_year = tjyear from #tb_year where OID = @i_year +1
	                        set @sqlfild =''
	                        set @sqlWhere =' where TJYear='+cast(@title_year as varchar(20))
	                        if @jidu>0
	                        BEGIN
		                        set @sqlWhere += ' and  TJQuarter = '+ CAST(@jidu as varchar(10))
	                        end
	                        if @area > 0
	                        BEGIN
		                        set @sqlWhere += ' and  Area = '+ CAST(@area as varchar(10))
	                        END
	                        if @gz > 0
	                        BEGIN
		                        set @sqlWhere += ' and  GaiZaoXingZhi = '+ CAST(@gz as varchar(10))
	                        END
	                        if @node_id>0
	                        BEGIN
		                        set @sqlfild = ',(select dbo.fn_ProjectHaveNode(ID,'+cast(@node_id as varchar(10))+')) as NodeTotal'
		                        set @sqlWhere += ' and  NodeTotal >0 '
	                        END
		

	                        set @sqls='select @total_year = count(1) from (select * from (select *'+@sqlfild+' from Project) as S '+@sqlWhere+') as T'
	                        print @sqls
	                        exec sp_executesql @sqls,N'@total_year INT OUTPUT',@total_year output;

	                        set @y_data += Cast(@total_year as varchar(5)) + ','
	                        --生成x轴数据
	                        set @x_data += ''''+Cast(@title_year as varchar(10))+''''+','
	                        set @i_year = @i_year +1
	
                        end
	                        set @y_data = LEFT(@y_data,LEN(@y_data)-1)
                        end

                        set @y_data += ']},'	
                        if LEN(@y_data)>0
                        begin
	                        set @y_data = LEFT(@y_data,LEN(@y_data)-1)
                        end
                        if LEN(@x_data) > 0
                        begin
	                        set @x_data = LEFT(@x_data,LEN(@x_data)-1)
                        end

                        select @x_data as x_data,@y_data as y_data
                        drop table #tb_year

                        end
                        ";
            Sql(sp_StatcticsProjectTotal);
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
