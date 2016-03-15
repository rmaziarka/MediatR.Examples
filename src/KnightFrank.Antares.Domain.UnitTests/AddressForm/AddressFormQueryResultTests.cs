namespace KnightFrank.Antares.Domain.UnitTests.AddressForm
{
    using FluentAssertions;

    using KnightFrank.Antares.Domain.AddressForm;

    using Xunit;

    [Collection("Models")]
    [Trait("FeatureTitle", "Models")]
    public class AddressFormQueryResultTests
    {
        public void When_CreateAddressFormQueryResult_Then_AddressFieldDefinitionsShouldBeEmpty()
        {
            // Act
            var addressFormQueryResult = new AddressFormQueryResult();

            // Act
            addressFormQueryResult.AddressFieldDefinitions.Should().NotBeNull();
            addressFormQueryResult.AddressFieldDefinitions.Should().BeEmpty();
        } 
    }
}