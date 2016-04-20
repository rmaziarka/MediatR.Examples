namespace KnightFrank.Antares.Domain.UnitTests.Resource.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
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
            Mock<ILocalised> localised,
            Mock<ILocalised> localisedWithOtherLocale,
            Mock<ICovariantReadGenericRepository<ILocalised>> repository,
            [Frozen] Mock<IResourceLocalisedRepositoryProvider> repositoryProvider,
            ResourceLocalisedQueryHandler handler,
            ResourceLocalisedQuery query,
            IFixture fixture)
        {
            // Arrange
            domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
            localised.Setup(l => l.Locale).Returns(this.CreateLocale(fixture, query.IsoCode));
            localisedWithOtherLocale.Setup(l => l.Locale).Returns(this.CreateLocale(fixture, query.IsoCode + "Other"));
            repository.Setup(r => r.Get()).Returns(new[] { localised.Object, localisedWithOtherLocale.Object }.AsQueryable());
            repositoryProvider.Setup(p => p.GetRepositories()).Returns(new[] { repository.Object });

            // Act
            Dictionary<Guid, string> dictionary = handler.Handle(query);

            // Assert
            dictionary.Should().NotBeNull();
            dictionary.Should().HaveCount(1);
            dictionary.Should().ContainKey(localised.Object.ResourceId);
            dictionary.Should().ContainValue(localised.Object.Value);
            repository.Verify(r => r.Get(), Times.Once());
            repositoryProvider.Verify(r => r.GetRepositories(), Times.Once());
        }

        private Locale CreateLocale(IFixture fixture, string isoCode)
        {
            return fixture.Build<Locale>().With(l => l.IsoCode, isoCode).Create();
        }
    }
}
