namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwnership : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ownership",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PurchasingDate = c.DateTime(),
                        SellingDate = c.DateTime(),
                        BuyingPrice = c.Decimal(precision: 19, scale: 4),
                        SellingPrice = c.Decimal(precision: 19, scale: 4),
                        IsCurrent = c.Boolean(nullable: false),
                        OwnershipTypeId = c.Guid(nullable: false),
                        Property_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnumTypeItem", t => t.OwnershipTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Property", t => t.Property_Id)
                .Index(t => t.OwnershipTypeId)
                .Index(t => t.Property_Id);
            
            CreateTable(
                "dbo.OwnershipContact",
                c => new
                    {
                        OwnershipId = c.Guid(nullable: false),
                        ContactId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OwnershipId, t.ContactId })
                .ForeignKey("dbo.Ownership", t => t.OwnershipId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .Index(t => t.OwnershipId)
                .Index(t => t.ContactId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ownership", "Property_Id", "dbo.Property");
            DropForeignKey("dbo.Ownership", "OwnershipTypeId", "dbo.EnumTypeItem");
            DropForeignKey("dbo.OwnershipContact", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.OwnershipContact", "OwnershipId", "dbo.Ownership");
            DropIndex("dbo.OwnershipContact", new[] { "ContactId" });
            DropIndex("dbo.OwnershipContact", new[] { "OwnershipId" });
            DropIndex("dbo.Ownership", new[] { "Property_Id" });
            DropIndex("dbo.Ownership", new[] { "OwnershipTypeId" });
            DropTable("dbo.OwnershipContact");
            DropTable("dbo.Ownership");
        }
    }
}
