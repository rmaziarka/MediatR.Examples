namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OwnershipRequireProperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ownership", "Property_Id", "dbo.Property");
            DropIndex("dbo.Ownership", new[] { "Property_Id" });
            RenameColumn(table: "dbo.Ownership", name: "Property_Id", newName: "PropertyId");
            AlterColumn("dbo.Ownership", "PropertyId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Ownership", "PropertyId");
            AddForeignKey("dbo.Ownership", "PropertyId", "dbo.Property", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ownership", "PropertyId", "dbo.Property");
            DropIndex("dbo.Ownership", new[] { "PropertyId" });
            AlterColumn("dbo.Ownership", "PropertyId", c => c.Guid());
            RenameColumn(table: "dbo.Ownership", name: "PropertyId", newName: "Property_Id");
            CreateIndex("dbo.Ownership", "Property_Id");
            AddForeignKey("dbo.Ownership", "Property_Id", "dbo.Property", "Id");
        }
    }
}
