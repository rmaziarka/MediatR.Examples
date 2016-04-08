namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.Validator;

    using Xunit;

    [Trait("FeatureTitle", "Activity")]
    [Collection("CreateActivityCommandDomainValidator")]
    public class CreateActivityCommandDomainValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_ValidationRules_When_Configuring_Then_ContactIdsHaveValidatorSetup(
            CreateActivityCommandDomainValidator validator)
        {   
            validator.ShouldHaveChildValidator(r => r.ContactIds, typeof(ContactValidator));
        } 
    }
}