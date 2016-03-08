namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEnumTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EnumLocalisation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnumTypeItemId = c.Int(nullable: false),
                        LocalId = c.Int(nullable: false),
                        Value = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnumTypeItem", t => t.EnumTypeItemId, cascadeDelete: true)
                .ForeignKey("dbo.Local", t => t.LocalId, cascadeDelete: true)
                .Index(t => t.EnumTypeItemId)
                .Index(t => t.LocalId);
            
            CreateTable(
                "dbo.EnumTypeItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 40),
                        EnumTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EnumType", t => t.EnumTypeId, cascadeDelete: true)
                .Index(t => t.EnumTypeId);
            
            CreateTable(
                "dbo.EnumType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Local",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsoCode = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnumLocalisation", "LocalId", "dbo.Local");
            DropForeignKey("dbo.EnumLocalisation", "EnumTypeItemId", "dbo.EnumTypeItem");
            DropForeignKey("dbo.EnumTypeItem", "EnumTypeId", "dbo.EnumType");
            DropIndex("dbo.EnumTypeItem", new[] { "EnumTypeId" });
            DropIndex("dbo.EnumLocalisation", new[] { "LocalId" });
            DropIndex("dbo.EnumLocalisation", new[] { "EnumTypeItemId" });
            DropTable("dbo.Local");
            DropTable("dbo.EnumType");
            DropTable("dbo.EnumTypeItem");
            DropTable("dbo.EnumLocalisation");
        }
    }
}
