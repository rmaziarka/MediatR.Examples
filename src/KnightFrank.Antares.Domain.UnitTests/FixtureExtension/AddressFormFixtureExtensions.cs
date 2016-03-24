namespace KnightFrank.Antares.Domain.UnitTests.FixtureExtension
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Domain.AddressForm.Queries;

    using Ploeh.AutoFixture;

    public static class AddressFormFixtureExtensions
    {
        public static AddressFormQuery BuildAddressFormQuery(
            this IFixture fixture,
            string entityTypeCode,
            string countryCode)
        {
            return fixture.Build<AddressFormQuery>()
                .With(q => q.EntityType, entityTypeCode)
                .With(q => q.CountryCode, countryCode)
                .Create();
        }

        public static GetCountriesForAddressFormsQuery BuildGetCountriesForAddressFormQuery(
            this IFixture fixture,
            string entityTypeCode,
            string isoCode)
        {
            return fixture.Build<GetCountriesForAddressFormsQuery>()
                .With(q => q.EntityType, entityTypeCode)
                .With(q => q.LocaleIsoCode, isoCode)
                .Create();
        }

        public static EnumType BuildEnumType(this IFixture fixture, string enumTypeCode)
        {
            return fixture.Build<EnumType>()
                .With(x => x.Code, enumTypeCode).Create();
        }

        public static EnumTypeItem BuildEnumTypeItem(this IFixture fixture, EnumType enumType, string entityTypeCode)
        {
            return fixture.Build<EnumTypeItem>()
                .With(x => x.Code, entityTypeCode)
                .With(x => x.EnumType, enumType)
                .With(x => x.EnumTypeId, enumType.Id)
                .Create();
        }

        public static EnumTypeItem BuildEnumTypeItem(
            this IFixture fixture,
            string enumTypeCode,
            string entityTypeCode)
        {
            EnumType enumType = fixture.Build<EnumType>()
                .With(x => x.Code, enumTypeCode).Create();

            return fixture.Build<EnumTypeItem>()
                .With(x => x.Code, entityTypeCode)
                .With(x => x.EnumType, enumType)
                .With(x => x.EnumTypeId, enumType.Id)
                .Create();
        }

        public static AddressForm BuildAddressForm(
            this IFixture fixture,
            EnumTypeItem enumTypeItem,
            Country country)
        {
            List<AddressFormEntityType> addressFormEntityTypes;
            if (enumTypeItem != null)
            {
                addressFormEntityTypes = new List<AddressFormEntityType>
                {
                    new AddressFormEntityType
                    {
                        EnumTypeItem = enumTypeItem,
                        EnumTypeItemId = enumTypeItem.Id
                    }
                };
            }
            else
            {
                addressFormEntityTypes = new List<AddressFormEntityType>();
            }

            return fixture.Build<AddressForm>()
                          .With(a => a.Country, country)
                          .With(a => a.CountryId, country.Id)
                          .With(a => a.AddressFormEntityTypes, addressFormEntityTypes)
                          .Create();
        }
    }
}
