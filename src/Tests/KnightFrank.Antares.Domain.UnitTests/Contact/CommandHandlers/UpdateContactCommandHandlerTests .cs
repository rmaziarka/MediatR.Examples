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

    [Collection("UpdateContact")]
    [Trait("FeatureTitle", "Contacts")]
    public class UpdateContactCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {

        [Theory]
        [AutoMoqData]
        public void Given_UpdateContactCommand_When_Handle_Then_UpdateContact(
            UpdateContactCommand updateContactCommand,
            Guid expectedGuid,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IEntityValidator> enttyValidator,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            UpdateContactCommandHandler updateContactCommandHandler,
            ContactUser leadNegotiator,
            List<ContactUser> secondaryNegotiators,
            Contact contactToUpdate,
            Fixture fixture)
        {
            //Arrange
            updateContactCommand.LeadNegotiator = this.CreateContactUser(fixture, leadNegotiator.UserId);
            updateContactCommand.SecondaryNegotiators[0] = this.CreateContactUser(fixture, secondaryNegotiators[0].UserId);
            updateContactCommand.SecondaryNegotiators[1] = this.CreateContactUser(fixture, secondaryNegotiators[1].UserId);
            contactToUpdate.ContactUsers = this.CreateContactUsers(fixture);

            this.SetupContactRepository(contactRepository, contactToUpdate);
            this.SetupEnumTypeRepository(enumTypeItemRepository, fixture);

            this.SetupUserRepository(userRepository, updateContactCommand, fixture);

            //Act
            updateContactCommandHandler.Handle(updateContactCommand);

            // Asserts

            this.VerifyAllPropertiesMatch(contactToUpdate, updateContactCommand);
            this.VerifyIfValidationsWereCalled(updateContactCommand, enttyValidator, collectionValidator);

            contactRepository.Verify(r => r.Save(), Times.Once());
        }

        private List<ContactUser> CreateContactUsers(Fixture fixture)
        {
            return new List<ContactUser>
            {
                fixture.Build<ContactUser>().With(i => i.UserId, Guid.NewGuid()).Create()
            };
        }

        private void SetupEnumTypeRepository(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository, Fixture fixture)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                              .Returns(
                                                  (Expression<Func<EnumTypeItem, bool>> expr) =>
                                                      new[]
                                                      {
                                              this.CreateEnumTypeItem(UserType.LeadNegotiator.ToString(), fixture),
                                              this.CreateEnumTypeItem(UserType.SecondaryNegotiator.ToString(), fixture)
                                                      }.Where(
                                                          expr.Compile()));
        }

        private void SetupContactRepository(Mock<IGenericRepository<Contact>> contactRepository, Contact contactToUpdate)
        {
            contactRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<Expression<Func<Contact, object>>>())).Returns(new List<Contact> { contactToUpdate });
        }

        private ContactUserCommand CreateContactUser(Fixture fixture, Guid userId)
        {
            return fixture.Build<ContactUserCommand>().With(i => i.UserId, userId).Create();
        }

        private void SetupUserRepository(
            Mock<IGenericRepository<User>> userRepository,
            UpdateContactCommand command,
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

        private void VerifyAllPropertiesMatch(Contact addedContact, UpdateContactCommand updateContactCommand)
        {
            addedContact.Title.Should().Be(updateContactCommand.Title);
            addedContact.FirstName.Should().Be(updateContactCommand.FirstName);
            addedContact.LastName.Should().Be(updateContactCommand.LastName);

            addedContact.MailingFormalSalutation.Should().Be(updateContactCommand.MailingFormalSalutation);
            addedContact.MailingSemiformalSalutation.Should().Be(updateContactCommand.MailingSemiformalSalutation);
            addedContact.MailingInformalSalutation.Should().Be(updateContactCommand.MailingInformalSalutation);
            addedContact.MailingPersonalSalutation.Should().Be(updateContactCommand.MailingPersonalSalutation);
            addedContact.MailingEnvelopeSalutation.Should().Be(updateContactCommand.MailingEnvelopeSalutation);
            addedContact.DefaultMailingSalutationId.Should().Be(updateContactCommand.DefaultMailingSalutationId);

            addedContact.EventInviteSalutation.Should().Be(updateContactCommand.EventInviteSalutation);
            addedContact.EventSemiformalSalutation.Should().Be(updateContactCommand.EventSemiformalSalutation);
            addedContact.EventInformalSalutation.Should().Be(updateContactCommand.EventInformalSalutation);
            addedContact.EventPersonalSalutation.Should().Be(updateContactCommand.EventPersonalSalutation);
            addedContact.EventEnvelopeSalutation.Should().Be(updateContactCommand.EventEnvelopeSalutation);
            addedContact.DefaultEventSalutationId.Should().Be(updateContactCommand.DefaultEventSalutationId);
        }

        private EnumTypeItem CreateEnumTypeItem(string code, IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, code).Create();
        }

        private void VerifyIfValidationsWereCalled(UpdateContactCommand command, Mock<IEntityValidator> entityValidator, Mock<ICollectionValidator> collectionValidator)
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
