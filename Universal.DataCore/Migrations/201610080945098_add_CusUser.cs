namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_CusUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CusUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Telphone = c.String(nullable: false, maxLength: 20),
                        NickName = c.String(maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 255),
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
                .Index(t => t.Telphone, unique: true);
            
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CusUser", new[] { "Telphone" });
            DropTable("dbo.CusVerification");
            DropTable("dbo.CusUser");
        }
    }
}
