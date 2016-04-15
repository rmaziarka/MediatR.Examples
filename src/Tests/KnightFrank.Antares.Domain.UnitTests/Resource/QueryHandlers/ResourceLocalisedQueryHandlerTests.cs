namespace KnightFrank.Antares.Domain.UnitTests.Resource.QueryHandlers
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Resource.Queries;
    using KnightFrank.Antares.Domain.Resource.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture;
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
            IList<ValidationFailure> validationFailures,
            IFixture fixture)
        {
            // Arrange
            domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult(validationFailures));

            // Act + Assert
            Assert.Throws<DomainValidationException>(() => handler.Handle(query)).Errors.ShouldBeEquivalentTo(validationFailures);
        }
    }
}
