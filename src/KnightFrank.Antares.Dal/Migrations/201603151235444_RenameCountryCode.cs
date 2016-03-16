namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class RenameCountryCode : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Country", "Code", "IsoCode");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Country", "IsoCode", "Code");
        }
    }
}
