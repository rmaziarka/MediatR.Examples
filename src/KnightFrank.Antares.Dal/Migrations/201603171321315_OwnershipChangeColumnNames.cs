namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnershipChangeColumnNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ownership", "PurchaseDate", c => c.DateTime());
            AddColumn("dbo.Ownership", "SellDate", c => c.DateTime());
            AddColumn("dbo.Ownership", "BuyPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Ownership", "SellPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Ownership", "CurrentOwner", c => c.Boolean(nullable: false));
            DropColumn("dbo.Ownership", "PurchasingDate");
            DropColumn("dbo.Ownership", "SellingDate");
            DropColumn("dbo.Ownership", "BuyingPrice");
            DropColumn("dbo.Ownership", "SellingPrice");
            DropColumn("dbo.Ownership", "IsCurrent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ownership", "IsCurrent", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ownership", "SellingPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Ownership", "BuyingPrice", c => c.Decimal(precision: 19, scale: 4));
            AddColumn("dbo.Ownership", "SellingDate", c => c.DateTime());
            AddColumn("dbo.Ownership", "PurchasingDate", c => c.DateTime());
            DropColumn("dbo.Ownership", "CurrentOwner");
            DropColumn("dbo.Ownership", "SellPrice");
            DropColumn("dbo.Ownership", "BuyPrice");
            DropColumn("dbo.Ownership", "SellDate");
            DropColumn("dbo.Ownership", "PurchaseDate");
        }
    }
}
