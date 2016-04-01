namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityContacts : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Activity");
            CreateTable(
                "dbo.ActivityContact",
                c => new
                    {
                        ActivityId = c.Guid(nullable: false),
                        ContactId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ActivityId, t.ContactId })
                .ForeignKey("dbo.Activity", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Contact", t => t.ContactId, cascadeDelete: true)
                .Index(t => t.ActivityId)
                .Index(t => t.ContactId);
            
            AddColumn("dbo.Activity", "PropertyId", c => c.Guid(nullable: false));
            AddColumn("dbo.Activity", "ActivityTypeId", c => c.Guid(nullable: false));
            AddColumn("dbo.Activity", "ActivityStatusId", c => c.Guid(nullable: false));
            AddColumn("dbo.Activity", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Activity", "LastModifiedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Activity", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Activity", "Id");
            CreateIndex("dbo.Activity", "PropertyId");
            CreateIndex("dbo.Activity", "ActivityTypeId");
            CreateIndex("dbo.Activity", "ActivityStatusId");
            AddForeignKey("dbo.Activity", "ActivityStatusId", "dbo.EnumTypeItem", "Id");
            AddForeignKey("dbo.Activity", "ActivityTypeId", "dbo.EnumTypeItem", "Id");
            AddForeignKey("dbo.Activity", "PropertyId", "dbo.Property", "Id");
            DropColumn("dbo.Activity", "CreatedAt");
            DropColumn("dbo.Activity", "LastModifiedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activity", "LastModifiedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Activity", "CreatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropForeignKey("dbo.Activity", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.ActivityContact", "ContactId", "dbo.Contact");
            DropForeignKey("dbo.ActivityContact", "ActivityId", "dbo.Activity");
            DropForeignKey("dbo.Activity", "ActivityTypeId", "dbo.EnumTypeItem");
            DropForeignKey("dbo.Activity", "ActivityStatusId", "dbo.EnumTypeItem");
            DropIndex("dbo.ActivityContact", new[] { "ContactId" });
            DropIndex("dbo.ActivityContact", new[] { "ActivityId" });
            DropIndex("dbo.Activity", new[] { "ActivityStatusId" });
            DropIndex("dbo.Activity", new[] { "ActivityTypeId" });
            DropIndex("dbo.Activity", new[] { "PropertyId" });
            DropPrimaryKey("dbo.Activity");
            AlterColumn("dbo.Activity", "Id", c => c.Guid(nullable: false));
            DropColumn("dbo.Activity", "LastModifiedDate");
            DropColumn("dbo.Activity", "CreatedDate");
            DropColumn("dbo.Activity", "ActivityStatusId");
            DropColumn("dbo.Activity", "ActivityTypeId");
            DropColumn("dbo.Activity", "PropertyId");
            DropTable("dbo.ActivityContact");
            AddPrimaryKey("dbo.Activity", "Id");
        }
    }
}
