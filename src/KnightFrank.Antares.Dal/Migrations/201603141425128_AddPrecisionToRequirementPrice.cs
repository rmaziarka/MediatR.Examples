namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrecisionToRequirementPrice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requirement", "MinPrice", c => c.Decimal(precision: 19, scale: 4));
            AlterColumn("dbo.Requirement", "MaxPrice", c => c.Decimal(precision: 19, scale: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requirement", "MaxPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Requirement", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
