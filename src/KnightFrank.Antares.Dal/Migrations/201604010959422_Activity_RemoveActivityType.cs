namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Activity_RemoveActivityType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activity", "ActivityTypeId", "dbo.EnumTypeItem");
            DropIndex("dbo.Activity", new[] { "ActivityTypeId" });
            DropColumn("dbo.Activity", "ActivityTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activity", "ActivityTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Activity", "ActivityTypeId");
            AddForeignKey("dbo.Activity", "ActivityTypeId", "dbo.EnumTypeItem", "Id");
        }
    }
}
