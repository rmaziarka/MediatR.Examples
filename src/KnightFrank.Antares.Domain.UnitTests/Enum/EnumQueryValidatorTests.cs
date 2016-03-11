namespace KnightFrank.Antares.Domain.UnitTests.Enum
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateContactCommandValidator")]
    [Trait("FeatureTitle", "Contacts")]
    public class EnumQueryValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectEnumQuery_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IReadGenericRepository<EnumType>> enumTypeRepository,
            EnumQueryValidator validator,
            EnumQuery query)
        {
            enumTypeRepository.Setup(x => x.Get()).Returns(new List<EnumType> { new EnumType { Code = query.Code } }.AsQueryable());

            ValidationResult validationResult = validator.Validate(query);

            validationResult.Errors.Should().BeEmpty();
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectEnumQueryWithNotExistingEnumCode_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IReadGenericRepository<EnumType>> enumTypeRepository,
            EnumQueryValidator validator,
            EnumQuery query)
        {
            enumTypeRepository.Setup(x => x.Get()).Returns(new List<EnumType>().AsQueryable());

            ValidationResult validationResult = validator.Validate(query);

            validationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == "Enum does not exists.");
        }
    }
}