namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Seed.Common;

    internal sealed class PropertyTypeData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedPropertyTypes(context);
            SeedPropertySubTypes(context);
            SeedPropertyTypeLocalised(context);
            SeedPropertyTypeDefinitions(context);
        }

        private static void SeedPropertyTypeDefinitions(KnightFrankContext context)
        {
            Guid countryId = context.Country.Where(x => x.IsoCode == "GB").Select(x => x.Id).Single();
            Guid residentialDivisionId = context.EnumTypeItem.Where(x => x.EnumType.Code == "Division" && x.Code == "Residential").Select(x => x.Id).Single();
            Guid commercialDivisionId = context.EnumTypeItem.Where(x => x.EnumType.Code == "Division" && x.Code == "Commercial").Select(x => x.Id).Single();

            var commercialTypeCodes = new[] { "Office", "Retail", "Industrial", "Other" };
            short order = 1;
            List<PropertyType> commercialPropertyTypes = context.PropertyType.Where(x => commercialTypeCodes.Contains(x.Code) || x.Parent != null).ToList();
            List<PropertyType> residentialPropertyTypes = context.PropertyType.Where(x => !commercialTypeCodes.Contains(x.Code) && x.Parent == null).ToList();

            List<PropertyTypeDefinition> definitions = commercialPropertyTypes.Select(a => new PropertyTypeDefinition
            {
                CountryId = countryId,
                DivisionId = commercialDivisionId,
                PropertyTypeId = a.Id,
                Order = order++
            }).ToList();

            definitions.AddRange(residentialPropertyTypes.Select(a => new PropertyTypeDefinition
            {
                CountryId = countryId,
                DivisionId = residentialDivisionId,
                PropertyTypeId = a.Id,
                Order = order++
            }));

            context.PropertyTypeDefinition.AddOrUpdate(x => new { x.CountryId, x.PropertyTypeId, x.DivisionId }, definitions.ToArray());
            context.SaveChanges();
        }

        private static void SeedPropertyTypeLocalised(KnightFrankContext context)
        {
            Guid localeId = context.Locale.Where(x => x.IsoCode == LocaleIsoCode.en.ToString()).Select(x => x.Id).Single();

            PropertyTypeLocalised[] propertyTypeLocalised =
                context.PropertyType.ToList().Select(
                    x => new PropertyTypeLocalised { LocaleId = localeId, PropertyTypeId = x.Id, Value = x.Code }).ToArray();

            context.PropertyTypeLocalised.AddOrUpdate(x => new { x.LocaleId, x.PropertyTypeId }, propertyTypeLocalised);
            context.SaveChanges();
        }

        private static void SeedPropertyTypes(KnightFrankContext context)
        {
            var propertyTypes = new[]
            {
                new PropertyType{ Code = "House" },
                new PropertyType{ Code = "Flat" },
                new PropertyType{ Code = "Bungalow" },
                new PropertyType{ Code = "Maisonette" },
                new PropertyType{ Code = "Studio Flat" },
                new PropertyType{ Code = "Development Plot" },
                new PropertyType{ Code = "Farm/Estate" },
                new PropertyType{ Code = "Garage Only" },
                new PropertyType{ Code = "Parking Space" },
                new PropertyType{ Code = "Land" },
                new PropertyType { Code = "Houseboat" },

                new PropertyType { Code = "Office" },
                new PropertyType { Code = "Retail" },
                new PropertyType { Code = "Industrial" },
                new PropertyType { Code = "Other" }
            };

            context.PropertyType.AddOrUpdate(x => x.Code, propertyTypes);
            context.SaveChanges();
        }

        private static void SeedPropertySubTypes(KnightFrankContext context)
        {
            Guid officeId = context.PropertyType.Where(x => x.Code == "Office").Select(x => x.Id).Single();
            Guid retailId = context.PropertyType.Where(x => x.Code == "Retail").Select(x => x.Id).Single();
            Guid industrialId = context.PropertyType.Where(x => x.Code == "Industrial").Select(x => x.Id).Single();
            Guid otherId = context.PropertyType.Where(x => x.Code == "Other").Select(x => x.Id).Single();

            var commercialPropertySubTypes = new[]
            {
                new PropertyType { Code = "Hotel", ParentId  = officeId },
                new PropertyType { Code = "Leisure", ParentId  = officeId },
                new PropertyType { Code = "Car Showroom", ParentId  = retailId },
                new PropertyType { Code = "Department Stores", ParentId  = retailId },
                new PropertyType { Code = "Shopping Centre", ParentId  = retailId },
                new PropertyType { Code = "Retails Unit A1", ParentId  = retailId },
                new PropertyType { Code = "Retails Unit A2", ParentId  = retailId },
                new PropertyType { Code = "Retails Unit A3", ParentId  = retailId },
                new PropertyType { Code = "Retails Unit A4", ParentId  = retailId },
                new PropertyType { Code = "Retails Unit A5", ParentId  = retailId },
                new PropertyType { Code = "Industrial Estate", ParentId  = industrialId },
                new PropertyType { Code = "Industrial/Distribution", ParentId  = industrialId },
                new PropertyType { Code = "Agricultural", ParentId  = otherId },
                new PropertyType { Code = "Car Park", ParentId  = otherId },
                new PropertyType { Code = "Institutional", ParentId  = otherId },
                new PropertyType { Code = "Mixed Use", ParentId  = otherId },
            };

            context.PropertyType.AddOrUpdate(x => x.Code, commercialPropertySubTypes);
            context.SaveChanges();
        }
    }
}
