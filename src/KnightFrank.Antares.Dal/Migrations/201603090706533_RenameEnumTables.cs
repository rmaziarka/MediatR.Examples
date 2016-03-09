namespace KnightFrank.Antares.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEnumTables : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Local", newName: "Locale");
            RenameTable(name: "dbo.EnumLocalisation", newName: "EnumLocalised");
            RenameColumn(table: "dbo.EnumLocalised", name: "LocalId", newName: "LocaleId");
            RenameIndex(table: "dbo.EnumLocalised", name: "IX_LocalId", newName: "IX_LocaleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.EnumLocalised", name: "IX_LocaleId", newName: "IX_LocalId");
            RenameColumn(table: "dbo.EnumLocalised", name: "LocaleId", newName: "LocalId");
            RenameTable(name: "dbo.EnumLocalised", newName: "EnumLocalisation");
            RenameTable(name: "dbo.Locale", newName: "Local");
        }
    }
}
