namespace KnightFrank.Antares.Domain.UnitTests.Tenancy.Query
{
    using Domain.Tenancy.Queries;
    using FluentAssertions;
    using FluentValidation.Resources;
    using FluentValidation.Results;
    using Ploeh.AutoFixture.Xunit2;
    using System;

    using FluentValidation.Validators;

    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier;

    using Tests.Common.Extensions.Fluent.ValidationResult;
    using Xunit;

    [Collection("TenancyQueryValidator")]
    [Trait("FeatureTitle", "Tenancy")]
    public class TenancyQueryValidatorTests
    {
        [Fact]
        public void Given_When_CreateQueryValidatorInstance_Then_ShouldHaveCorrectRules()
        {
            // arrange
            var validator = new TenancyQueryValidator();

            // Assert
            validator.ShouldHaveRulesCount(1);

            validator.ShouldHaveRules(x => x.Id,
                RuleVerifiersComposer.Build()
                .AddPropertyValidatorVerifier<NotEmptyValidator>()
                .Create());
        }
    }
}
