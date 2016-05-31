namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Ploeh.AutoFixture;

    using Xunit;

    using KnightFrank.Antares.Domain.Activity.Commands;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("UpdateActivityUserCommandValidator")]
    public class UpdateActivityUserCommandValidatorTests
    {
        private readonly UpdateActivityUserCommandValidator validator;

        public UpdateActivityUserCommandValidatorTests()
        {
            IFixture fixture = new Fixture().Customize();
            this.validator = fixture.Create<UpdateActivityUserCommandValidator>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityUserCommand_When_Validating_Then_IsValid(
            UpdateActivityUserCommand command)
        {
            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityUserCommandWithNullDate_When_Validating_Then_IsValid(
            UpdateActivityUserCommand command)
        {
            command.CallDate = null;

            this.AssertIfValid(command);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_LeadNegotiatorIdIsNotSet_Validating_Then_IsNotValid(
            UpdateActivityUserCommand command)
        {
            // Arrange
            command.Id = default(Guid);

            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Id), nameof(Messages.notempty_error));
        }

        private void AssertIfValid(UpdateActivityUserCommand command)
        {
            // Act
            ValidationResult validationResult = this.validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }
}