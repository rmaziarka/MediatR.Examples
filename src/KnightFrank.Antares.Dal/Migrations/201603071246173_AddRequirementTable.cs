namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddRequirementTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requirement",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CreateDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RequirementContact",
                c => new
                {
                    Requirement_Id = c.Int(nullable: false),
                    Contact_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Requirement_Id, t.Contact_Id })
                .ForeignKey("dbo.Requirement", t => t.Requirement_Id, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.Contact_Id, cascadeDelete: true)
                .Index(t => t.Requirement_Id)
                .Index(t => t.Contact_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.RequirementContact", "Contact_Id", "dbo.Contact");
            DropForeignKey("dbo.RequirementContact", "Requirement_Id", "dbo.Requirement");
            DropIndex("dbo.RequirementContact", new[] { "Contact_Id" });
            DropIndex("dbo.RequirementContact", new[] { "Requirement_Id" });
            DropTable("dbo.RequirementContact");
            DropTable("dbo.Requirement");
        }
    }
}
