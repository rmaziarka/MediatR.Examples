namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Seed.Common;

    internal static class PropertyTypeData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedPropertyTypes(context);
            SeedPropertyTypeLocalised(context);
            SeedPropertyTypeDefinitions(context);
        }

        private static void SeedPropertyTypeDefinitions(KnightFrankContext context)
        {
            Guid countryId = context.Countries.Where(x => x.IsoCode == "GB").Select(x => x.Id).Single();
            Guid residentialDivisionId = context.EnumTypeItems.Where(x => x.EnumType.Code == "Division" && x.Code == "Residential").Select(x => x.Id).Single();
            Guid commercialDivisionId = context.EnumTypeItems.Where(x => x.EnumType.Code == "Division" && x.Code == "Commercial").Select(x => x.Id).Single();

            short order = 1;

            List<PropertyTypeDefinition> definitions = commercialTypes.SelectMany(GetPropertyTypeHierarchy, (p, c) => new PropertyTypeDefinition
            {
                CountryId = countryId,
                DivisionId = commercialDivisionId,
                PropertyTypeId = c.Id,
                Order = order++
            }).ToList();

            definitions.AddRange(residentialTypes.SelectMany(GetPropertyTypeHierarchy, (p, c) => new PropertyTypeDefinition
            {
                CountryId = countryId,
                DivisionId = residentialDivisionId,
                PropertyTypeId = c.Id,
                Order = order++
            }));

            context.PropertyTypeDefinitions.AddOrUpdate(x => new { x.CountryId, x.PropertyTypeId, x.DivisionId }, definitions.ToArray());
            context.SaveChanges();
        }

        private static void SeedPropertyTypeLocalised(KnightFrankContext context)
        {
            Guid localeId = context.Locales.Where(x => x.IsoCode == LocaleIsoCode.en.ToString()).Select(x => x.Id).Single();

            PropertyTypeLocalised[] propertyTypeLocalised =
                context.PropertyTypes.ToList().Select(
                    x => new PropertyTypeLocalised { LocaleId = localeId, PropertyTypeId = x.Id, Value = x.Code }).ToArray();

            context.PropertyTypeLocaliseds.AddOrUpdate(x => new { x.LocaleId, x.PropertyTypeId }, propertyTypeLocalised);
            context.SaveChanges();
        }

        private static void SeedPropertyTypes(KnightFrankContext context)
        {
            foreach (var type in commercialTypes)
            {
                context.PropertyTypes.Add(type);
            }

            foreach (var type in residentialTypes)
            {
                context.PropertyTypes.Add(type);
            }

            context.SaveChanges();
        }

        private static IEnumerable<PropertyType> GetPropertyTypeHierarchy(PropertyType parent)
        {
            yield return parent;
            if (parent.Children != null)
            {
                foreach (PropertyType child in parent.Children)
                    foreach (PropertyType relative in GetPropertyTypeHierarchy(child))
                        yield return relative;
            }
        }

        private static readonly List<PropertyType> commercialTypes = new List<PropertyType>
        {
            new PropertyType
            {
                Code = "Office"
            },
            new PropertyType
            {
                Code = "Leisure",
                Children = new List<PropertyType>
                {
                    new PropertyType { Code = "Hotel" },
                    new PropertyType { Code = "Leisure" }
                }
            },
            new PropertyType
            {
                Code = "Retail",
                Children = new List<PropertyType>
                {
                    new PropertyType { Code = "Car Showroom" },
                    new PropertyType { Code = "Department Stores" },
                    new PropertyType { Code = "Shopping Centre" },
                    new PropertyType { Code = "Retail Unit A1" },
                    new PropertyType { Code = "Retail Unit A2" },
                    new PropertyType { Code = "Retail Unit A3" },
                    new PropertyType { Code = "Retail Unit A4" },
                    new PropertyType { Code = "Retail Unit A5" }
                }
            },
            new PropertyType
            {
                Code = "Industrial",
                Children = new List<PropertyType>
                {
                    new PropertyType { Code = "Industrial Estate" },
                    new PropertyType { Code = "Industrial/Distribution" }
                }
            },
            new PropertyType
            {
                Code = "Other",
                Children = new List<PropertyType>
                {
                    new PropertyType { Code = "Agricultural" },
                    new PropertyType { Code = "Car Park" },
                    new PropertyType { Code = "Institutional" },
                    new PropertyType { Code = "Mixed Use" }
                }
            }
        };

        private static readonly List<PropertyType> residentialTypes = new List<PropertyType>
        {
            new PropertyType { Code = "House" },
            new PropertyType { Code = "Flat" },
            new PropertyType { Code = "Bungalow" },
            new PropertyType { Code = "Maisonette" },
            new PropertyType { Code = "Studio Flat" },
            new PropertyType { Code = "Development Plot" },
            new PropertyType { Code = "Farm/Estate" },
            new PropertyType { Code = "Garage Only" },
            new PropertyType { Code = "Parking Space" },
            new PropertyType { Code = "Land" },
            new PropertyType { Code = "Houseboat" }
        };
    }
}
