namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyContact",
                c => new
                    {
                        CompanyId = c.Guid(nullable: false),
                        ContactId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CompanyId, t.ContactId })
                .ForeignKey("dbo.Company", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.ContactId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyContact", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.CompanyContact", "CompanyId", "dbo.Company");
            DropIndex("dbo.CompanyContact", new[] { "ContactId" });
            DropIndex("dbo.CompanyContact", new[] { "CompanyId" });
            DropTable("dbo.CompanyContact");
            DropTable("dbo.Company");
        }
    }
}
