namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using KnightFrank.Antares.Dal.Model;

    internal static class AddressFormData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedAddressField(context);
            SeedAddressFieldLabel(context);
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
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
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
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    RegEx = "[XYZ]",
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.PROPERTYNAME"),
                    Required = true,
                    RowOrder = 1,
                    ColumnOrder = 2,
                    ColumnSize = 3
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.PROPERTYNUMBER"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 1,
                    ColumnOrder = 1,
                    ColumnSize = 3
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.ADDRESSLINE1"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 2,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine2"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.ADDRESSLINE2"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 3,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine3"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.ADDRESSLINE3"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 4,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.POSTCODE"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 5,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "City"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.CITY"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 6,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.COUNTY"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 7,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = GetAddressFormIdByCountryCode(context, "GB"),
                    AddressFieldId = GetAddressFieldIdByName(context, "Country"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.COUNTRY"),
                    RegEx = "[ABC]",
                    Required = true,
                    RowOrder = 8,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
            };

            context.AddressFieldDefinition.AddOrUpdate(af => new { af.AddressFieldId, af.AddressFormId, af.AddressFieldLabelId }, addressFieldDefinitions.ToArray());
            context.SaveChanges();
        }

        private static void SeedAddressForm(KnightFrankContext context)
        {
            var addressForm = new AddressForm
            {
                Id = GetAddressFormIdByCountryIsoCode(context, "GB"),
                CountryId = GetCountryIdByCode(context, "GB")
            };

            context.AddressForm.AddOrUpdate(addressForm);
            context.SaveChanges();
        }

        private static Guid GetAddressFormIdByCountryIsoCode(KnightFrankContext context, string countryIsoCode)
        {
            AddressForm addressForm = context.AddressForm.FirstOrDefault(af => af.Country.IsoCode == countryIsoCode);

            return addressForm?.Id ?? default(Guid);
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
                    LabelKey = "ADDRESSFORM.POSTCODE"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    LabelKey = "ADDRESSFORM.POSTALCODE"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    LabelKey = "ADDRESSFORM.PINCODE"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Country"),
                    LabelKey = "ADDRESSFORM.COUNTRY"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    LabelKey = "ADDRESSFORM.COUNTY"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    LabelKey = "ADDRESSFORM.STATE"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    LabelKey = "ADDRESSFORM.PROVINCE"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "City"),
                    LabelKey = "ADDRESSFORM.CITY"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "ADDRESSFORM.ADDRESSLINE1"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "ADDRESSFORM.STREET"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "ADDRESSFORM.ADDRESS"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine1"),
                    LabelKey = "ADDRESSFORM.STREETNUMBER"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine2"),
                    LabelKey = "ADDRESSFORM.ADDRESSLINE2"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine2"),
                    LabelKey = "ADDRESSFORM.STREETNAME"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "AddressLine3"),
                    LabelKey = "ADDRESSFORM.ADDRESSLINE3"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    LabelKey = "ADDRESSFORM.PROPERTYNAME"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    LabelKey = "ADDRESSFORM.BUILDINGNAME"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    LabelKey = "ADDRESSFORM.PROPERTYNUMBER"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    LabelKey = "ADDRESSFORM.FLATNUMBER"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    LabelKey = "ADDRESSFORM.UNIT"
                },
            };

            context.AddressFieldLabel.AddOrUpdate(x => new { x.AddressFieldId }, input.ToArray());
            context.SaveChanges();
        }

        private static Guid GetCountryIdByCode(KnightFrankContext context, string countryCode)
        {
            Country country = context.Country.FirstOrDefault(c => c.IsoCode == countryCode);

            return country?.Id ?? default(Guid);
        }

        private static Guid GetAddressFormIdByCountryCode(KnightFrankContext context, string countryCode)
        {
            AddressForm addressForm = context.AddressForm.FirstOrDefault(af => af.Country.IsoCode == countryCode);

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