namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    public class CreateOwnershipCommandValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateOwnershipCommand command;
        private readonly CreateOwnershipCommandValidator validator;
        private readonly Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        
        public CreateOwnershipCommandValidatorTests()
        {
            IFixture fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.command = fixture.Build<CreateOwnershipCommand>()
                               .With(x => x.PurchaseDate, new DateTime(2000, 1, 1))
                               .With(x => x.SellDate, new DateTime(2010, 1, 1))
                               .With(x => x.BuyPrice, 1)
                               .With(x => x.SellPrice, 1)
                               .Create();

            this.enumTypeItemRepository = fixture.Freeze<Mock<IGenericRepository<EnumTypeItem>>>();
            this.enumTypeItemRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new EnumTypeItem()
            {
                EnumType = fixture.BuildEnumType("OwnershipType")
            });
            
            this.validator = fixture.Create<CreateOwnershipCommandValidator>();}

        [Fact]
        public void Given_CorrectCreateOwnershipCommand_When_Validating_Then_NoValidationErrors()
        {
            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_CorrectBuyPriceAndNoSellPrice_When_Validating_Then_NoValidationErrors()
        {
            this.command.SellPrice = null;

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_NoBuyPriceAndCorrectSellPrice_When_Validating_Then_NoValidationErrors()
        {
            this.command.BuyPrice = null;

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_NoBuyPriceAndNoSellPrice_When_Validating_Then_NoValidationErrors()
        {
            this.command.BuyPrice = null;
            this.command.SellPrice = null;

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_NoPurchaseDateAndCorrectSellDate_When_Validating_Then_NoValidationErrors()
        {
            this.command.PurchaseDate = null;

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_CorrectPurchaseDateAndNoSellDate_When_Validating_Then_NoValidationErrors()
        {
            this.command.SellDate = null;

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_NoPurchaseDateAndNoSellDate_When_Validating_Then_NoValidationErrors()
        {
            this.command.PurchaseDate = null;
            this.command.SellDate = null;

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_PurchaseDateGreatherThanSellDate_When_Validating_Then_ValidationErrors()
        {
            this.command.PurchaseDate = new DateTime(2002, 1, 1);
            this.command.SellDate = new DateTime(2001, 1, 1);

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_BuyPriceLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.BuyPrice = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.BuyPrice));
        }

        [Fact]
        public void Given_SellPriceLessThanZero_When_Validating_Then_ValidationErrors()
        {
            this.command.SellPrice = -1;

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.SellPrice));
        }

        [Fact]
        public void Given_NoContactsId_When_Validating_Then_ValidationErrors()
        {
            this.command.ContactIds.Clear();

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.ContactIds));
        }
        
        [Fact]
        public void Given_IncorrectOwnershipTypeId_When_Validating_Then_ValidationErrors()
        {
            this.enumTypeItemRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((EnumTypeItem)null);

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.OwnershipTypeId));
        }

        private static void TestIncorrectCommand(CreateOwnershipCommandValidator validator, CreateOwnershipCommand command,
            string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
