namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Xunit;

    public class CreateRequirementCommandDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CreateRequirementCommandDomainValidator_When_Validating_Then_ContactIdsPropertyShouldHaveProperValidator(
            CreateRequirementCommandDomainValidator validator)
        {
            validator.ShouldHaveChildValidator(x => x.ContactIds, typeof(ContactValidator));
        }
    }
}
