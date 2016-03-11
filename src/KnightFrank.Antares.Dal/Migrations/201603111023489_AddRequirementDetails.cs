namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirementDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requirement", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Requirement", "MaxPrice", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Requirement", "MinBedrooms", c => c.Int());
            AddColumn("dbo.Requirement", "MaxBedrooms", c => c.Int());
            AddColumn("dbo.Requirement", "MinReceptionRooms", c => c.Int());
            AddColumn("dbo.Requirement", "MaxReceptionRooms", c => c.Int());
            AddColumn("dbo.Requirement", "MinBathrooms", c => c.Int());
            AddColumn("dbo.Requirement", "MaxBathrooms", c => c.Int());
            AddColumn("dbo.Requirement", "MinParkingSpaces", c => c.Int());
            AddColumn("dbo.Requirement", "MaxParkingSpaces", c => c.Int());
            AddColumn("dbo.Requirement", "MinArea", c => c.Double());
            AddColumn("dbo.Requirement", "MaxArea", c => c.Double());
            AddColumn("dbo.Requirement", "MinLandArea", c => c.Double());
            AddColumn("dbo.Requirement", "MaxLandArea", c => c.Double());
            AddColumn("dbo.Requirement", "Description", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requirement", "Description");
            DropColumn("dbo.Requirement", "MaxLandArea");
            DropColumn("dbo.Requirement", "MinLandArea");
            DropColumn("dbo.Requirement", "MaxArea");
            DropColumn("dbo.Requirement", "MinArea");
            DropColumn("dbo.Requirement", "MaxParkingSpaces");
            DropColumn("dbo.Requirement", "MinParkingSpaces");
            DropColumn("dbo.Requirement", "MaxBathrooms");
            DropColumn("dbo.Requirement", "MinBathrooms");
            DropColumn("dbo.Requirement", "MaxReceptionRooms");
            DropColumn("dbo.Requirement", "MinReceptionRooms");
            DropColumn("dbo.Requirement", "MaxBedrooms");
            DropColumn("dbo.Requirement", "MinBedrooms");
            DropColumn("dbo.Requirement", "MaxPrice");
            DropColumn("dbo.Requirement", "MinPrice");
        }
    }
}
