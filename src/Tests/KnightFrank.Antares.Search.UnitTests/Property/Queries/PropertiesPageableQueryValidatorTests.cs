namespace KnightFrank.Antares.Search.UnitTests.Property.Queries
{
    using FluentValidation.Validators;

    using KnightFrank.Antares.Search.Property.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier;

    using Xunit;

    [Trait("FeatureTitle", "Property")]
    [Collection("PropertiesPageableQueryValidator")]
    public class PropertiesPageableQueryValidatorTests
    {
        private readonly PropertiesPageableQueryValidator queryValidator = new PropertiesPageableQueryValidator();

        [Fact]
        public void Given_When_CreateInstance_Then_ShouldHaveCorrectRules()
        {
            // Assert
            this.queryValidator.ShouldHaveRulesCount(3);

            this.queryValidator.ShouldHaveRules(
                x => x.Page,
                RuleVerifiersComposer.Build().AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>(0).Create());

            this.queryValidator.ShouldHaveRules(
                x => x.Size,
                RuleVerifiersComposer.Build().AddPropertyValidatorVerifier<GreaterThanValidator>(0).Create());

            this.queryValidator.ShouldHaveRules(
                x => x.Query,
                RuleVerifiersComposer.Build().AddPropertyValidatorVerifier<NotEmptyValidator>().Create());
        }
    }
}
