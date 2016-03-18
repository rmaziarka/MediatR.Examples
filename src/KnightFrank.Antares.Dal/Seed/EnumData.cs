namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Seed.Common;

    internal static class EnumData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedEnum(
                context,
                "EntityType",
                new Dictionary<string, Dictionary<string, string>>
                {
                    ["Property"] =
                            new Dictionary<string, string>
                            {
                                [LocaleIsoCode.en.ToString()] = "Property"
                            },
                    ["Requirement"] =
                            new Dictionary<string, string>
                            {
                                [LocaleIsoCode.en.ToString()] = "Requirement"
                            }
                });
        }

        private static void SeedEnum(KnightFrankContext context, string enumType, Dictionary<string, Dictionary<string, string>> enumTypeItems)
        {
            SeedEnumType(context, enumType);

            foreach (KeyValuePair<string, Dictionary<string, string>> enumTypeItem in enumTypeItems)
            {
                SeedEnumTypeItem(context, enumType, enumTypeItem.Key);

                foreach (KeyValuePair<string, string> enumLocalisation in enumTypeItem.Value)
                {
                    SeedEnumLocalisation(context, enumLocalisation.Key, enumTypeItem.Key, enumLocalisation.Value);
                }
            }
        }

        private static void SeedEnumType(KnightFrankContext context, string enumTypeCode)
        {
            var enumType = new EnumType { Code = enumTypeCode };

            context.EnumType.AddOrUpdate(x => x.Code, enumType);
            context.SaveChanges();
        }

        private static void SeedEnumTypeItem(KnightFrankContext context, string enumType, string enumTypeItemCode)
        {
            Guid enumTypeId = context.EnumType.Where(x => x.Code == enumType).Select(x => x.Id).Single();

            var enumTypeItem = new EnumTypeItem { Code = enumTypeItemCode, EnumTypeId = enumTypeId };

            context.EnumTypeItem.AddOrUpdate(x => x.Code, enumTypeItem);
            context.SaveChanges();
        }

        private static void SeedEnumLocalisation(KnightFrankContext context, string locale, string enumTypeItemCode, string enumLocalisedValue)
        {
            Guid localeId = context.Locale.Where(x => x.IsoCode == locale).Select(x => x.Id).Single();
            Guid enumTypeItemId = context.EnumTypeItem.Where(x => x.Code == enumTypeItemCode).Select(x => x.Id).Single();

            var enumLocalised = new EnumLocalised { EnumTypeItemId = enumTypeItemId, LocaleId = localeId, Value = enumLocalisedValue };

            context.EnumLocalised.AddOrUpdate(x => x.Value, enumLocalised);
            context.SaveChanges();
        }
    }
}