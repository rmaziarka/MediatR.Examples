namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Ownership.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    public class CreateOwnershipCommandDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateOwnershipCommand command;
        private readonly CreateOwnershipCommandDomainValidator validator;
        private readonly Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository;
        private readonly Mock<IGenericRepository<Ownership>> ownershipRepository;
        private readonly Mock<IGenericRepository<Property>> propertyRepository;
        private readonly Mock<IGenericRepository<Contact>> contactRepository;

        private readonly IFixture fixture;
        
        public CreateOwnershipCommandDomainValidatorTests()
        {
            this.fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            this.fixture.Behaviors.Clear();
            this.fixture.RepeatCount = 1;
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.command = this.fixture.Build<CreateOwnershipCommand>()
                               .With(x => x.PurchaseDate, new DateTime(2000, 1, 1))
                               .With(x => x.SellDate, new DateTime(2010, 1, 1))
                               .Create();

            this.enumTypeItemRepository = this.fixture.Freeze<Mock<IGenericRepository<EnumTypeItem>>>();
            this.ownershipRepository = this.fixture.Freeze<Mock<IGenericRepository<Ownership>>>();

            this.enumTypeItemRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new EnumTypeItem()
            {
                EnumType = this.fixture.BuildEnumType("OwnershipType")
            });

            this.propertyRepository = this.fixture.Freeze<Mock<IGenericRepository<Property>>>();
            this.propertyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new Property());

            this.contactRepository = this.fixture.Freeze<Mock<IGenericRepository<Contact>>>();
            this.contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(new List<Contact>()
            {
                new Contact()
            }.AsQueryable());

            this.validator = this.fixture.Create<CreateOwnershipCommandDomainValidator>();
        }

        [Fact]
        public void Given_ClosedPeriodsDoNotOverlap_When_Validating_Then_NoValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>()))
                .Returns(new List<Ownership>()
                {
                    this.fixture.BuildOwnership(new DateTime(1980, 1, 1), new DateTime(1990, 1, 1)),
                    this.fixture.BuildOwnership(new DateTime(2010, 1, 1), new DateTime(2015, 1, 1))
                });

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_OpenPreviousPeriodDoNotOverlap_When_Validating_Then_NoValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>()))
                               .Returns(new List<Ownership>()
                               {
                                   this.fixture.BuildOwnership(null, new DateTime(1990, 1, 1))
                               }.AsQueryable());

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_OpenNextPeriodDoNotOverlap_When_Validating_Then_NoValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(new DateTime(2010, 1, 1))
            }.AsQueryable());

            ValidationResult validationResult = this.validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Given_OpenPreviousPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(null, new DateTime(2005, 1, 1))
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_OpenNextPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(new DateTime(2005, 1, 1))
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_OpenPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership()
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_ClosedPreviousPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(new DateTime(1990, 1, 1), new DateTime(2005, 1, 1))
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_ClosedNextPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(new DateTime(2005, 1, 1), new DateTime(2020, 1, 1))
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_ClosedInternalPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(new DateTime(2006, 1, 1), new DateTime(2007, 1, 1))
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_ClosedExternalPeriodOverlap_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>())).Returns(new List<Ownership>()
            {
                this.fixture.BuildOwnership(new DateTime(1999, 1, 1), new DateTime(2011, 1, 1))
            }.AsQueryable());

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PurchaseDate));
        }

        [Fact]
        public void Given_IncorrectOwnershipType_When_Validating_Then_ValidationErrors()
        {
            this.ownershipRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Ownership, bool>>>()))
                .Returns(new List<Ownership>()
                {
                    this.fixture.BuildOwnership(new DateTime(1980, 1, 1), new DateTime(1990, 1, 1)),
                    this.fixture.BuildOwnership(new DateTime(2010, 1, 1), new DateTime(2015, 1, 1))
                });

            this.enumTypeItemRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(new EnumTypeItem()
            {
                EnumType = new EnumType() { Code = this.fixture.Create<string>() }
            });

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.OwnershipTypeId));
        }

        [Fact]
        public void Given_IncorrectContactIds_When_Validating_Then_ValidationErrors()
        {
            var results = this.command.ContactIds.Select(x => new Contact()).ToList();
            results.Add(new Contact());

            this.contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(results);

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.ContactIds));
        }

        [Fact]
        public void Given_IncorrectPropertyId_When_Validating_Then_ValidationErrors()
        {
            this.propertyRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((Property)null);

            TestIncorrectCommand(this.validator, this.command, nameof(this.command.PropertyId));
        }

        [Fact]
        public void Given_CorrectPropertyId_When_Validating_Then_NoValidationErrors()
        {
            Expression<Func<IGenericRepository<Property>, Property>> expression = x => x.GetById(this.command.PropertyId);
            this.propertyRepository.Setup(expression)
                .Returns(new Property() { Id = this.command.PropertyId });

            ValidationResult validationResult = this.validator.Validate(this.command);

            this.propertyRepository.Verify(expression, Times.Once);
            Assert.True(validationResult.IsValid);
        }

        private static void TestIncorrectCommand(CreateOwnershipCommandDomainValidator validator, CreateOwnershipCommand command,
            string testedPropertyName)
        {
            ValidationResult validationResult = validator.Validate(command);
            Assert.False(validationResult.IsValid);

            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}
