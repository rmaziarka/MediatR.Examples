namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    internal static class AddressFormMetadata
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedAddressField(context);
            SeedAddressFieldLabel(context);
            SeedCountry(context);
            SeedAddressForm(context);
            SeedAddressFieldDefinition(context);
            SeedAddressFormEntityTypeItem(context);
        }

        private static void SeedAddressFormEntityTypeItem(KnightFrankContext context)
        {
            var addressFormEntityTypes = new List<AddressFormEntityType>
            {
                new AddressFormEntityType
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    EnumTypeItemId = GetEntityTypeItemIdByCode(context, "Property")
                }
            };

            context.AddressFormEntityType.AddOrUpdate(x => x.AddressFormId, addressFormEntityTypes.ToArray());
            context.SaveChanges();
        }

        private static Guid GetEntityTypeItemIdByCode(KnightFrankContext context, string propertyCode)
        {
            return context.EnumTypeItem.FirstOrDefault(eti => eti.Code == propertyCode)?.Id ?? default(Guid);
        }

        private static void SeedAddressFieldDefinition(KnightFrankContext context)
        {
            var addressFieldDefinitions = new List<AddressFieldDefinition>
            {
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    RegEx = "[XYZ]",
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{PropertyName}"),
                    Required = true,
                    RowOrder = 1,
                    ColumnOrder = 2
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{PropertyNumber}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 1,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{AddressLine1}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 2,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine2"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{AddressLine2}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 3,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine3"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{AddressLine3}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 4,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{Postcode}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 5,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "City"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{City}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 6,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{County}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 7,
                    ColumnOrder = 1
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "UK"),
                    AddressFieldId = GetAddressFieldIdByName(context, "Country"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "{Country}"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 8,
                    ColumnOrder = 1
                },
            };

            context.AddressFieldDefinition.AddOrUpdate(af => new { af.AddressFieldId, af.AddressFormId, af.AddressFieldLabelId }, addressFieldDefinitions.ToArray());
            context.SaveChanges();
        }

        private static void SeedCountry(KnightFrankContext context)
        {
            var countries = new List<Country>
            {
                new Country
                {
                    Id = GetCountryIdByCode(context, "UK"),
                    Code = "UK"
                }
            };

            context.Country.AddOrUpdate(x => x.Code, countries.ToArray());
            context.SaveChanges();
        }

        private static void SeedAddressForm(KnightFrankContext context)
        {
            var addressForm = new AddressForm
            {
                CountryId = GetCountryIdByCode(context, "UK")
            };

            context.AddressForm.AddOrUpdate(addressForm);
            context.SaveChanges();
        }

        private static void SeedAddressField(KnightFrankContext context)
        {
            var input = new List<AddressField>
            {
                new AddressField
                {
                    Name = "PropertyName"
                },
                new AddressField
                {
                    Name = "PropertyNumber"
                },
                new AddressField
                {
                    Name = "AddressLine1"
                },
                new AddressField
                {
                    Name = "AddressLine2"
                },
                new AddressField
                {
                    Name = "AddressLine3"
                },
                new AddressField
                {
                    Name = "Postcode"
                },
                new AddressField
                {
                    Name = "City"
                },
                new AddressField
                {
                    Name = "County"
                },
                new AddressField
                {
                    Name = "Country"
                }
            };
            context.AddressField.AddOrUpdate(x => x.Name, input.ToArray());
            context.SaveChanges();
        }

        private static void SeedAddressFieldLabel(KnightFrankContext context)
        {
            var input = new List<AddressFieldLabel>
            {
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    LabelKey = "{Postcode}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    LabelKey = "{Postalcode}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    LabelKey = "{Pincode}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Country"),
                    LabelKey = "{Country}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    LabelKey = "{County}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    LabelKey = "{State}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    LabelKey = "{Province}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "City"),
                    LabelKey = "{City}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "{AddressLine1}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "{Street}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "{Address}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "{StreetNumber}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine2"),
                    LabelKey = "{AddressLine2}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine2"),
                    LabelKey = "{StreetName}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine3"),
                    LabelKey = "{AddressLine3}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    LabelKey = "{PropertyName}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    LabelKey = "{BuildingName}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    LabelKey = "{PropertyNumber}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    LabelKey = "{FlatNumber}"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    LabelKey = "{Unit}"
                },
            };

            context.AddressFieldLabel.AddOrUpdate(x => new { x.LabelKey, x.AddressFieldId }, input.ToArray());
            context.SaveChanges();
        }

        private static Guid GetCountryIdByCode(KnightFrankContext context, string countryCode)
        {
            Country country = context.Country.FirstOrDefault(c => c.Code == countryCode);

            return country?.Id ?? default(Guid);
        }

        private static Guid GetAddressFormIdByCountryCode(KnightFrankContext context, string countryCode)
        {
            AddressForm addressForm = context.AddressForm.FirstOrDefault(af => af.Country.Code == countryCode);

            return addressForm?.Id ?? default(Guid);
        }

        private static Guid GetAddressFieldIdByName(KnightFrankContext context, string addressFieldName)
        {
            AddressField addressField = context.AddressField.FirstOrDefault(af => addressFieldName.Equals(af.Name));

            return addressField?.Id ?? default(Guid);
        }

        private static Guid GetAddressFieldLabelIdByLabelKey(KnightFrankContext context, string key)
        {
            AddressFieldLabel result = context.AddressFieldLabel.FirstOrDefault(l => l.LabelKey == key);

            return result?.Id ?? default(Guid);
        }
    }
}