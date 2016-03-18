namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BusinessesAndDepartments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EnumLocalised", "LocaleId", "dbo.Locale");
            DropForeignKey("dbo.CountryLocalised", "LocaleId", "dbo.Locale");
            CreateTable(
                "dbo.Business",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CountryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        CountryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            AlterColumn("dbo.Locale", "IsoCode", c => c.String(maxLength: 2));
            AlterColumn("dbo.Country", "IsoCode", c => c.String(maxLength: 2));
            AddForeignKey("dbo.EnumLocalised", "LocaleId", "dbo.Locale", "Id");
            AddForeignKey("dbo.CountryLocalised", "LocaleId", "dbo.Locale", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CountryLocalised", "LocaleId", "dbo.Locale");
            DropForeignKey("dbo.EnumLocalised", "LocaleId", "dbo.Locale");
            DropForeignKey("dbo.Department", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Business", "CountryId", "dbo.Country");
            DropIndex("dbo.Department", new[] { "CountryId" });
            DropIndex("dbo.Business", new[] { "CountryId" });
            AlterColumn("dbo.Country", "IsoCode", c => c.String(maxLength: 100));
            AlterColumn("dbo.Locale", "IsoCode", c => c.String(maxLength: 40));
            DropTable("dbo.Department");
            DropTable("dbo.Business");
            AddForeignKey("dbo.CountryLocalised", "LocaleId", "dbo.Locale", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EnumLocalised", "LocaleId", "dbo.Locale", "Id", cascadeDelete: true);
        }
    }
}
