namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyTypeIdToProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Property", "PropertyTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Property", "PropertyTypeId");
            AddForeignKey("dbo.Property", "PropertyTypeId", "dbo.PropertyType", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Property", "PropertyTypeId", "dbo.PropertyType");
            DropIndex("dbo.Property", new[] { "PropertyTypeId" });
            DropColumn("dbo.Property", "PropertyTypeId");
        }
    }
}
