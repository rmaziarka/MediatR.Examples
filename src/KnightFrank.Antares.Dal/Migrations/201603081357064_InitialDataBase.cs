namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(),
                        Surname = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requirement",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnumLocalisation",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EnumTypeItemId = c.Guid(nullable: false),
                        LocalId = c.Guid(nullable: false),
                        Value = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnumTypeItem", t => t.EnumTypeItemId, cascadeDelete: true)
                .ForeignKey("dbo.Local", t => t.LocalId, cascadeDelete: true)
                .Index(t => t.EnumTypeItemId)
                .Index(t => t.LocalId);
            
            CreateTable(
                "dbo.EnumTypeItem",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 40),
                        EnumTypeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnumType", t => t.EnumTypeId, cascadeDelete: true)
                .Index(t => t.EnumTypeId);
            
            CreateTable(
                "dbo.EnumType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Local",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        IsoCode = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequirementContact",
                c => new
                    {
                        RequirementId = c.Guid(nullable: false),
                        ContactId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequirementId, t.ContactId })
                .ForeignKey("dbo.Requirement", t => t.RequirementId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .Index(t => t.RequirementId)
                .Index(t => t.ContactId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnumLocalisation", "LocalId", "dbo.Local");
            DropForeignKey("dbo.EnumLocalisation", "EnumTypeItemId", "dbo.EnumTypeItem");
            DropForeignKey("dbo.EnumTypeItem", "EnumTypeId", "dbo.EnumType");
            DropForeignKey("dbo.RequirementContact", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.RequirementContact", "RequirementId", "dbo.Requirement");
            DropIndex("dbo.RequirementContact", new[] { "ContactId" });
            DropIndex("dbo.RequirementContact", new[] { "RequirementId" });
            DropIndex("dbo.EnumTypeItem", new[] { "EnumTypeId" });
            DropIndex("dbo.EnumLocalisation", new[] { "LocalId" });
            DropIndex("dbo.EnumLocalisation", new[] { "EnumTypeItemId" });
            DropTable("dbo.RequirementContact");
            DropTable("dbo.Local");
            DropTable("dbo.EnumType");
            DropTable("dbo.EnumTypeItem");
            DropTable("dbo.EnumLocalisation");
            DropTable("dbo.Requirement");
            DropTable("dbo.Contact");
        }
    }
}
