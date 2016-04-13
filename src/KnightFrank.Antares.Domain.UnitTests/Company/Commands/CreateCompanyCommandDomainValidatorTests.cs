namespace KnightFrank.Antares.Domain.UnitTests.Company.Commands
{
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.Common.Validator;
    using KnightFrank.Antares.Domain.Company.Commands;

    using Xunit;

    [Trait("FeatureTitle", "Company")]
    [Collection("CreateCompanyCommandDomainValidator")]
    public class CreateCompanyCommandDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CreateCompanyCommandDomainValidator_When_Validating_Then_ContactIdsPropertyShouldHaveProperValidator(
            CreateCompanyCommandDomainValidator commandDomainValidator)
        {
            commandDomainValidator.ShouldHaveChildValidator(x => x.ContactIds, typeof(ContactValidator));
        }
    }
}