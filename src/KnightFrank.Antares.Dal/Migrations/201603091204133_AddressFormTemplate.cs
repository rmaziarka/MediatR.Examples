namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressFormTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CountryId = c.Guid(nullable: false),
                        AddressFormId = c.Guid(nullable: false),
                        PropertyName = c.String(maxLength: 28),
                        PropertyNumber = c.String(maxLength: 8),
                        Line1 = c.String(maxLength: 128),
                        Line2 = c.String(maxLength: 128),
                        Line3 = c.String(maxLength: 128),
                        Postcode = c.String(maxLength: 10),
                        City = c.String(maxLength: 128),
                        County = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .ForeignKey("dbo.AddressForm", t => t.AddressFormId, cascadeDelete: true)
                .Index(t => t.CountryId)
                .Index(t => t.AddressFormId);
            
            CreateTable(
                "dbo.AddressForm",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CountryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.AddressFieldDefinition",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AddressFieldId = c.Guid(nullable: false),
                        AddressFieldLabelId = c.Guid(nullable: false),
                        AddressFormId = c.Guid(nullable: false),
                        Required = c.Boolean(nullable: false),
                        RegEx = c.String(maxLength: 50),
                        RowOrder = c.Short(nullable: false),
                        ColumnOrder = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressField", t => t.AddressFieldId)
                .ForeignKey("dbo.AddressFieldLabel", t => t.AddressFieldLabelId)
                .ForeignKey("dbo.AddressForm", t => t.AddressFormId)
                .Index(t => t.AddressFieldId)
                .Index(t => t.AddressFieldLabelId)
                .Index(t => t.AddressFormId);
            
            CreateTable(
                "dbo.AddressField",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AddressFieldLabel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AddressFieldId = c.Guid(nullable: false),
                        LabelKey = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressField", t => t.AddressFieldId)
                .Index(t => t.AddressFieldId);
            
            CreateTable(
                "dbo.AddressFormEntityType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AddressFormId = c.Guid(nullable: false),
                        EnumTypeItemId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressForm", t => t.AddressFormId, cascadeDelete: true)
                .ForeignKey("dbo.EnumTypeItem", t => t.EnumTypeItemId, cascadeDelete: true)
                .Index(t => t.AddressFormId)
                .Index(t => t.EnumTypeItemId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Address", "AddressFormId", "dbo.AddressForm");
            DropForeignKey("dbo.AddressForm", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Address", "CountryId", "dbo.Country");
            DropForeignKey("dbo.AddressFormEntityType", "EnumTypeItemId", "dbo.EnumTypeItem");
            DropForeignKey("dbo.AddressFormEntityType", "AddressFormId", "dbo.AddressForm");
            DropForeignKey("dbo.AddressFieldDefinition", "AddressFormId", "dbo.AddressForm");
            DropForeignKey("dbo.AddressFieldLabel", "AddressFieldId", "dbo.AddressField");
            DropForeignKey("dbo.AddressFieldDefinition", "AddressFieldLabelId", "dbo.AddressFieldLabel");
            DropForeignKey("dbo.AddressFieldDefinition", "AddressFieldId", "dbo.AddressField");
            DropIndex("dbo.AddressFormEntityType", new[] { "EnumTypeItemId" });
            DropIndex("dbo.AddressFormEntityType", new[] { "AddressFormId" });
            DropIndex("dbo.AddressFieldLabel", new[] { "AddressFieldId" });
            DropIndex("dbo.AddressFieldDefinition", new[] { "AddressFormId" });
            DropIndex("dbo.AddressFieldDefinition", new[] { "AddressFieldLabelId" });
            DropIndex("dbo.AddressFieldDefinition", new[] { "AddressFieldId" });
            DropIndex("dbo.AddressForm", new[] { "CountryId" });
            DropIndex("dbo.Address", new[] { "AddressFormId" });
            DropIndex("dbo.Address", new[] { "CountryId" });
            DropTable("dbo.Country");
            DropTable("dbo.AddressFormEntityType");
            DropTable("dbo.AddressFieldLabel");
            DropTable("dbo.AddressField");
            DropTable("dbo.AddressFieldDefinition");
            DropTable("dbo.AddressForm");
            DropTable("dbo.Address");
        }
    }
}
