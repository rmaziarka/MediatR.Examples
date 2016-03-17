namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnSizeToAddressFieldDefinition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddressFieldDefinition", "ColumnSize", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AddressFieldDefinition", "ColumnSize");
        }
    }
}
