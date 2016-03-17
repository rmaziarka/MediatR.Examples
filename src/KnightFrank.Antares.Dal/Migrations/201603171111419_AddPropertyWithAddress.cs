namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyWithAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AddressId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Property", "Id", "dbo.Address");
            DropIndex("dbo.Property", new[] { "Id" });
            DropTable("dbo.Property");
        }
    }
}
