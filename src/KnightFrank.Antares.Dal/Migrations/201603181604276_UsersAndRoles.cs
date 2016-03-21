namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersAndRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ActiveDirectoryDomain = c.String(maxLength: 40),
                        ActiveDirectoryLogin = c.String(maxLength: 100),
                        FirstName = c.String(maxLength: 40),
                        LastName = c.String(maxLength: 40),
                        BusinessId = c.Guid(nullable: false),
                        CountryId = c.Guid(nullable: false),
                        DepartmentId = c.Guid(nullable: false),
                        LocaleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Department", t => t.DepartmentId)
                .ForeignKey("dbo.Business", t => t.BusinessId)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.Locale", t => t.LocaleId)
                .Index(t => t.BusinessId)
                .Index(t => t.CountryId)
                .Index(t => t.DepartmentId)
                .Index(t => t.LocaleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleUser",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Role", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "LocaleId", "dbo.Locale");
            DropForeignKey("dbo.User", "CountryId", "dbo.Country");
            DropForeignKey("dbo.User", "BusinessId", "dbo.Business");
            DropForeignKey("dbo.RoleUser", "User_Id", "dbo.User");
            DropForeignKey("dbo.RoleUser", "Role_Id", "dbo.Role");
            DropForeignKey("dbo.User", "DepartmentId", "dbo.Department");
            DropIndex("dbo.RoleUser", new[] { "User_Id" });
            DropIndex("dbo.RoleUser", new[] { "Role_Id" });
            DropIndex("dbo.User", new[] { "LocaleId" });
            DropIndex("dbo.User", new[] { "DepartmentId" });
            DropIndex("dbo.User", new[] { "CountryId" });
            DropIndex("dbo.User", new[] { "BusinessId" });
            DropTable("dbo.RoleUser");
            DropTable("dbo.Role");
            DropTable("dbo.User");
        }
    }
}
