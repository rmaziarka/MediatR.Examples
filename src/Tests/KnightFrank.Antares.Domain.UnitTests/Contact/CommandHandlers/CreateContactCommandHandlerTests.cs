namespace KnightFrank.Antares.Domain.UnitTests.Contact.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Contact.CommandHandlers;
    using KnightFrank.Antares.Domain.Contact.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateContact")]
    [Trait("FeatureTitle", "Contacts")]
    public class CreateContactCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {

        [Theory]
        [AutoMoqData]
        public void Given_CreateContactCommand_When_Handle_Then_CreateContact(
            CreateContactCommand createContactCommand,
            Guid expectedGuid,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IEntityValidator> enttyValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            CreateContactCommandHandler createContactCommandHandler,
            ContactUser leadNegotiator,
            List<ContactUser> secondaryNegotiators,
            Fixture fixture)
        {
            //Arrange
            createContactCommand.LeadNegotiator = this.CreateContactUser(fixture, leadNegotiator.UserId);
            createContactCommand.SecondaryNegotiators[0] = this.CreateContactUser(fixture, secondaryNegotiators[0].UserId);
            createContactCommand.SecondaryNegotiators[1] = this.CreateContactUser(fixture, secondaryNegotiators[1].UserId);

            Contact addedContact = null;

            contactRepository.Setup(r => r.Add(It.IsAny<Contact>())).Returns(
                (Contact contact) =>
                {
                    addedContact = contact;
                    return addedContact;
                });

            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns(
                                      (Expression<Func<EnumTypeItem, bool>> expr) =>
                                          new[]
                                          {
                                              this.CreateEnumTypeItem(UserType.LeadNegotiator.ToString(), fixture),
                                              this.CreateEnumTypeItem(UserType.SecondaryNegotiator.ToString(), fixture)
                                          }.Where(
                                              expr.Compile()));

           this.SetupUserRepository(userRepository, createContactCommand, fixture);
          
            contactRepository.Setup(r => r.Save()).Callback(() => { addedContact.Id = expectedGuid; });


            //Act
            Guid guid = createContactCommandHandler.Handle(createContactCommand);

            // Asserts
            guid.Should().Be(expectedGuid);
            addedContact.Should().NotBeNull();

            this.VerifyAllPropertiesMatch(addedContact, createContactCommand);
            this.VerifyIfValidationsWereCalled(createContactCommand, enttyValidator, collectionValidator);

            contactRepository.Verify(r => r.Add(It.IsAny<Contact>()), Times.Once());
            contactRepository.Verify(r => r.Save(), Times.Once());
        }

       private ContactUserCommand CreateContactUser(Fixture fixture, Guid userId)
        {
            return fixture.Build<ContactUserCommand>().With(i => i.UserId, userId).Create();
        }

        private void SetupUserRepository(
            Mock<IGenericRepository<User>> userRepository,
            CreateContactCommand command,
            IFixture fixture)
        {
            var userIds = new List<Guid> { command.LeadNegotiator.UserId };
            foreach (ContactUserCommand updateActivityUser in command.SecondaryNegotiators)
            {
                userIds.Add(updateActivityUser.UserId);
            }

            var availableUsers = new List<User>();
            userIds.Distinct().ToList().ForEach(userId => availableUsers.Add(this.CreateUser(userId, fixture)));

            userRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                          .Returns(
                              (Expression<Func<User, bool>> expr, Expression<Func<User, object>>[] expr2) =>
                                  availableUsers.Where(expr.Compile()));
        }
        
        private User CreateUser(Guid userId, IFixture fixture)
        {
            return fixture.Build<User>()
                .With(x => x.Id, userId)
                .Create();
        }

        private void VerifyAllPropertiesMatch(Contact addedContact, CreateContactCommand createContactCommand)
        {
            addedContact.Title.Should().Be(createContactCommand.Title);
            addedContact.FirstName.Should().Be(createContactCommand.FirstName);
            addedContact.LastName.Should().Be(createContactCommand.LastName);

            addedContact.MailingFormalSalutation.Should().Be(createContactCommand.MailingFormalSalutation);
            addedContact.MailingSemiformalSalutation.Should().Be(createContactCommand.MailingSemiformalSalutation);
            addedContact.MailingInformalSalutation.Should().Be(createContactCommand.MailingInformalSalutation);
            addedContact.MailingPersonalSalutation.Should().Be(createContactCommand.MailingPersonalSalutation);
            addedContact.MailingEnvelopeSalutation.Should().Be(createContactCommand.MailingEnvelopeSalutation);
            addedContact.DefaultMailingSalutationId.Should().Be(createContactCommand.DefaultMailingSalutationId);

            addedContact.EventInviteSalutation.Should().Be(createContactCommand.EventInviteSalutation);
            addedContact.EventSemiformalSalutation.Should().Be(createContactCommand.EventSemiformalSalutation);
            addedContact.EventInformalSalutation.Should().Be(createContactCommand.EventInformalSalutation);
            addedContact.EventPersonalSalutation.Should().Be(createContactCommand.EventPersonalSalutation);
            addedContact.EventEnvelopeSalutation.Should().Be(createContactCommand.EventEnvelopeSalutation);
            addedContact.DefaultEventSalutationId.Should().Be(createContactCommand.DefaultEventSalutationId);
        }

        private EnumTypeItem CreateEnumTypeItem(string code, IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, code).Create();
        }

        private void VerifyIfValidationsWereCalled(CreateContactCommand command, Mock<IEntityValidator> entityValidator, Mock<ICollectionValidator> collectionValidator)
        {
            entityValidator.Verify(x => x.EntityExists<User>(command.LeadNegotiator.UserId), Times.Once);
            entityValidator.Verify(x => x.EntitiesExist<User>(It.Is<List<Guid>>(list => list.SequenceEqual(command.SecondaryNegotiators.Select(n => n.UserId).ToList()))), Times.Once);

            List<Guid> expectedNegotiators = command.SecondaryNegotiators.Select(n => n.UserId).ToList();
            expectedNegotiators.Add(command.LeadNegotiator.UserId);

            collectionValidator.Verify(
                x => x.CollectionIsUnique(
                        It.Is<ICollection<Guid>>(list => list.OrderBy(i => i).SequenceEqual(expectedNegotiators.OrderBy(i => i))),
                        ErrorMessage.Negotiators_Not_Unique), Times.Once);
        }
    }
}
