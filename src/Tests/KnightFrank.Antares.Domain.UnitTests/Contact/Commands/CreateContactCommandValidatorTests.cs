namespace KnightFrank.Antares.Domain.UnitTests.Contact.Commands
{
	using System;

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
		[InlineAutoData("")]
		[InlineAutoData((string) null)]
		public void Given_IncorrectCreateContactCommandWithEmptyTitle_When_Validating_Then_ValidationError(string value, CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.Title = value;

			TestIncorrectCommand(validator, command, nameof(command.Title));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongTitle_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.Title = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.Title));
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
		[InlineAutoData((string) null)]
		public void Given_IncorrectCreateContactCommandWithEmptyLastName_When_Validating_Then_ValidationError(string value, CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.LastName = value;

			TestIncorrectCommand(validator, command, nameof(command.LastName));
		}

		[Theory]
        [AutoData]
        public void Given_IncorrectCreateContactCommandWithTooLongLastName_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
        {
            command.LastName = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

            TestIncorrectCommand(validator, command, nameof(command.LastName));
        }

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongMailingFormalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.MailingFormalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.MailingFormalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongMailingSemiformalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.MailingSemiformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.MailingSemiformalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongMailingInformalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.MailingInformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.MailingInformalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongMailingPersonalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.MailingPersonalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.MailingPersonalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongMailingEnvelopeSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.MailingEnvelopeSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.MailingEnvelopeSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongEventInviteSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.EventInviteSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.EventInviteSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongEventSemiformalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.EventSemiformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.EventSemiformalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongEventInformalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.EventInformalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.EventInformalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongEventPersonalSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.EventPersonalSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.EventPersonalSalutation));
		}

		[Theory]
		[AutoData]
		public void Given_IncorrectCreateContactCommandWithTooLongEventEnvelopeSalutation_When_Validating_Then_ValidationError(CreateContactCommandValidator validator, CreateContactCommand command, Fixture fixture)
		{
			command.EventEnvelopeSalutation = string.Join(string.Empty, fixture.CreateMany<string>(5)).Substring(0, 129);

			TestIncorrectCommand(validator, command, nameof(command.EventEnvelopeSalutation));
		}

		private static void TestIncorrectCommand(CreateContactCommandValidator validator, CreateContactCommand command, string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
}}