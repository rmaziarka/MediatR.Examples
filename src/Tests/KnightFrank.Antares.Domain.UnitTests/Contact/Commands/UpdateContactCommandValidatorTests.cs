namespace KnightFrank.Antares.Domain.UnitTests.Contact.Commands
{
    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Contact.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Ploeh.AutoFixture;

    using Xunit;

    [Collection("UpdateContactCommandValidator")]
    [Trait("FeatureTitle", "Contacts")]
    public class UpdateContactCommandValidatorTests
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectUpdateContactCommand_When_Validating_Then_NoValidationErrors(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            ValidationResult validationResult = validator.Validate(command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithEmptyTitle_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.Title = string.Empty;

            TestIncorrectCommand(validator, command, nameof(command.Title));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongTitle_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.Title = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.Title));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongFirstName_When_Validating_Then_ValidationErrors(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.FirstName = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.FirstName));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithEmptyLastName_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.LastName = string.Empty;

            TestIncorrectCommand(validator, command, nameof(command.LastName));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongLastName_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.LastName = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.LastName));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongMailingFormalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.MailingFormalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.MailingFormalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongMailingSemiformalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.MailingSemiformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.MailingSemiformalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongMailingInformalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.MailingInformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.MailingInformalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongMailingPersonalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.MailingPersonalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.MailingPersonalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongMailingEnvelopeSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.MailingEnvelopeSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.MailingEnvelopeSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongEventInviteSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.EventInviteSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.EventInviteSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongEventSemiformalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.EventSemiformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.EventSemiformalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongEventInformalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.EventInformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.EventInformalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongEventPersonalSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.EventPersonalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.EventPersonalSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithTooLongEventEnvelopeSalutation_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            Fixture fixture)
        {
            command.EventEnvelopeSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.EventEnvelopeSalutation));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectUpdateContactCommandWithNoLeadNegotiator_When_Validating_Then_ValidationError(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command)
        {
            command.LeadNegotiator = null;

            TestIncorrectCommand(validator, command, nameof(command.LeadNegotiator));
        }

        private static void TestIncorrectCommand(
            UpdateContactCommandValidator validator,
            UpdateContactCommand command,
            string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}