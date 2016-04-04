namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddActivity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastModifiedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Activity");
        }
    }
}
