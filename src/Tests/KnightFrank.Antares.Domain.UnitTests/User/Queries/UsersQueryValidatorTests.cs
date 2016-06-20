namespace KnightFrank.Antares.Domain.UnitTests.User.Queries
{
    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.User.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;


    public class UsersQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectInput_When_Validating_Then_NoValidationErrors(
            UsersQueryValidator validator,
            UsersQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_NullInput_When_Validating_Then_ValidationError(
            UsersQueryValidator validator,
            UsersQuery query)
        {
            // Setup
            query.PartialName = null;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.PartialName), nameof(Messages.notempty_error));
        }

        [Theory]
        [AutoData]
        public void Given_EmptyInput_When_Validating_Then_ValidationError(
            UsersQueryValidator validator,
            UsersQuery query)
        {
            // Setup
            query.PartialName = string.Empty;

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsInvalid(nameof(query.PartialName), nameof(Messages.notempty_error));
        }
    }

}