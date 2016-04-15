namespace KnightFrank.Antares.Domain.UnitTests.Enum.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryHandlers;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("EnumLocalisedQueryHandler")]
    [Trait("FeatureTitle", "Enums")]
    public class EnumLocalisedQueryHandlerTests
    {
        public class EnumQueryHandlerTests : IClassFixture<BaseTestClassFixture>
        {
            [Theory]
            [AutoMoqData]
            public void Given_EnumLocalisedQueryInvalid_When_Handling_Then_ShouldThrowDomainValidationException(
                [Frozen] Mock<IDomainValidator<EnumLocalisedQuery>> domainValidator,
                EnumLocalisedQuery query,
                EnumLocalisedQueryHandler handler,
                IList<ValidationFailure> validationFailures,
                IFixture fixture)
            {
                // Arrange
                domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult(validationFailures));

                DomainValidationException exception = null;

                // Act + Assert
                Assert.Throws<DomainValidationException>(() => handler.Handle(query)).Errors.ShouldBeEquivalentTo(validationFailures);
            }

            [Theory]
            [AutoMoqData]
            public void Given_NotExistEnumLocalised_When_Handling_Then_ShouldReturnEmptyDictionary(
                [Frozen] Mock<IDomainValidator<EnumLocalisedQuery>> domainValidator,
                [Frozen] Mock<IReadGenericRepository<EnumLocalised>> enumLocalisedRepository,
                EnumLocalisedQuery query,
                EnumLocalisedQueryHandler handler)
            {
                // Arrange
                domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
                enumLocalisedRepository.Setup(r => r.Get()).Returns(new EnumLocalised[] { }.AsQueryable());

                // Act
                Dictionary<Guid, string> dictionary = handler.Handle(query);

                // Assert
                dictionary.Should().NotBeNull();
                dictionary.Should().BeEmpty();
                domainValidator.Verify(r => r.Validate(It.IsAny<EnumLocalisedQuery>()), Times.Once());
                enumLocalisedRepository.Verify(r => r.Get(), Times.Once());
            }

            [Theory]
            [AutoMoqData]
            public void Given_ExistEnumLocalised_When_Handling_Then_ShouldReturnDictionary(
                [Frozen] Mock<IDomainValidator<EnumLocalisedQuery>> domainValidator,
                [Frozen] Mock<IReadGenericRepository<EnumLocalised>> enumLocalisedRepository,
                EnumLocalisedQuery query,
                EnumLocalisedQueryHandler handler,
                IFixture fixure)
            {
                // Arrange
                EnumLocalised enumLocalised = this.CreateEnumLocalised(fixure, query.IsoCode);
                EnumLocalised enumLocalisedWithOtherLocale = this.CreateEnumLocalised(fixure, query.IsoCode + "Other");
                domainValidator.Setup(v => v.Validate(query)).Returns(new ValidationResult());
                enumLocalisedRepository.Setup(r => r.Get()).Returns(new[] { enumLocalised, enumLocalisedWithOtherLocale }.AsQueryable());

                // Act
                Dictionary<Guid, string> dictionary = handler.Handle(query);

                // Assert
                dictionary.Should().NotBeNull();
                dictionary.Should().HaveCount(1);
                dictionary.Should().ContainKey(enumLocalised.EnumTypeItemId);
                dictionary.Should().ContainValue(enumLocalised.Value);
                domainValidator.Verify(r => r.Validate(It.IsAny<EnumLocalisedQuery>()), Times.Once());
                enumLocalisedRepository.Verify(r => r.Get(), Times.Once());
            }

            private EnumLocalised CreateEnumLocalised(IFixture fixure, string isoCode)
            {
                Locale lcoale = fixure.Build<Locale>().With(l => l.IsoCode, isoCode).Create();
                return fixure.Build<EnumLocalised>().With(el => el.Locale, lcoale).Create();
            }
        }
    }
}
