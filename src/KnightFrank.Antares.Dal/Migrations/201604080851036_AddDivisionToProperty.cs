namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDivisionToProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Property", "DivisionId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Property", "DivisionId");
            AddForeignKey("dbo.Property", "DivisionId", "dbo.EnumTypeItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Property", "DivisionId", "dbo.EnumTypeItem");
            DropIndex("dbo.Property", new[] { "DivisionId" });
            DropColumn("dbo.Property", "DivisionId");
        }
    }
}
