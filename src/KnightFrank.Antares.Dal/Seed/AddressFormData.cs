namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;

    internal static class AddressFormData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedAddressField(context);
            SeedAddressFieldLabel(context);
            Guid propertyFormId = SeedAddressForm(context, "GB", "Property");
            SeedPropertyAddressFieldDefinition(context, propertyFormId);
            SeedAddressFormEntityTypeItem(context, "Property", propertyFormId);

            Guid requirementFormId = SeedAddressForm(context, "GB", "Requirement");
            SeedRequirementAddressFieldDefinition(context, requirementFormId);
            SeedAddressFormEntityTypeItem(context, "Requirement", requirementFormId);
        }

        private static void SeedAddressFormEntityTypeItem(KnightFrankContext context, string entityTypeCode, Guid formId)
        {
            var addressFormEntityType =
                new AddressFormEntityType
                {
                    AddressFormId = formId,
                    EnumTypeItemId = GetEntityTypeItemIdByCode(context, entityTypeCode)
                };

            context.AddressFormEntityType.AddOrUpdate(afet => new { afet.AddressFormId, afet.EnumTypeItemId }, addressFormEntityType);
            context.SaveChanges();
        }

        private static Guid GetEntityTypeItemIdByCode(KnightFrankContext context, string propertyCode)
        {
            return context.EnumTypeItem.FirstOrDefault(eti => eti.Code == propertyCode)?.Id ?? default(Guid);
        }

        private static void SeedRequirementAddressFieldDefinition(KnightFrankContext context, Guid formId)
        {
            var addressFieldDefinitions = new List<AddressFieldDefinition>
            {
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    RegEx = @"^.{0,128}$",
                    AddressFieldId = GetAddressFieldIdByName(context, "City"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.CITY"),
                    Required = false,
                    RowOrder = 1,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    RegEx = @"^.{0,128}$",
                    AddressFieldId = GetAddressFieldIdByName(context, "Line2"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.STREETNAME"),
                    Required = false,
                    RowOrder = 2,
                    ColumnOrder = 1,
                    ColumnSize = 6
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    RegEx = @"^.{1,10}$",
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.POSTCODE"),
                    Required = false,
                    RowOrder = 3,
                    ColumnOrder = 1,
                    ColumnSize = 6
                }
            };

            context.AddressFieldDefinition.AddOrUpdate(af => new { af.AddressFieldId, af.AddressFormId, af.AddressFieldLabelId }, addressFieldDefinitions.ToArray());
            context.SaveChanges();
        }

        private static void SeedPropertyAddressFieldDefinition(KnightFrankContext context, Guid formId)
        {
            var addressFieldDefinitions = new List<AddressFieldDefinition>
            {
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyNumber"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.PROPERTYNUMBER"),
                    RegEx = "^.{1,8}$",
                    Required = false,
                    RowOrder = 1,
                    ColumnOrder = 1,
                    ColumnSize = 3
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    RegEx = "^.{0,128}$",
                    AddressFieldId = GetAddressFieldIdByName(context, "PropertyName"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.PROPERTYNAME"),
                    Required = false,
                    RowOrder = 1,
                    ColumnOrder = 2,
                    ColumnSize = 7
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    AddressFieldId = GetAddressFieldIdByName(context, "Line2"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.LINE2"),
                    RegEx = "^.{0,128}$",
                    Required = false,
                    RowOrder = 2,
                    ColumnOrder = 1,
                    ColumnSize = 10
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    AddressFieldId = GetAddressFieldIdByName(context, "Line3"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.LINE3"),
                    RegEx = "^.{0,128}$",
                    Required = false,
                    RowOrder = 3,
                    ColumnOrder = 1,
                    ColumnSize = 10
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    AddressFieldId = GetAddressFieldIdByName(context, "Postcode"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.POSTCODE"),
                    RegEx = "^.{1,10}$",
                    Required = true,
                    RowOrder = 4,
                    ColumnOrder = 1,
                    ColumnSize = 3
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    AddressFieldId = GetAddressFieldIdByName(context, "City"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.CITY"),
                    RegEx = "^.{0,128}$",
                    Required = false,
                    RowOrder = 5,
                    ColumnOrder = 1,
                    ColumnSize = 7
                },
                new AddressFieldDefinition
                {
                    AddressFormId = formId,
                    AddressFieldId = GetAddressFieldIdByName(context, "County"),
                    AddressFieldLabelId = GetAddressFieldLabelIdByLabelKey(context, "ADDRESSFORM.COUNTY"),
                    RegEx = "^.{0,128}$",
                    Required = false,
                    RowOrder = 6,
                    ColumnOrder = 1,
                    ColumnSize = 7
                }
            };

            context.AddressFieldDefinition.AddOrUpdate(af => new { af.AddressFieldId, af.AddressFormId, af.AddressFieldLabelId }, addressFieldDefinitions.ToArray());
            context.SaveChanges();
        }

        private static Guid SeedAddressForm(KnightFrankContext context, string countryCode, string entityEnumCode)
        {
            AddressForm addressForm =
                context.AddressForm.FirstOrDefault(af => af.Country.IsoCode == countryCode &&
                                                         af.AddressFormEntityTypes.Any(
                                                             afet => afet.EnumTypeItem.Code == entityEnumCode));

            addressForm = addressForm ?? new AddressForm
            {
                CountryId = GetCountryIdByCode(context, countryCode)
            };

            context.AddressForm.AddOrUpdate(addressForm);
            context.SaveChanges();

            return addressForm.Id;
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
                    Name = "Line1"
                },
                new AddressField
                {
                    Name = "Line2"
                },
                new AddressField
                {
                    Name = "Line3"
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
                    AddressFieldId = GetAddressFieldIdByName(context, "Line1"),
                    LabelKey = "ADDRESSFORM.LINE1"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Line1"),
                    LabelKey = "ADDRESSFORM.STREET"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Line1"),
                    LabelKey = "ADDRESSFORM.ADDRESS"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Line1"),
                    LabelKey = "ADDRESSFORM.STREETNUMBER"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Line2"),
                    LabelKey = "ADDRESSFORM.LINE2"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Line2"),
                    LabelKey = "ADDRESSFORM.STREETNAME"
                },
                new AddressFieldLabel
                {
                    AddressFieldId = GetAddressFieldIdByName(context, "Line3"),
                    LabelKey = "ADDRESSFORM.LINE3"
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

            context.AddressFieldLabel.AddOrUpdate(x => new { x.LabelKey, x.AddressFieldId }, input.ToArray());
            context.SaveChanges();
        }

        private static Guid GetCountryIdByCode(KnightFrankContext context, string countryCode)
        {
            Country country = context.Country.FirstOrDefault(c => c.IsoCode == countryCode);

            return country?.Id ?? default(Guid);
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