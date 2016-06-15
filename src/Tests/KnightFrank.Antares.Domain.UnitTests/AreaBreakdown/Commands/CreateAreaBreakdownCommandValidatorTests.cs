namespace KnightFrank.Antares.Domain.UnitTests.AreaBreakdown.Commands
{
    using System;

    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;
    using FluentValidation.TestHelper;

    using KnightFrank.Antares.Domain.AreaBreakdown.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.ValidationResult;

    using Ploeh.AutoFixture;

    using Xunit;

    [Trait("FeatureTitle", "AreaBreakdown")]
    [Collection("CreateAreaBreakdown")]
    public class CreateAreaBreakdownCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Fixture fixture;
        private readonly CreateAreaBreakdownCommandValidator validator;

        public CreateAreaBreakdownCommandValidatorTests()
        {
            this.fixture = new Fixture().Customize();
            this.validator = this.fixture.Create<CreateAreaBreakdownCommandValidator>();
        }
        
        [Fact]
        public void Given_ValidCreateAreaBreakdownCommand_When_Validating_Then_IsValid()
        {
            var command = this.fixture.Create<CreateAreaBreakdownCommand>();

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Given_CreateAreaBreakdownCommandValidator_When_Validating_AreasHaveConfiguredValidator()
        {
            this.validator.ShouldHaveChildValidator(x => x.Areas, typeof(AreaValidator));
        }

        [Fact]
        public void Given_InvalidCreateAreaBreakdownCommand_With_PropertyIdSetToEmptyGuid_When_Validating_Then_IsNotValid()
        {
            CreateAreaBreakdownCommand command =
                this.fixture.Build<CreateAreaBreakdownCommand>().With(x => x.PropertyId, Guid.Empty).Create();

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsInvalid(nameof(command.PropertyId), nameof(Messages.notempty_error));
        }

        [Fact]
        public void Given_InvalidCreateAreaBreakdownCommand_With_AreasSetToNull_When_Validating_Then_IsNotValid()
        {
            CreateAreaBreakdownCommand command = this.fixture.Build<CreateAreaBreakdownCommand>().With(x => x.Areas, null).Create();

            ValidationResult validationResult = this.validator.Validate(command);

            validationResult.IsInvalid(nameof(command.Areas), nameof(Messages.notnull_error));
        }
    }
}