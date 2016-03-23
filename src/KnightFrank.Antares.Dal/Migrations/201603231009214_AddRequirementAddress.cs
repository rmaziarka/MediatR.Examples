namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirementAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "Street", c => c.String(maxLength: 128));
            AddColumn("dbo.Requirement", "AddressId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Requirement", "AddressId");
            AddForeignKey("dbo.Requirement", "AddressId", "dbo.Address", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requirement", "AddressId", "dbo.Address");
            DropIndex("dbo.Requirement", new[] { "AddressId" });
            DropColumn("dbo.Requirement", "AddressId");
            DropColumn("dbo.Address", "Street");
        }
    }
}
