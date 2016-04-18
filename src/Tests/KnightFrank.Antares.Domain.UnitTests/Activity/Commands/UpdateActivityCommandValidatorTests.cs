namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("UpdateActivityCommandValidator")]
    public class UpdateActivityCommandValidatorTests
    {
        [Theory]
        [AutoMoqData]
        [InlineAutoMoqData(0)]
        [InlineAutoMoqData(10, 0)]
        [InlineAutoMoqData(20, 10, 0)]
        public void Given_ValidUpdateActivityCommand_When_Validating_Then_IsValid(
            decimal marketAppraisalPrice,
            decimal recommendedPrice,
            decimal vendorEstimatedPrice,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.MarketAppraisalPrice = marketAppraisalPrice;
            command.RecommendedPrice = recommendedPrice;
            command.VendorEstimatedPrice = vendorEstimatedPrice;

            AssertIfValid(enumTypeItemRepository, command, validator);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_MarketAppraisalPriceIsNotSet_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.MarketAppraisalPrice = null;

            AssertIfValid(enumTypeItemRepository, command, validator);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_RecommendedPriceIsNotSet_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.RecommendedPrice = null;

            AssertIfValid(enumTypeItemRepository, command, validator);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_VendorEstimatedPriceIsNotSet_Validating_Then_IsValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.VendorEstimatedPrice = null;

            AssertIfValid(enumTypeItemRepository, command, validator);
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidUpdateActivityCommand_When_MarketAppraisalPriceIsNegative_Then_IsNotValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.MarketAppraisalPrice = -1;

            this.AssertIfNotNegativePriceValidation(enumTypeItemRepository, command, validator, nameof(command.MarketAppraisalPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidUpdateActivityCommand_When_RecommendedPriceIsNegative_Then_IsNotValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.RecommendedPrice = -1;

            this.AssertIfNotNegativePriceValidation(enumTypeItemRepository, command, validator, nameof(command.RecommendedPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidUpdateActivityCommand_When_VendorEstimatedPriceIsNegative_Then_IsNotValid(
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            command.VendorEstimatedPrice = -1;

            this.AssertIfNotNegativePriceValidation(enumTypeItemRepository, command, validator, nameof(command.VendorEstimatedPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidationRules_When_Configuring_Then_ActivityStatusIdHaveValidatorSetup(UpdateActivityCommandValidator validator)
        {
            validator.ShouldHaveChildValidator(x => x.ActivityStatusId, typeof(ActivityStatusValidator));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityIdInCommand_When_Validating_Then_ValidationPasses(
            [Frozen]Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen]Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            activityRepository.Setup(x => x.GetById(command.Id)).Returns(new Activity());

            ValidationResult validationResult = validator.Validate(command);

            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityIdInCommand_When_Validating_Then_ShouldReturnValidationError(
            [Frozen]Mock<IGenericRepository<Activity>> activityRepository,
            [Frozen]Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            activityRepository.Setup(x => x.GetById(command.Id)).Returns((Activity)null);

            ValidationResult validationResult = validator.Validate(command);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == "Activity does not exist.");
            validationResult.Errors.Should().ContainSingle(x => x.PropertyName == nameof(command.Id));
        }

        private void AssertIfNotNegativePriceValidation(
            Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            UpdateActivityCommand command,
            UpdateActivityCommandValidator validator,
            string propertyName)
        {
            // Arrange
            enumTypeItemRepository.Setup(r => r.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == propertyName);
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.greaterthanorequal_error));
        }

        private static void AssertIfValid(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository, UpdateActivityCommand command,
            UpdateActivityCommandValidator validator)
        {
            // Arrange
            enumTypeItemRepository.Setup(r => r.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}