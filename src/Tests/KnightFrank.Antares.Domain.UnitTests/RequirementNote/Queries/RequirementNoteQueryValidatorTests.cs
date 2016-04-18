namespace KnightFrank.Antares.Domain.UnitTests.RequirementNote.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.RequirementNote.Queries;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("RequirementNoteQueryValidator")]
    [Trait("FeatureTitle", "RequirementNote")]
    public class RequirementNoteQueryValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectRequirementNoteQuery_When_Validating_Then_IsValid(
            RequirementNoteQueryValidator validator,
            RequirementNoteQuery query)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoData]
        public void Given_RequirementNoteIdIsEmpty_When_Validating_Then_IsInvalid(
            RequirementNoteQueryValidator validator,
            RequirementNoteQuery query)
        {
            // Arrange
            query.Id = default(Guid);

            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == nameof(query.Id));
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == nameof(Messages.notempty_error));
        }
    }
}
