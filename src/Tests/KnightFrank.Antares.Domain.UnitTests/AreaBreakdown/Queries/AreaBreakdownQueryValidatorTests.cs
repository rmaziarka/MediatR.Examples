namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.Queries
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AreaBreakdown.Queries;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("AreaBreakdownQueryValidator")]
    public class AreaBreakdownQueryValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Fixture fixture;
        private readonly AreaBreakdownQueryValidator validator;

        public AreaBreakdownQueryValidatorTests()
        {
            this.fixture = new Fixture().Customize();
            this.validator = this.fixture.Create<AreaBreakdownQueryValidator>();
        }

        [Fact]
        public void Given_ValidAreaBreakdownQuery_When_Validating_Then_IsValid()
        {
            var query = this.fixture.Create<AreaBreakdownQuery>();

            ValidationResult validationResult = this.validator.Validate(query);

            validationResult.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public void Given_InvalidAreaBreakdownQuery_With_PropertyIdSetToEmptyGuid_When_Validating_Then_IsNotValid()
        {
            AreaBreakdownQuery query = this.fixture.Build<AreaBreakdownQuery>().With(x => x.PropertyId, Guid.Empty).Create();
            
            ValidationResult validationResult = this.validator.Validate(query);

            validationResult.IsInvalid(nameof(query.PropertyId), nameof(Messages.notempty_error));
        }
    }
}