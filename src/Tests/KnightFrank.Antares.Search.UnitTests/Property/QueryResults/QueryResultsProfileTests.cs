namespace KnightFrank.Antares.Search.UnitTests.Property.QueryResults
{
    using System.Collections.Generic;

    using AutoMapper;

    using FluentAssertions;

    using KnightFrank.Antares.Search.Models;
    using KnightFrank.Antares.Search.Property.QueryResults;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("Mappings")]
    [Trait("FeatureTitle", "Mappings")]
    public class QueryResultsProfileTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoData]
        public void Given_Property_When_Mapping_Then_ShouldGetPropertyResult(Ownership ownership, IFixture fixture)
        {
            // Arrange
            Property property =
                fixture.Build<Property>().With(p => p.Ownerships, new List<Ownership> { new Ownership(), ownership }).Create();

            // Act
            var propertyResult = Mapper.Map<PropertyResult>(property);

            // Assert
            propertyResult.Should().NotBeNull();
            propertyResult.Ownerships.Should().HaveCount(1);
            propertyResult.Ownerships[0].ShouldBeEquivalentTo(
                ownership,
                options => options.IncludingProperties().ExcludingMissingMembers());
        }
    }
}
