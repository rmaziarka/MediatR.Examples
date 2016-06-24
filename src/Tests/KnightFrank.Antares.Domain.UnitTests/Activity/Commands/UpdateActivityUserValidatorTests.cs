namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using Ploeh.AutoFixture;

    using Xunit;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("UpdateActivityUserValidator")]
    public class UpdateActivityUserValidatorTests
    {
        private readonly UpdateActivityUserValidator validator;

        public UpdateActivityUserValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();
            this.validator = fixture.Create<UpdateActivityUserValidator>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityUser_When_Validating_Then_IsValid(
            UpdateActivityUser command)
        {
            this.AssertIfValid(command);
        }

        //[Theory] TODO: solve issue with Autofixture and incjecting values inside constructor
        [AutoMoqData]
        public void Given_ValidUpdateActivityUserWithNullDate_When_Validating_Then_IsValid(
            UpdateActivityUser command)
        {
            command.CallDate = null;

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_LeadNegotiatorIdIsNotSet_Validating_Then_IsNotValid(
            UpdateActivityUser command)
        {
            // Arrange
            command.UserId = default(Guid);

            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.UserId), nameof(Messages.notempty_error));
        }

        private void AssertIfValid(UpdateActivityUser command)
        {
            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}