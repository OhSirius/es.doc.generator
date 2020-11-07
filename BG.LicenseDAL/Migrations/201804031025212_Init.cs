namespace BG.LicenseDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BGAccount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BGLicense",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        AccountId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ChangeDate = c.DateTime(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Access = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BGAccount", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.BGApplication", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.BGLicenseType", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.ApplicationId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.BGApplication",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BGLicenseType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Application_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BGApplication", t => t.Application_Id)
                .Index(t => t.Application_Id);
            
            CreateTable(
                "dbo.BGLogonHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Success = c.Boolean(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Host = c.String(),
                        Comment = c.String(),
                        UserName = c.String(),
                        LogonType = c.Int(),
                        TokenID = c.String(),
                        ApplicationName = c.String(),
                        ApplicationVersion = c.String(),
                        MachineName = c.String(),
                        Login = c.String(),
                        UserDomainName = c.String(),
                        InternalIP = c.String(),
                        HardwareId = c.String(),
                        ProductName = c.String(),
                        CSDBuildNumber = c.String(),
                        CSDVersion = c.String(),
                        CurrentBuild = c.String(),
                        RegisteredOwner = c.String(),
                        ProductId = c.String(),
                        InternalId = c.Int(),
                        LogonApplication = c.Int(),
                        ServerUrl = c.String(),
                        SessionID = c.String(),
                        Region = c.String(),
                        IP = c.String(),
                        PingTime = c.Int(),
                        AccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BGAccount", t => t.AccountId)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BGLogonHistory", "AccountId", "dbo.BGAccount");
            DropForeignKey("dbo.BGLicense", "TypeId", "dbo.BGLicenseType");
            DropForeignKey("dbo.BGLicenseType", "Application_Id", "dbo.BGApplication");
            DropForeignKey("dbo.BGLicense", "ApplicationId", "dbo.BGApplication");
            DropForeignKey("dbo.BGLicense", "AccountId", "dbo.BGAccount");
            DropIndex("dbo.BGLogonHistory", new[] { "AccountId" });
            DropIndex("dbo.BGLicenseType", new[] { "Application_Id" });
            DropIndex("dbo.BGLicense", new[] { "TypeId" });
            DropIndex("dbo.BGLicense", new[] { "ApplicationId" });
            DropIndex("dbo.BGLicense", new[] { "AccountId" });
            DropTable("dbo.BGLogonHistory");
            DropTable("dbo.BGLicenseType");
            DropTable("dbo.BGApplication");
            DropTable("dbo.BGLicense");
            DropTable("dbo.BGAccount");
        }
    }
}
