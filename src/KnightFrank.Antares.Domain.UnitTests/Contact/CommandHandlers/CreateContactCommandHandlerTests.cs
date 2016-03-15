namespace KnightFrank.Antares.Domain.UnitTests.Contact.CommandHandlers
{
    using System;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Contact.CommandHandlers;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("Mappings")]
    [Trait("FeatureTitle", "Mappings")]
    public class CreateContactCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CreateContactCommand_When_Handle_Then_ShouldCreateContact(
            CreateContactCommand createContactCommand,
            Guid expectedGuid,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            CreateContactCommandHandler createContactCommandHandler)
        {
            // Arrange
            Contact addedContact = null;
            contactRepository.Setup(r => r.Add(It.IsAny<Contact>())).Returns(
                (Contact contact) =>
                    {
                        addedContact = contact;
                        return addedContact;
                    });

            contactRepository.Setup(r => r.Save()).Callback(() => { addedContact.Id = expectedGuid; });

            //Act
            Guid guid = createContactCommandHandler.Handle(createContactCommand);

            // Asserts
            guid.Should().Be(expectedGuid);
            addedContact.Should().NotBeNull();
            addedContact.FirstName.Should().Be(createContactCommand.FirstName);
            addedContact.Surname.Should().Be(createContactCommand.Surname);
            addedContact.Title.Should().Be(createContactCommand.Title);
            contactRepository.Verify(r => r.Add(It.IsAny<Contact>()), Times.Once());
            contactRepository.Verify(r => r.Save(), Times.Once());
        }
    }
}
