namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;

    using FluentValidation.Results;
    
    using KnightFrank.Antares.Domain.Ownership.Commands;
    
    using Ploeh.AutoFixture;

    using Xunit;

    public class CreateOwnershipCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateOwnershipCommand command;

        public CreateOwnershipCommandValidatorTests()
        {
            Fixture fixture = new Fixture();
            fixture.Customize<CreateOwnershipCommand>(c => c
                .With(x => x.PurchaseDate, new DateTime(2000, 1, 1))
                .With(x => x.SellDate, new DateTime(2010, 1, 1))
                .With(x => x.BuyPrice, 1)
                .With(x => x.SellPrice, 1)
                );

            this.command = fixture.Create<CreateOwnershipCommand>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectCreateOwnershipCommand_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectBuyPriceAndNoSellPrice_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.SellPrice = null;

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NoBuyPriceAndCorrectSellPrice_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.BuyPrice = null;

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NoBuyPriceAndNoSellPrice_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.BuyPrice = null;
            this.command.SellPrice = null;

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NoPurchaseDateAndCorrectSellDate_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.PurchaseDate = null;

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectPurchaseDateAndNoSellDate_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.SellDate = null;

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_NoPurchaseDateAndNoSellDate_When_Validating_Then_NoValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.PurchaseDate = null;
            this.command.SellDate = null;

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_PurchaseDateGreatherThanSellDate_When_Validating_Then_ValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.PurchaseDate = new DateTime(2002, 1, 1);
            this.command.SellDate = new DateTime(2001, 1, 1);

            TestIncorrectCommand(validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Theory]
        [AutoMoqData]
        public void Given_BuyPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.BuyPrice = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.BuyPrice));
        }

        [Theory]
        [AutoMoqData]
        public void Given_SellPriceLessThanZero_When_Validating_Then_ValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.SellPrice = -1;

            TestIncorrectCommand(validator, this.command, nameof(this.command.SellPrice));
        }


        [Theory]
        [AutoMoqData]
        public void Given_NoContactsId_When_Validating_Then_ValidationErrors(CreateOwnershipCommandValidator validator)
        {
            this.command.ContactIds.Clear();

            TestIncorrectCommand(validator, this.command, nameof(this.command.ContactIds));
        }

        private static void TestIncorrectCommand(CreateOwnershipCommandValidator validator, CreateOwnershipCommand command, string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
