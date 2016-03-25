namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCurentOwnerFromOwnership : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ownership", "CurrentOwner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ownership", "CurrentOwner", c => c.Boolean(nullable: false));
        }
    }
}
