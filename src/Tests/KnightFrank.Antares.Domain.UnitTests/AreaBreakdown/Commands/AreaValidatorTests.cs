namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.Commands
{
    using System.Linq;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("CreateAreaBreakdown")]
    public class AreaValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Fixture fixture;
        private readonly AreaValidator validator;

        public AreaValidatorTests()
        {
            this.fixture = new Fixture().Customize();
            this.validator = this.fixture.Create<AreaValidator>();
        }

        [Fact]
        public void Given_ValidCreateAreaBreakdownCommand_When_Validating_Then_IsValid()
        {
            var area = this.fixture.Create<Area>();

            ValidationResult validationResult = this.validator.Validate(area);

            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Given_InvalidArea_With_NameSetTo129Characters_When_Validating_Then_IsNotValid()
        {
            Area area = this.fixture.Build<Area>().With(x => x.Name, string.Join(string.Empty, Enumerable.Repeat('x', 129))).Create();

            ValidationResult validationResult = this.validator.Validate(area);

            validationResult.IsInvalid(nameof(area.Name), nameof(Messages.length_error));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Given_InvalidArea_With_IncorrectName_When_Validating_Then_IsNotValid(string name)
        {
            Area area = this.fixture.Build<Area>().With(x => x.Name, name).Create();

            ValidationResult validationResult = this.validator.Validate(area);

            validationResult.IsInvalid(nameof(area.Name), nameof(Messages.notempty_error));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Given_InvalidArea_With_IncorrectSize_When_Validating_Then_IsNotValid(double size)
        {
            Area area = this.fixture.Build<Area>().With(x => x.Size, size).Create();

            ValidationResult validationResult = this.validator.Validate(area);

            validationResult.IsInvalid(nameof(area.Size), nameof(Messages.greaterthan_error));
        }
    }
}