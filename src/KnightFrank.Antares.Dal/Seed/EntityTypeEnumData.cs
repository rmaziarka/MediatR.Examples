namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    internal static class EntityTypeEnumData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedEnumType(context);
            SeedEnumTypeItem(context);
            SeedEnumLocalisation(context);
        }

        private static void SeedEnumType(KnightFrankContext context)
        {
            var entityTypeEnumType = new EnumType { Code = "EntityType" };

            context.EnumType.AddOrUpdate(x => x.Code, entityTypeEnumType);
            context.SaveChanges();
        }

        private static void SeedEnumTypeItem(KnightFrankContext context)
        {
            Guid entityTypeEnumTypeId = context.EnumType.Where(x => x.Code == "EntityType").Select(x => x.Id).Single();

            var requirementEnumTypeItem = new EnumTypeItem { Code = "Requirement", EnumTypeId = entityTypeEnumTypeId };
            var propertyEnumTypeItem = new EnumTypeItem { Code = "Property", EnumTypeId = entityTypeEnumTypeId };

            context.EnumTypeItem.AddOrUpdate(x => x.Code, requirementEnumTypeItem, propertyEnumTypeItem);
            context.SaveChanges();
        }

        private static void SeedEnumLocalisation(KnightFrankContext context)
        {
            Guid enLocaleId = context.Locale.Where(x => x.IsoCode == "en").Select(x=> x.Id).Single();
            Guid requirementEnumTypeItemId = context.EnumTypeItem.Where(x => x.Code == "Requirement").Select(x => x.Id).Single();
            Guid propertyEnumTypeItemId = context.EnumTypeItem.Where(x => x.Code == "Property").Select(x => x.Id).Single();

            var entityTypeEnumLocalisations = new[]
                                                  {
                                                      new EnumLocalised
                                                          {
                                                              EnumTypeItemId = requirementEnumTypeItemId,
                                                              LocaleId = enLocaleId,
                                                              Value = "Requirement"
                                                          },
                                                      new EnumLocalised
                                                          {
                                                              EnumTypeItemId = propertyEnumTypeItemId,
                                                              LocaleId = enLocaleId,
                                                              Value = "Property"
                                                          }
                                                  };

            context.EnumLocalised.AddOrUpdate(x => x.Value, entityTypeEnumLocalisations.ToArray());
            context.SaveChanges();
        }
    }
}