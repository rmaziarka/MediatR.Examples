namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyTypeDefinitionOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PropertyTypeDefinition", "Order", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PropertyTypeDefinition", "Order");
        }
    }
}
