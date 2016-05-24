namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;
    using System.Collections.Generic;
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
            command.SecondaryNegotiatorIds = new List<Guid>();

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_LeadNegotiatorIsNotSet_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            // Arrange
            command.LeadNegotiatorId = default(Guid);

            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.LeadNegotiatorId), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_SecondaryNegotiatorsIsNotSet_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            // Arrange
            command.SecondaryNegotiatorIds = null;

            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.SecondaryNegotiatorIds), nameof(Messages.notnull_error));
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