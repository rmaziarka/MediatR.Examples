namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PropertyType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ParentId = c.Guid(),
                        Code = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PropertyType", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.PropertyTypeDefinition",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PropertyTypeId = c.Guid(nullable: false),
                        CountryId = c.Guid(nullable: false),
                        DivisionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.EnumTypeItem", t => t.DivisionId)
                .ForeignKey("dbo.PropertyType", t => t.PropertyTypeId)
                .Index(t => t.PropertyTypeId)
                .Index(t => t.CountryId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.PropertyTypeLocalised",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PropertyTypeId = c.Guid(nullable: false),
                        LocaleId = c.Guid(nullable: false),
                        Value = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locale", t => t.LocaleId)
                .ForeignKey("dbo.PropertyType", t => t.PropertyTypeId)
                .Index(t => t.PropertyTypeId)
                .Index(t => t.LocaleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PropertyTypeLocalised", "PropertyTypeId", "dbo.PropertyType");
            DropForeignKey("dbo.PropertyTypeLocalised", "LocaleId", "dbo.Locale");
            DropForeignKey("dbo.PropertyTypeDefinition", "PropertyTypeId", "dbo.PropertyType");
            DropForeignKey("dbo.PropertyTypeDefinition", "DivisionId", "dbo.EnumTypeItem");
            DropForeignKey("dbo.PropertyTypeDefinition", "CountryId", "dbo.Country");
            DropForeignKey("dbo.PropertyType", "ParentId", "dbo.PropertyType");
            DropIndex("dbo.PropertyTypeLocalised", new[] { "LocaleId" });
            DropIndex("dbo.PropertyTypeLocalised", new[] { "PropertyTypeId" });
            DropIndex("dbo.PropertyTypeDefinition", new[] { "DivisionId" });
            DropIndex("dbo.PropertyTypeDefinition", new[] { "CountryId" });
            DropIndex("dbo.PropertyTypeDefinition", new[] { "PropertyTypeId" });
            DropIndex("dbo.PropertyType", new[] { "ParentId" });
            DropTable("dbo.PropertyTypeLocalised");
            DropTable("dbo.PropertyTypeDefinition");
            DropTable("dbo.PropertyType");
        }
    }
}
