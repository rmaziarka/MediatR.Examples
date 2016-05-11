namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Moq;

    using Ploeh.AutoFixture;

    using Xunit;
    using FixtureExtension;
    [Trait("FeatureTitle", "Property Activity")]
    [Collection("UpdateActivityCommandValidator")]
    public class UpdateActivityCommandValidatorTests
    {
        private readonly Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        private readonly Mock<IGenericRepository<ActivityType>> activityTypeRepository;
        private readonly Mock<IGenericRepository<Activity>> activityRepository;
        private readonly Mock<IGenericRepository<ActivityTypeDefinition>> activityTypeDefinitionRepository;
        private readonly UpdateActivityCommandValidator validator;

        public UpdateActivityCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();

            this.enumTypeItemRepository = fixture.Freeze<Mock<IGenericRepository<EnumTypeItem>>>();
            this.activityTypeRepository = fixture.Freeze<Mock<IGenericRepository<ActivityType>>>();
            this.activityRepository = fixture.Freeze<Mock<IGenericRepository<Activity>>>();
            this.activityTypeDefinitionRepository = fixture.Freeze<Mock<IGenericRepository<ActivityTypeDefinition>>>();
            
            this.enumTypeItemRepository.Setup(r => r.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(true);
            this.activityTypeRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ActivityType, bool>>>())).Returns(true);
            this.activityRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Activity());
            this.activityTypeDefinitionRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>())).Returns(true);

            this.validator = fixture.Create<UpdateActivityCommandValidator>();
        }

        [Theory]
        [AutoMoqData]
        [InlineAutoMoqData(0)]
        [InlineAutoMoqData(10, 0)]
        [InlineAutoMoqData(20, 10, 0)]
        public void Given_ValidUpdateActivityCommand_When_Validating_Then_IsValid(
            decimal marketAppraisalPrice,
            decimal recommendedPrice,
            decimal vendorEstimatedPrice,
            UpdateActivityCommand command)
        {
            command.MarketAppraisalPrice = marketAppraisalPrice;
            command.RecommendedPrice = recommendedPrice;
            command.VendorEstimatedPrice = vendorEstimatedPrice;

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_MarketAppraisalPriceIsNotSet_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            command.MarketAppraisalPrice = null;

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_RecommendedPriceIsNotSet_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            command.RecommendedPrice = null;

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_VendorEstimatedPriceIsNotSet_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            command.VendorEstimatedPrice = null;

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidUpdateActivityCommand_When_MarketAppraisalPriceIsNegative_Then_IsNotValid(
            UpdateActivityCommand command)
        {
            command.MarketAppraisalPrice = -1;

            AssertIfNotNegativePriceValidation(command, this.validator, nameof(command.MarketAppraisalPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidUpdateActivityCommand_When_RecommendedPriceIsNegative_Then_IsNotValid(
            UpdateActivityCommand command)
        {
            command.RecommendedPrice = -1;

            AssertIfNotNegativePriceValidation(command, this.validator, nameof(command.RecommendedPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_InvalidUpdateActivityCommand_When_VendorEstimatedPriceIsNegative_Then_IsNotValid(
            UpdateActivityCommand command)
        {
            command.VendorEstimatedPrice = -1;

            AssertIfNotNegativePriceValidation(command, this.validator, nameof(command.VendorEstimatedPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityIdInCommand_When_Validating_Then_ValidationPasses(
            UpdateActivityCommand command)
        {
            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityIdInCommand_When_Validating_Then_ShouldReturnValidationError(
            UpdateActivityCommand command)
        {
            this.activityRepository.Setup(x => x.GetById(command.Id)).Returns((Activity)null);

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(x => x.ErrorMessage == "Activity does not exist.");
            validationResult.Errors.Should().ContainSingle(x => x.PropertyName == nameof(command.Id));
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityStatusIdInCommand_When_Validating_Then_ShouldReturnValidationError(
           UpdateActivityCommand command)
        {
            this.enumTypeItemRepository.Setup(x => x.Any(It.IsAny<Expression<Func<EnumTypeItem, bool>>>())).Returns(false);

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(x => x.PropertyName == nameof(command.ActivityStatusId));
        }


        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityTypeInCommand_When_Validating_Then_ShouldReturnValidationError(
            UpdateActivityCommand command)
        {
            this.activityTypeRepository.Setup(x => x.Any(It.IsAny<Expression<Func<ActivityType, bool>>>())).Returns(false);

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(x => x.PropertyName == nameof(command.ActivityTypeId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_NotExistingActivityTypeDefinitionInCommand_When_Validating_Then_ShouldReturnValidationError(
            UpdateActivityCommand command)
        {
            this.activityTypeDefinitionRepository.Setup(x => x.Any(It.IsAny<Expression<Func<ActivityTypeDefinition, bool>>>())).Returns(false);

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(x => x.PropertyName == nameof(command.ActivityTypeId));
        }

        private static void AssertIfNotNegativePriceValidation(UpdateActivityCommand command,
            UpdateActivityCommandValidator validator,
            string propertyName)
        {
            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == propertyName);
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.greaterthanorequal_error));
        }

        private void AssertIfValid(UpdateActivityCommand command)
        {
            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}