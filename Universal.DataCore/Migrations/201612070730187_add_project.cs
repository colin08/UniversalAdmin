namespace Universal.DataCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_project : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NodeID = c.Int(nullable: false),
                        Title = c.String(maxLength: 30),
                        PID = c.Int(),
                        TOID = c.String(),
                        Depth = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Node", t => t.NodeID, cascadeDelete: true)
                .ForeignKey("dbo.Flow", t => t.PID)
                .Index(t => t.NodeID)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.Node",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        Location = c.String(maxLength: 500),
                        Content = c.String(maxLength: 2000),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NodeFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NodeID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                        CusUserID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .ForeignKey("dbo.Node", t => t.NodeID, cascadeDelete: true)
                .Index(t => t.NodeID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.NodeUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NodeID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .ForeignKey("dbo.Node", t => t.NodeID, cascadeDelete: true)
                .Index(t => t.NodeID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.ProjectFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.Byte(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 200),
                        CusUserID = c.Int(nullable: false),
                        ApproveUserID = c.Int(nullable: false),
                        QLTelphone = c.String(maxLength: 50),
                        FlowID = c.Int(nullable: false),
                        See = c.Int(nullable: false),
                        TOID = c.String(maxLength: 1000),
                        GaiZaoXingZhi = c.String(maxLength: 30),
                        ZhongDiHao = c.String(maxLength: 200),
                        ShenBaoZhuTi = c.String(maxLength: 200),
                        ZongMianJi = c.String(maxLength: 200),
                        WuLeiQuanMianJi = c.String(maxLength: 200),
                        LaoWuCunMianJi = c.String(maxLength: 200),
                        FeiNongMianJi = c.String(maxLength: 200),
                        KaiFaMianJi = c.String(maxLength: 200),
                        RongJiLv = c.String(maxLength: 20),
                        TuDiShiYongQuan = c.String(maxLength: 200),
                        JianSheGuiHuaZheng = c.String(maxLength: 200),
                        ChaiQianYongDiMianJi = c.String(maxLength: 200),
                        ChaiQianJianZhuMianJi = c.String(maxLength: 200),
                        LiXiangTime = c.DateTime(nullable: false),
                        ZhuanXiangTime = c.DateTime(),
                        ZhuTiTime = c.DateTime(),
                        YongDiTime = c.DateTime(),
                        KaiPanTime = c.DateTime(),
                        FenChengBiLi = c.String(maxLength: 20),
                        JunJia = c.String(maxLength: 20),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.ApproveUserID, cascadeDelete: false)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: false)
                .Index(t => t.CusUserID)
                .Index(t => t.ApproveUserID);
            
            CreateTable(
                "dbo.ProjectNode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        PID = c.Int(),
                        Title = c.String(maxLength: 255),
                        Location = c.String(maxLength: 500),
                        Content = c.String(maxLength: 2000),
                        TOID = c.String(),
                        Depth = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        IsStart = c.Boolean(nullable: false),
                        IsEnd = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                        LastUpdateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProjectNode", t => t.PID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.PID);
            
            CreateTable(
                "dbo.ProjectUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.ProjectNodeFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectNodeID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                        CusUserID = c.Int(nullable: false),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .ForeignKey("dbo.ProjectNode", t => t.ProjectNodeID, cascadeDelete: true)
                .Index(t => t.ProjectNodeID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.ProjectNodeUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectNodeID = c.Int(nullable: false),
                        CusUserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CusUser", t => t.CusUserID, cascadeDelete: true)
                .ForeignKey("dbo.ProjectNode", t => t.ProjectNodeID, cascadeDelete: true)
                .Index(t => t.ProjectNodeID)
                .Index(t => t.CusUserID);
            
            CreateTable(
                "dbo.ProjectStageFile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectStageID = c.Int(nullable: false),
                        FileName = c.String(maxLength: 200),
                        FilePath = c.String(maxLength: 500),
                        FileSize = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProjectStage", t => t.ProjectStageID, cascadeDelete: true)
                .Index(t => t.ProjectStageID);
            
            CreateTable(
                "dbo.ProjectStage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        BeginTime = c.DateTime(nullable: false),
                        ZongHuShu = c.String(maxLength: 50),
                        YiQYHuShu = c.String(maxLength: 50),
                        WeiQYHuShu = c.String(maxLength: 50),
                        ZhanDiMianJi = c.String(maxLength: 50),
                        JiDiMianJi = c.String(maxLength: 50),
                        KongDiMianJi = c.String(maxLength: 50),
                        YiQYMianJi = c.String(maxLength: 50),
                        WeiQYMianJi = c.String(maxLength: 50),
                        ChaiZhanDiMianJi = c.String(maxLength: 50),
                        ChaiJianZhuMianJi = c.String(maxLength: 50),
                        ChaiBuChangMianJi = c.String(maxLength: 50),
                        ChaiBuChangjinE = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Project", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectStageFile", "ProjectStageID", "dbo.ProjectStage");
            DropForeignKey("dbo.ProjectStage", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectNodeUser", "ProjectNodeID", "dbo.ProjectNode");
            DropForeignKey("dbo.ProjectNodeUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.ProjectNodeFile", "ProjectNodeID", "dbo.ProjectNode");
            DropForeignKey("dbo.ProjectNodeFile", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.ProjectUser", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.ProjectNode", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.ProjectNode", "PID", "dbo.ProjectNode");
            DropForeignKey("dbo.ProjectFile", "ProjectID", "dbo.Project");
            DropForeignKey("dbo.Project", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.Project", "ApproveUserID", "dbo.CusUser");
            DropForeignKey("dbo.Flow", "PID", "dbo.Flow");
            DropForeignKey("dbo.Flow", "NodeID", "dbo.Node");
            DropForeignKey("dbo.NodeUser", "NodeID", "dbo.Node");
            DropForeignKey("dbo.NodeUser", "CusUserID", "dbo.CusUser");
            DropForeignKey("dbo.NodeFile", "NodeID", "dbo.Node");
            DropForeignKey("dbo.NodeFile", "CusUserID", "dbo.CusUser");
            DropIndex("dbo.ProjectStage", new[] { "ProjectID" });
            DropIndex("dbo.ProjectStageFile", new[] { "ProjectStageID" });
            DropIndex("dbo.ProjectNodeUser", new[] { "CusUserID" });
            DropIndex("dbo.ProjectNodeUser", new[] { "ProjectNodeID" });
            DropIndex("dbo.ProjectNodeFile", new[] { "CusUserID" });
            DropIndex("dbo.ProjectNodeFile", new[] { "ProjectNodeID" });
            DropIndex("dbo.ProjectUser", new[] { "CusUserID" });
            DropIndex("dbo.ProjectUser", new[] { "ProjectID" });
            DropIndex("dbo.ProjectNode", new[] { "PID" });
            DropIndex("dbo.ProjectNode", new[] { "ProjectID" });
            DropIndex("dbo.Project", new[] { "ApproveUserID" });
            DropIndex("dbo.Project", new[] { "CusUserID" });
            DropIndex("dbo.ProjectFile", new[] { "ProjectID" });
            DropIndex("dbo.NodeUser", new[] { "CusUserID" });
            DropIndex("dbo.NodeUser", new[] { "NodeID" });
            DropIndex("dbo.NodeFile", new[] { "CusUserID" });
            DropIndex("dbo.NodeFile", new[] { "NodeID" });
            DropIndex("dbo.Flow", new[] { "PID" });
            DropIndex("dbo.Flow", new[] { "NodeID" });
            DropTable("dbo.ProjectStage");
            DropTable("dbo.ProjectStageFile");
            DropTable("dbo.ProjectNodeUser");
            DropTable("dbo.ProjectNodeFile");
            DropTable("dbo.ProjectUser");
            DropTable("dbo.ProjectNode");
            DropTable("dbo.Project");
            DropTable("dbo.ProjectFile");
            DropTable("dbo.NodeUser");
            DropTable("dbo.NodeFile");
            DropTable("dbo.Node");
            DropTable("dbo.Flow");
        }
    }
}
