namespace KnightFrank.Antares.Domain.UnitTests.Resource.QueryHandlers
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Resource.Dictionaries;
    using KnightFrank.Antares.Domain.Resource.Queries;
    using KnightFrank.Antares.Domain.Resource.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("ResourceLocalisedQueryHandler")]
    [Trait("FeatureTitle", "Resources")]
    public class ResourceLocalisedQueryHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ResourceLocalisedQueryInvalid_When_Handling_Then_ShouldThrowDomainValidationException(
            [Frozen] Mock<IDomainValidator<ResourceLocalisedQuery>> domainValidator,
            ResourceLocalisedQuery query,
            ResourceLocalisedQueryHandler handler,
            IList<ValidationFailure> validationFailures)
        {
            // Arrange
            domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult(validationFailures));

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => handler.Handle(query)).Errors.ShouldBeEquivalentTo(validationFailures);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ResourceLocalisedQueryValid_When_Handling_Then_ShouldReturnDictionary(
            Mock<IDomainValidator<ResourceLocalisedQuery>> domainValidator,
            Mock<IResourceLocalisedDictionary> resourceLocalisedDictionary,
            Dictionary<Guid, string> resourceDictionary,
            string stringValue,
            ResourceLocalisedQuery query)
        {
            // Arrange
            domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            resourceLocalisedDictionary.Setup(r => r.GetDictionary(query.IsoCode)).Returns(resourceDictionary);

            var handler = new ResourceLocalisedQueryHandler(domainValidator.Object, new[] { resourceLocalisedDictionary.Object });

            // Act
            Dictionary<Guid, string> dictionary = handler.Handle(query);

            // Assert
            dictionary.Should().NotBeNull();
            dictionary.ShouldAllBeEquivalentTo(resourceDictionary);
            resourceLocalisedDictionary.Verify(r => r.GetDictionary(It.IsAny<string>()), Times.Once());
        }
    }
}
