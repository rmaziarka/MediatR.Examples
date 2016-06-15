namespace KnightFrank.Antares.Domain.UnitTests.Activity.Commands
{
    using FluentAssertions;

    using FluentValidation.Resources;
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Xunit;

    using System.Linq;

    using FluentValidation.Validators;

    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;
    using KnightFrank.Antares.Tests.Common.Extensions.Fluent.RulesVerifier;

    [Trait("FeatureTitle", "Property Activity")]
    [Collection("UpdateActivityCommandValidator")]
    public class UpdateActivityCommandValidatorTests
    {
        private readonly UpdateActivityCommandValidator commandValidator = new UpdateActivityCommandValidator();

        [Fact]
        public void Given_When_CreateInstance_Then_ShouldHaveCorrectRules()
        {
            // Assert
            this.commandValidator.ShouldHaveRulesCount(9);

            this.commandValidator.ShouldHaveRules(x => x.MarketAppraisalPrice,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>(0)
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.RecommendedPrice,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>(0)
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.VendorEstimatedPrice,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<GreaterThanOrEqualValidator>(0)
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.ActivityStatusId,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotEmptyValidator>()
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.ActivityTypeId,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotEmptyValidator>()
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.LeadNegotiator,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotNullValidator>()
                                     .AddChildValidatorVerifier<UpdateActivityUserValidator>()
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.LeadNegotiator.CallDate,
                RuleVerifiersComposer.Build()
                                     .AddCustomVerifier()
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.SecondaryNegotiators,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotNullValidator>()
                                     .AddChildCollectionValidatorVerifier<UpdateActivityUserValidator>()
                                     .Create());

            this.commandValidator.ShouldHaveRules(x => x.Departments,
                    RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotNullValidator>()
                                     .AddChildCollectionValidatorVerifier<UpdateActivityDepartmentValidator>()
                                     .Create());
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidUpdateActivityCommand_When_SecondaryNegotiatorCallDateIsNull_Validating_Then_IsValid(
            UpdateActivityCommand command)
        {
            // Arrange
            command.SecondaryNegotiators.First().CallDate = null;

            // Act
            ValidationResult validationResult = this.commandValidator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdateActivityCommand_When_LeadNegotiatorCallDateIsNull_Validating_Then_IsNotValid(
            UpdateActivityCommand command)
        {
            // Arrange
            command.LeadNegotiator.CallDate = null;

            // Act
            ValidationResult validationResult = this.commandValidator.Validate(command);

            // Assert
            validationResult.IsInvalid(nameof(command.LeadNegotiator) + "." + nameof(command.LeadNegotiator.CallDate),
                nameof(Messages.notnull_error));
        }
    }
}
