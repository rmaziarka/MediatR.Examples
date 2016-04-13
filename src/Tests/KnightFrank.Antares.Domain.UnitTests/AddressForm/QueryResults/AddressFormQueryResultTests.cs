namespace KnightFrank.Antares.Domain.UnitTests.AddressForm.QueryResults
{
    using FluentAssertions;

    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using Xunit;

    [Collection("Models")]
    [Trait("FeatureTitle", "Models")]
    public class AddressFormQueryResultTests
    {
        [Fact]
        public void Given_AddressFormQueryResult_When_Create_Then_AddressFieldDefinitionsShouldBeEmpty()
        {
            // Act
            var addressFormQueryResult = new AddressFormQueryResult();

            // Act
            addressFormQueryResult.AddressFieldDefinitions.Should().NotBeNull();
            addressFormQueryResult.AddressFieldDefinitions.Should().BeEmpty();
        }
    }
}
