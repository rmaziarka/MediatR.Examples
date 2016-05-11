namespace KnightFrank.Antares.Domain.UnitTests.RequirementNote.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.RequirementNote.Queries;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

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
            validationResult.IsInvalid(nameof(query.Id), nameof(Messages.notempty_error));
        }
    }
}
