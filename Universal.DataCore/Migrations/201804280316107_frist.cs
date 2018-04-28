namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class frist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClinicArea",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ClinicDepartment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClinicID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Desc = c.String(maxLength: 255),
                        Status = c.Boolean(nullable: false),
                        SZM = c.String(maxLength: 30),
                        Weight = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clinic", t => t.ClinicID, cascadeDelete: true)
                .Index(t => t.ClinicID);
            
            CreateTable(
                "dbo.Clinic",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClinicAreaID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        ICON = c.String(maxLength: 255),
                        WorkHours = c.String(maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        Telphone = c.String(maxLength: 100),
                        FuWuXiangMu = c.String(maxLength: 500),
                        FuWuYuYan = c.String(maxLength: 300),
                        ChengCheLuXian = c.String(maxLength: 1000),
                        Weight = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ClinicArea", t => t.ClinicAreaID, cascadeDelete: true)
                .Index(t => t.ClinicAreaID);
            
            CreateTable(
                "dbo.MPUserDoctors",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ClinicID = c.Int(),
                        TouXian = c.String(nullable: false, maxLength: 100),
                        ShowMe = c.String(maxLength: 200),
                        CanAdvisory = c.Boolean(nullable: false),
                        AdvisoryPrice = c.Decimal(nullable: false, storeType: "money"),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clinic", t => t.ClinicID)
                .ForeignKey("dbo.MPUser", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.ClinicID);
            
            CreateTable(
                "dbo.DoctorsSpecialty",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        SZM = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MPUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OpenID = c.String(nullable: false, maxLength: 255),
                        Identity = c.Byte(nullable: false),
                        Avatar = c.String(maxLength: 255),
                        NickName = c.String(maxLength: 100),
                        RealName = c.String(maxLength: 100),
                        IDCardType = c.Byte(nullable: false),
                        IDCardNumber = c.String(maxLength: 30),
                        Telphone = c.String(maxLength: 50),
                        Gender = c.Byte(nullable: false),
                        Brithday = c.DateTime(storeType: "date"),
                        AccountBalance = c.Decimal(nullable: false, storeType: "money"),
                        IsFullInfo = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastLoginTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.OpenID, unique: true)
                .Index(t => t.IDCardNumber, unique: true)
                .Index(t => t.Telphone, unique: true);
            
            CreateTable(
                "dbo.MedicalBanner",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        ImgUrl = c.String(maxLength: 255),
                        Status = c.Boolean(nullable: false),
                        LinkType = c.Byte(nullable: false),
                        LinkVal = c.String(maxLength: 500),
                        Weight = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MedicalItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OnlyID = c.String(nullable: false, maxLength: 30),
                        Status = c.Boolean(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        SZM = c.String(maxLength: 30),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Weight = c.Int(nullable: false),
                        Desc = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Medical",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        Status = c.Byte(nullable: false),
                        Weight = c.Int(nullable: false),
                        ImgUrl = c.String(maxLength: 255),
                        YPrice = c.Decimal(nullable: false, storeType: "money"),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Desc = c.String(maxLength: 2000),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MPUserAmountDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MPUserID = c.Int(nullable: false),
                        Title = c.String(maxLength: 500),
                        Type = c.Byte(nullable: false),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MPUser", t => t.MPUserID, cascadeDelete: true)
                .Index(t => t.MPUserID);
            
            CreateTable(
                "dbo.MpUserMedicalReport",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDCardNumber = c.String(maxLength: 30),
                        Title = c.String(maxLength: 255),
                        FilePath = c.String(),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NewsCategoryID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        TResource = c.String(nullable: false, maxLength: 30),
                        ImgUrl = c.String(maxLength: 255),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        Summary = c.String(maxLength: 500),
                        Content = c.String(),
                        LinkUrl = c.String(maxLength: 500),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NewsCategory", t => t.NewsCategoryID, cascadeDelete: true)
                .Index(t => t.NewsCategoryID);
            
            CreateTable(
                "dbo.NewsCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 10),
                        Status = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NewsTag",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 10),
                        Weight = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NewsBanner",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 20),
                        ImgUrl = c.String(maxLength: 255),
                        Status = c.Boolean(nullable: false),
                        LinkType = c.Byte(nullable: false),
                        LinkVal = c.String(maxLength: 500),
                        Weight = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderMedicalItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderMedicalID = c.Int(nullable: false),
                        Type = c.Byte(nullable: false),
                        MedicalID = c.Int(nullable: false),
                        Title = c.String(maxLength: 255),
                        Weight = c.Int(nullable: false),
                        OnlyID = c.String(maxLength: 30),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        Desc = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OrderMedical", t => t.OrderMedicalID, cascadeDelete: true)
                .Index(t => t.OrderMedicalID);
            
            CreateTable(
                "dbo.OrderMedical",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderNum = c.String(maxLength: 100),
                        Status = c.Byte(nullable: false),
                        Desc = c.String(maxLength: 255),
                        PayType = c.Byte(nullable: false),
                        Amount = c.Decimal(nullable: false, storeType: "money"),
                        RelAmount = c.Decimal(nullable: false, storeType: "money"),
                        MPUserID = c.Int(nullable: false),
                        MedicalID = c.Int(nullable: false),
                        Title = c.String(maxLength: 255),
                        MYPrice = c.Decimal(nullable: false, storeType: "money"),
                        MPrice = c.Decimal(nullable: false, storeType: "money"),
                        ImgUrl = c.String(maxLength: 255),
                        RealName = c.String(maxLength: 100),
                        IDCardType = c.Byte(nullable: false),
                        IDCardNumber = c.String(maxLength: 30),
                        Telphone = c.String(maxLength: 50),
                        Gender = c.Byte(nullable: false),
                        Brithday = c.DateTime(storeType: "date"),
                        YuYueDate = c.DateTime(nullable: false),
                        PayTime = c.DateTime(),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MPUser", t => t.MPUserID, cascadeDelete: true)
                .Index(t => t.OrderNum, unique: true)
                .Index(t => t.MPUserID);
            
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
            
            CreateTable(
                "dbo.MPUserDoctorsClinicDepartment",
                c => new
                    {
                        MPUserDoctors_ID = c.Int(nullable: false),
                        ClinicDepartment_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MPUserDoctors_ID, t.ClinicDepartment_ID })
                .ForeignKey("dbo.MPUserDoctors", t => t.MPUserDoctors_ID, cascadeDelete: true)
                .ForeignKey("dbo.ClinicDepartment", t => t.ClinicDepartment_ID, cascadeDelete: true)
                .Index(t => t.MPUserDoctors_ID)
                .Index(t => t.ClinicDepartment_ID);
            
            CreateTable(
                "dbo.DoctorsSpecialtyMPUserDoctors",
                c => new
                    {
                        DoctorsSpecialty_ID = c.Int(nullable: false),
                        MPUserDoctors_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DoctorsSpecialty_ID, t.MPUserDoctors_ID })
                .ForeignKey("dbo.DoctorsSpecialty", t => t.DoctorsSpecialty_ID, cascadeDelete: true)
                .ForeignKey("dbo.MPUserDoctors", t => t.MPUserDoctors_ID, cascadeDelete: true)
                .Index(t => t.DoctorsSpecialty_ID)
                .Index(t => t.MPUserDoctors_ID);
            
            CreateTable(
                "dbo.MedicalMedicalItem",
                c => new
                    {
                        Medical_ID = c.Int(nullable: false),
                        MedicalItem_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Medical_ID, t.MedicalItem_ID })
                .ForeignKey("dbo.Medical", t => t.Medical_ID, cascadeDelete: true)
                .ForeignKey("dbo.MedicalItem", t => t.MedicalItem_ID, cascadeDelete: true)
                .Index(t => t.Medical_ID)
                .Index(t => t.MedicalItem_ID);
            
            CreateTable(
                "dbo.NewsTagNewsCategory",
                c => new
                    {
                        NewsTag_ID = c.Int(nullable: false),
                        NewsCategory_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewsTag_ID, t.NewsCategory_ID })
                .ForeignKey("dbo.NewsTag", t => t.NewsTag_ID, cascadeDelete: true)
                .ForeignKey("dbo.NewsCategory", t => t.NewsCategory_ID, cascadeDelete: true)
                .Index(t => t.NewsTag_ID)
                .Index(t => t.NewsCategory_ID);
            
            CreateTable(
                "dbo.NewsTagNews",
                c => new
                    {
                        NewsTag_ID = c.Int(nullable: false),
                        News_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NewsTag_ID, t.News_ID })
                .ForeignKey("dbo.NewsTag", t => t.NewsTag_ID, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.News_ID, cascadeDelete: true)
                .Index(t => t.NewsTag_ID)
                .Index(t => t.News_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SysLogMethod", "SysUserID", "dbo.SysUser");
            DropForeignKey("dbo.SysUser", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.SysRoleRoute", "SysRouteID", "dbo.SysRoute");
            DropForeignKey("dbo.SysRoleRoute", "SysRoleID", "dbo.SysRole");
            DropForeignKey("dbo.OrderMedicalItem", "OrderMedicalID", "dbo.OrderMedical");
            DropForeignKey("dbo.OrderMedical", "MPUserID", "dbo.MPUser");
            DropForeignKey("dbo.News", "NewsCategoryID", "dbo.NewsCategory");
            DropForeignKey("dbo.NewsTagNews", "News_ID", "dbo.News");
            DropForeignKey("dbo.NewsTagNews", "NewsTag_ID", "dbo.NewsTag");
            DropForeignKey("dbo.NewsTagNewsCategory", "NewsCategory_ID", "dbo.NewsCategory");
            DropForeignKey("dbo.NewsTagNewsCategory", "NewsTag_ID", "dbo.NewsTag");
            DropForeignKey("dbo.MPUserAmountDetails", "MPUserID", "dbo.MPUser");
            DropForeignKey("dbo.MedicalMedicalItem", "MedicalItem_ID", "dbo.MedicalItem");
            DropForeignKey("dbo.MedicalMedicalItem", "Medical_ID", "dbo.Medical");
            DropForeignKey("dbo.MPUserDoctors", "ID", "dbo.MPUser");
            DropForeignKey("dbo.DoctorsSpecialtyMPUserDoctors", "MPUserDoctors_ID", "dbo.MPUserDoctors");
            DropForeignKey("dbo.DoctorsSpecialtyMPUserDoctors", "DoctorsSpecialty_ID", "dbo.DoctorsSpecialty");
            DropForeignKey("dbo.MPUserDoctorsClinicDepartment", "ClinicDepartment_ID", "dbo.ClinicDepartment");
            DropForeignKey("dbo.MPUserDoctorsClinicDepartment", "MPUserDoctors_ID", "dbo.MPUserDoctors");
            DropForeignKey("dbo.MPUserDoctors", "ClinicID", "dbo.Clinic");
            DropForeignKey("dbo.ClinicDepartment", "ClinicID", "dbo.Clinic");
            DropForeignKey("dbo.Clinic", "ClinicAreaID", "dbo.ClinicArea");
            DropIndex("dbo.NewsTagNews", new[] { "News_ID" });
            DropIndex("dbo.NewsTagNews", new[] { "NewsTag_ID" });
            DropIndex("dbo.NewsTagNewsCategory", new[] { "NewsCategory_ID" });
            DropIndex("dbo.NewsTagNewsCategory", new[] { "NewsTag_ID" });
            DropIndex("dbo.MedicalMedicalItem", new[] { "MedicalItem_ID" });
            DropIndex("dbo.MedicalMedicalItem", new[] { "Medical_ID" });
            DropIndex("dbo.DoctorsSpecialtyMPUserDoctors", new[] { "MPUserDoctors_ID" });
            DropIndex("dbo.DoctorsSpecialtyMPUserDoctors", new[] { "DoctorsSpecialty_ID" });
            DropIndex("dbo.MPUserDoctorsClinicDepartment", new[] { "ClinicDepartment_ID" });
            DropIndex("dbo.MPUserDoctorsClinicDepartment", new[] { "MPUserDoctors_ID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRouteID" });
            DropIndex("dbo.SysRoleRoute", new[] { "SysRoleID" });
            DropIndex("dbo.SysRole", new[] { "RoleName" });
            DropIndex("dbo.SysUser", new[] { "SysRoleID" });
            DropIndex("dbo.SysUser", new[] { "UserName" });
            DropIndex("dbo.SysLogMethod", new[] { "SysUserID" });
            DropIndex("dbo.OrderMedical", new[] { "MPUserID" });
            DropIndex("dbo.OrderMedical", new[] { "OrderNum" });
            DropIndex("dbo.OrderMedicalItem", new[] { "OrderMedicalID" });
            DropIndex("dbo.News", new[] { "NewsCategoryID" });
            DropIndex("dbo.MPUserAmountDetails", new[] { "MPUserID" });
            DropIndex("dbo.MPUser", new[] { "Telphone" });
            DropIndex("dbo.MPUser", new[] { "IDCardNumber" });
            DropIndex("dbo.MPUser", new[] { "OpenID" });
            DropIndex("dbo.MPUserDoctors", new[] { "ClinicID" });
            DropIndex("dbo.MPUserDoctors", new[] { "ID" });
            DropIndex("dbo.Clinic", new[] { "ClinicAreaID" });
            DropIndex("dbo.ClinicDepartment", new[] { "ClinicID" });
            DropTable("dbo.NewsTagNews");
            DropTable("dbo.NewsTagNewsCategory");
            DropTable("dbo.MedicalMedicalItem");
            DropTable("dbo.DoctorsSpecialtyMPUserDoctors");
            DropTable("dbo.MPUserDoctorsClinicDepartment");
            DropTable("dbo.SysRoute");
            DropTable("dbo.SysRoleRoute");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysUser");
            DropTable("dbo.SysLogMethod");
            DropTable("dbo.SysLogException");
            DropTable("dbo.OrderMedical");
            DropTable("dbo.OrderMedicalItem");
            DropTable("dbo.NewsBanner");
            DropTable("dbo.NewsTag");
            DropTable("dbo.NewsCategory");
            DropTable("dbo.News");
            DropTable("dbo.MpUserMedicalReport");
            DropTable("dbo.MPUserAmountDetails");
            DropTable("dbo.Medical");
            DropTable("dbo.MedicalItem");
            DropTable("dbo.MedicalBanner");
            DropTable("dbo.MPUser");
            DropTable("dbo.DoctorsSpecialty");
            DropTable("dbo.MPUserDoctors");
            DropTable("dbo.Clinic");
            DropTable("dbo.ClinicDepartment");
            DropTable("dbo.ClinicArea");
        }
    }
}
