namespace KnightFrank.Antares.Search.UnitTests.Property.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Search.Common.QueryResults;
    using KnightFrank.Antares.Search.Models;
    using KnightFrank.Antares.Search.Property.Queries;
    using KnightFrank.Antares.Search.Property.QueryHandlers;
    using KnightFrank.Antares.Search.Property.QueryResults;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Nest;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ElasticSearchQueryHandler")]
    [Trait("FeatureTitle", "ElasticSearchQueryHandler")]
    public class PropertiesPageableQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_PropertiesSearchDescriptor_When_Create_Then_ShouldEmptyOwnershipShouldNotBeMapped(
            SearchDescriptor<Property> propertySearchDescriptor,
            SearchDescriptor<Contact> contactSearchDescriptor,
            ISearchResponse<Property> propertySearchResponse,
            ISearchResponse<Contact> contactSearchResponse,
            [Frozen] Mock<IElasticClient> elasticClient,
            PropertiesPageableQuery pageableQuery,
            PropertiesPageableQueryHandler handler)
        {
            // Arrange
            elasticClient.Setup(c => c.Search<Property>(propertySearchDescriptor)).Returns(propertySearchResponse);
            elasticClient.Setup(c => c.Search<Contact>(contactSearchDescriptor)).Returns(contactSearchResponse);

            // Act
            PageableResult<PropertyResult> result = handler.Handle(pageableQuery);

            // Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(propertySearchResponse.Total);
            result.Page.Should().Be(pageableQuery.Page);
            result.Size.Should().Be(pageableQuery.Size);
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(propertySearchResponse.Documents.ToList().Count);
            result.Data.ShouldAllBeEquivalentTo(
                propertySearchResponse.Documents,
                options => options.IncludingProperties().ExcludingMissingMembers());

            List<ContactResult> contactResults = result.Data.SelectMany(p => p.Ownerships).SelectMany(o => o.Contacts).ToList();
            contactResults.Should().HaveCount(contactSearchResponse.Documents.ToList().Count);
            contactResults.ShouldAllBeEquivalentTo(
                contactSearchResponse.Documents,
                options => options.IncludingProperties().ExcludingMissingMembers());
        }
    }
}
