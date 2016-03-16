namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountriesAndLocales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CountryLocalised",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CountryId = c.Guid(nullable: false),
                        LocaleId = c.Guid(nullable: false),
                        Value = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Locale", t => t.LocaleId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.LocaleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CountryLocalised", "LocaleId", "dbo.Locale");
            DropForeignKey("dbo.CountryLocalised", "CountryId", "dbo.Country");
            DropIndex("dbo.CountryLocalised", new[] { "LocaleId" });
            DropIndex("dbo.CountryLocalised", new[] { "CountryId" });
            DropTable("dbo.CountryLocalised");
        }
    }
}
