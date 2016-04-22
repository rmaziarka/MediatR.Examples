namespace KnightFrank.Antares.Domain.UnitTests.Common.Validator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("ActivityStatusValidator")]
    public class ActivityStatusValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_ActivityStatusDoesNotExist_When_Validating_Then_IsInvalid(
           ActivityStatusValidator validator,
           Guid activityStatusId)
        {
            // Act
            ValidationResult validationResult = validator.Validate(activityStatusId);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(activityStatusId));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorMessage == "Activity Status does not exist.");
        }

        [Theory]
        [AutoMoqData]
        public void Given_ActivityStatusValidator_When_Validating_Then_CorrectActivityStatusIsFiltered(
            [Frozen]Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            EnumTypeItem enumTypeItem,
            Guid activityStatusId,
            ActivityStatusValidator validator)
        {
            // Arrange
            var mockedData = new List<EnumTypeItem>();
            enumTypeItem.EnumType = new EnumType { Code = "ActivityStatus" };
            enumTypeItem.Id = activityStatusId;

            mockedData.Add(enumTypeItem);

            enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) => mockedData.Any(expr.Compile()));

            // Act
            ValidationResult validationResult = validator.Validate(activityStatusId);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}