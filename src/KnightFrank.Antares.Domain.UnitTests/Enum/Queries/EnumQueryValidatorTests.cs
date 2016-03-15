namespace KnightFrank.Antares.Domain.UnitTests.Enum.Queries
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum.Queries;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("EnumQueryValidator")]
    [Trait("FeatureTitle", "Enums")]
    public class EnumQueryValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectEnumQuery_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IReadGenericRepository<EnumType>> enumTypeRepository,
            EnumQueryValidator validator,
            EnumQuery query)
        {
            // Arrange
            enumTypeRepository.Setup(x => x.Get()).Returns(new List<EnumType> { new EnumType { Code = query.Code } }.AsQueryable());

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Asserts
            validationResult.Errors.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectEnumQueryWithNotExistingEnumCode_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IReadGenericRepository<EnumType>> enumTypeRepository,
            EnumQueryValidator validator,
            EnumQuery query)
        {
            // Arrange
            enumTypeRepository.Setup(x => x.Get()).Returns(new List<EnumType>().AsQueryable());

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Asserts
            validationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == "Enum does not exists.");
        }
    }
}
