namespace KnightFrank.Antares.Domain.UnitTests.User.Commands
{
    using System;

    using FluentAssertions;
    using FluentValidation.Resources;
    using FluentValidation.Results;
    using Xunit;

    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Domain.User.Commands;

   

    [Collection("UpdateUserCommand")]
    [Trait("FeatureTitle", "User")]
    public class UpdateUserCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
      [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateOfferCommand_When_Validating_Then_IsValid(UpdateUserCommandValidator validator
            , UpdateUserCommand command)
        {
            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_EmptyId_When_Validating_Then_IsInvalid(UpdateUserCommandValidator validator
             , UpdateUserCommand command)
        {
            // Arrange
            command.Id = Guid.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.Id), nameof(Messages.notempty_error));
        }     
    }
}
