namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateOwnershipCommandDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateOwnershipCommand command;

        public CreateOwnershipCommandDomainValidatorTests()
        {
            Fixture fixture = new Fixture();
            fixture.Customize<CreateOwnershipCommand>(c => c
                .With(x => x.PurchaseDate, new DateTime(2000, 1, 1))
                .With(x => x.SellDate, new DateTime(2010, 1, 1))
                );

            this.command = fixture.Create<CreateOwnershipCommand>();
        }
        
        [Theory]
        [AutoMoqData]
        public void Given_ClosedPeriodsDoNotOverlap_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership,bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
                {
                    PurchaseDate = new DateTime(1980,1,1),
                    SellDate = new DateTime(1990,1,1)
                },
                new Ownership()
                {
                    PurchaseDate = new DateTime(2010,1,1),
                    SellDate = new DateTime(2015,1,1)
                }
            }.AsQueryable());

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_OpenPreviousPeriodDoNotOverlap_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
                {
                    SellDate = new DateTime(1990,1,1)
                }
            }.AsQueryable());

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_OpenNextPeriodDoNotOverlap_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
                {
                    PurchaseDate = new DateTime(2010,1,1)
                },
            }.AsQueryable());

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_OpenPreviousPeriodOverlap_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
                {
                    SellDate = new DateTime(2005,1,1)
                },
            }.AsQueryable());

            TestIncorrectCommand(validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Theory]
        [AutoMoqData]
        public void Given_OpenNextPeriodOverlap_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
                {
                    PurchaseDate = new DateTime(2005,1,1)
                },
            }.AsQueryable());

            TestIncorrectCommand(validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Theory]
        [AutoMoqData]
        public void Given_OpenPeriodOverlap_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
            }.AsQueryable());

            TestIncorrectCommand(validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ClosedPreviousPeriodOverlap_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership() {
                    PurchaseDate = new DateTime(1990,1,1),
                    SellDate = new DateTime(2005,1,1)
                }
            }.AsQueryable());

            TestIncorrectCommand(validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Theory]
        [AutoMoqData]
        public void Given_ClosedNextPeriodOverlap_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            CreateOwnershipCommandDomainValidator validator)
        {
            ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                new Ownership()
                {
                    PurchaseDate = new DateTime(2005,1,1),
                    SellDate = new DateTime(2020,1,1)
                }
            }.AsQueryable());

            TestIncorrectCommand(validator, this.command, nameof(this.command.PurchaseDate));
        }

        private static void TestIncorrectCommand(CreateOwnershipCommandDomainValidator validator, CreateOwnershipCommand command, string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
