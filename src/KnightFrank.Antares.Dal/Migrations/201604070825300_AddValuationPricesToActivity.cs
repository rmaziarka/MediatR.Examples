namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddValuationPricesToActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "MarketAppraisalPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Activity", "RecommendedPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Activity", "VendorEstimatedPrice", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activity", "VendorEstimatedPrice");
            DropColumn("dbo.Activity", "RecommendedPrice");
            DropColumn("dbo.Activity", "MarketAppraisalPrice");
        }
    }
}
