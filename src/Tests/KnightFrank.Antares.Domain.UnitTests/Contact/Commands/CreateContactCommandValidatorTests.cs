namespace KnightFrank.Antares.Domain.UnitTests.Contact.Commands
{
    using FluentValidation.Validators;

    using KnightFrank.Antares.Domain.Contact.Commands;
    using KnightFrank.Antares.Tests.Common.Extension.Fluent.RulesVerifier;

    using Xunit;

    [Collection("CreateContactCommandValidator")]
    [Trait("FeatureTitle", "Contacts")]
    public class CreateContactCommandValidatorTests
    {
        [Fact]
        public void Given_When_CreateInstance_Then_ShouldHaveCorrectRules()
        {
            // Act
            var commandValidator = new CreateContactCommandValidator();

            // Assert
            commandValidator.ShouldHaveRulesCount(3);

            commandValidator.ShouldHaveRules(x => x.FirstName,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotEmptyValidator>()
                                     .AddPropertyValidatorVerifier<LengthValidator>(1, 128)
                                     .Create());

            commandValidator.ShouldHaveRules(x => x.Surname,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotEmptyValidator>()
                                     .AddPropertyValidatorVerifier<LengthValidator>(1, 128)
                                     .Create());

            commandValidator.ShouldHaveRules(x => x.Title,
                RuleVerifiersComposer.Build()
                                     .AddPropertyValidatorVerifier<NotEmptyValidator>()
                                     .AddPropertyValidatorVerifier<LengthValidator>(1, 128)
                                     .Create());
        }
    }
}
