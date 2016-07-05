namespace KnightFrank.Antares.Domain.UnitTests.Contact.Commands
{
	using System;

	using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Contact.Commands;
	using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

	using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("UpdateContactCommandValidatorTests")]
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
        public void Given_IncorrectCreateContactCommandWithNoLeadNegotiator_When_Validating_Then_ValidationError(
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
}}