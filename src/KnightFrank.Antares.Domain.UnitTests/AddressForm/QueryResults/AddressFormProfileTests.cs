namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.QueryResults
{
    using AutoMapper;
    
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("Mappings")]
    [Trait("FeatureTitle", "Mappings")]
    public class AddressFormProfileTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoData]
        public void Given_AddressForm_When_Mapping_Then_ShouldGetAddressFormQueryResult(Fixture fixture)
        {
            // Arrange
            AddressField addressField =
                fixture.Build<AddressField>()
                       .Without(af => af.AddressFieldLabels)
                       .Create();

            AddressFieldLabel addressFieldLabel =
                fixture.Build<AddressFieldLabel>()
                       .Without(afl => afl.AddressField)
                       .Create();

            AddressFieldDefinition addressFieldDefinition =
                fixture.Build<AddressFieldDefinition>()
                       .With(afd => afd.AddressField, addressField)
                       .With(afd => afd.AddressFieldLabel, addressFieldLabel)
                       .Without(afd => afd.AddressForm)
                       .Create();

            AddressForm addressForm =
                fixture.Build<AddressForm>()
                       .With(af => af.AddressFieldDefinitions, new[] { addressFieldDefinition })
                       .Without(af => af.Country)
                       .Without(af => af.AddressFormEntityTypes)
                       .Create();

            // Act
            var addressFormQueryResult = Mapper.Map<AddressFormQueryResult>(addressForm);

            // Asserts
            Assert.NotNull(addressFormQueryResult);
            Assert.Equal(addressForm.Id, addressFormQueryResult.Id);
            Assert.Equal(addressForm.CountryId, addressFormQueryResult.CountryId);
            Assert.Equal(1, addressFormQueryResult.AddressFieldDefinitions.Count);
            Assert.Equal(addressField.Name, addressFormQueryResult.AddressFieldDefinitions[0].Name);
            Assert.Equal(addressFieldLabel.LabelKey, addressFormQueryResult.AddressFieldDefinitions[0].LabelKey);
            Assert.Equal(addressFieldDefinition.RegEx, addressFormQueryResult.AddressFieldDefinitions[0].RegEx);
            Assert.Equal(addressFieldDefinition.ColumnOrder, addressFormQueryResult.AddressFieldDefinitions[0].ColumnOrder);
            Assert.Equal(addressFieldDefinition.RowOrder, addressFormQueryResult.AddressFieldDefinitions[0].RowOrder);
            Assert.Equal(addressFieldDefinition.Required, addressFormQueryResult.AddressFieldDefinitions[0].Required);
        }
    }
}
