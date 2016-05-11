namespace KnightFrank.Antares.Api.UnitTests.FixtureExtension
{
    using FluentAssertions;

    using FluentValidation.Results;

    public static class ValidationResultExtensions
    {
        public static void IsInvalid(this ValidationResult validationResult, string propertyName, string errorCode)
        {
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle(e => e.PropertyName == propertyName);
            validationResult.Errors.Should().ContainSingle(e => e.ErrorCode == errorCode);
        }
    }
}