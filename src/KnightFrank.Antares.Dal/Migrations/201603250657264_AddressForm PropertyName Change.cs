namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressFormPropertyNameChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Address", "PropertyName", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Address", "PropertyName", c => c.String(maxLength: 28));
        }
    }
}
