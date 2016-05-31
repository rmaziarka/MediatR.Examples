namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Ploeh.AutoFixture;

    using Xunit;

    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Activity.Commands;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("UpdateActivityCommandValidator")]
    public class UpdateActivityCommandValidatorTests
    {
        private readonly UpdateActivityCommandValidator validator;

        public UpdateActivityCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();
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
        public void Given_ValidUpdateActivityCommand_When_SecondaryNegotiatorsIsEmpty_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            command.SecondaryNegotiators = new List<UpdateActivityUserCommand>();

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_LeadNegotiatorIsNotSet_Validating_Then_IsNotValid(
            UpdateActivityCommand command)
        {
            // Arrange
            command.LeadNegotiator = null;

            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.LeadNegotiator), nameof(Messages.notnull_error));
        }

        [Fact]
        public void Given_UpdateActivityCommandValidator_When_Validating_LeadNegotiatorHaveConfiguredValidator()
        {
            this.validator.ShouldHaveChildValidator(x => x.LeadNegotiator, typeof(UpdateActivityUserCommandValidator));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_SecondaryNegotiatorsIsNotSet_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            // Arrange
            command.SecondaryNegotiators = null;

            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.SecondaryNegotiators), nameof(Messages.notnull_error));
        }

        [Fact]
        public void Given_UpdateActivityCommandValidator_When_Validating_SecondaryNegotiatorsHaveConfiguredValidator()
        {
            this.validator.ShouldHaveChildValidator(x => x.SecondaryNegotiators, typeof(UpdateActivityUserCommandValidator));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ExistingActivityIdInCommand_When_Validating_Then_ValidationPasses(
            UpdateActivityCommand command)
        {
            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeTrue();
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