namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStreetFieldFromAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirement", "AddressId", "dbo.Address");
            AddForeignKey("dbo.Requirement", "AddressId", "dbo.Address", "Id");
            DropColumn("dbo.Address", "Street");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Address", "Street", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Requirement", "AddressId", "dbo.Address");
            AddForeignKey("dbo.Requirement", "AddressId", "dbo.Address", "Id", cascadeDelete: true);
        }
    }
}
