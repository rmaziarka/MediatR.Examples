namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.QueryResults
{
    using FluentAssertions;

    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

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
