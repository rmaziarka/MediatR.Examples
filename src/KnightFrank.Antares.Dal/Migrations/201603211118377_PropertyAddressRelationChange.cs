namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyAddressRelationChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Property", "Id", "dbo.Address");
            DropIndex("dbo.Property", new[] { "Id" });
            AlterColumn("dbo.Property", "AddressId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Property", "AddressId");
            AddForeignKey("dbo.Property", "AddressId", "dbo.Address", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Property", "AddressId", "dbo.Address");
            DropIndex("dbo.Property", new[] { "AddressId" });
            AlterColumn("dbo.Property", "AddressId", c => c.Guid(nullable: false, identity: true));
            CreateIndex("dbo.Property", "Id");
            AddForeignKey("dbo.Property", "Id", "dbo.Address", "Id");
        }
    }
}
