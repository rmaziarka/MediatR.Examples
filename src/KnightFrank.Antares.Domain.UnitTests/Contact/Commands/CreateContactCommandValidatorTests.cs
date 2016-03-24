namespace KnightFrank.Antares.Domain.UnitTests.Contact.Commands
{
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Contact.Commands;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateContactCommandValidator")]
    [Trait("FeatureTitle", "Contacts")]
    public class CreateContactCommandValidatorTests
    {
        [Theory]
        [AutoData]
        public void Given_CorrectCreateContactCommand_When_Validating_Then_NoValidationErrors(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            ValidationResult validationResult = validator.Validate(command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateContactCommandWithTooLongFirstName_When_Validating_Then_ValidationErrors(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.FirstName = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.FirstName));
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_IncorrectCreateContactCommandWithEmptyFirstName_When_Validating_Then_ValidationErrors(string value, CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.FirstName = value;

            TestIncorrectCommand(validator, command, nameof(command.FirstName));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateContactCommandWithTooLongTitle_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.Title = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.Title));
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_IncorrectCreateContactCommandWithEmptyTitle_When_Validating_Then_ValidationError(string value, CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.Title = value;

            TestIncorrectCommand(validator, command, nameof(command.Title));
        }

        [Theory]
        [AutoData]
        public void Given_IncorrectCreateContactCommandWithTooLongSurname_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.Surname = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.Surname));
        }

        [Theory]
        [InlineAutoData("")]
        [InlineAutoData((string)null)]
        public void Given_IncorrectCreateContactCommandWithEmptySurname_When_Validating_Then_ValidationError(string value, CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.Surname = value;

            TestIncorrectCommand(validator, command, nameof(command.Surname));
        }

        private static void TestIncorrectCommand(CreateContactCommandValidator validator, CreateContactCommand command, string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
}}