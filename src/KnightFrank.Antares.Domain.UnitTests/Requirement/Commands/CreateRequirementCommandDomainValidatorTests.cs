namespace KnightFrank.Antares.Domain.UnitTests.Requirement.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact;
    using KnightFrank.Antares.Domain.Requirement.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateRequirementCommandDomainValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateRequirementCommand command;

        public CreateRequirementCommandDomainValidatorTests()
        {
            var fixture = new Fixture();
            this.command = fixture.Create<CreateRequirementCommand>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_ContactListWithNonExistingGuid_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<CreateRequirementCommand> createRequirementCommand,
            CreateRequirementCommandDomainValidator validator)
        {
            this.command.Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Id = Guid.Empty
                }
            };

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(new List<Contact>().AsQueryable());

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ContactListWithDifferentCount_When_Validating_Then_ValidationErrors(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<CreateRequirementCommand> createRequirementCommand,
            CreateRequirementCommandDomainValidator validator)
        {
            Guid id = Guid.NewGuid();

            this.command.Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Id = id
                },
                new ContactDto
                {
                    Id = Guid.Empty
                }
            };

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(new List<Contact>
                             {
                                 new Contact { Id = id }
                             }.AsQueryable());

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.False(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_ContactListWithGuidInDatabase_When_Validating_Then_NoValidationErrors(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            CreateRequirementCommandDomainValidator validator)
        {
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();

            this.command.Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Id = id
                },
                new ContactDto
                {
                    Id = id2
                }
            };

            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>()))
                             .Returns(new List<Contact>
                             {
                                 new Contact { Id = id },
                                 new Contact { Id = id2 }
                             }.AsQueryable());

            ValidationResult validationResult = validator.Validate(this.command);

            Assert.True(validationResult.IsValid);
        }
    }
}
