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
    using Ploeh.AutoFixture.Dsl;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using ErrorMessage = KnightFrank.Antares.Domain.Common.BusinessValidators.ErrorMessage;

    [Trait("FeatureTitle", "Contact")]
    [Collection("UpdateContactCommandHandler")]
    public class UpdateContactCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
      
        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_Handling_Then_ContactUpdated(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            UpdateContactCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
           UpdateContactCommand command = this.CreateUpdatContactCommand(fixture);
            Contact contact = this.GetContact(fixture);
            this.SetupContact(contact, fixture);

            contactRepository.Setup(x => x.GetWithInclude(
                It.IsAny<Expression<Func<Contact, bool>>>(),
                It.IsAny<Expression<Func<Contact, object>>[]>())).Returns(new List<Contact> { contact });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);
            this.SetupUserRepository(userRepository, command, contact, fixture);
            
            // Act
            Guid updatedGuid =  handler.Handle(command);
            updatedGuid.ShouldBeEquivalentTo(contact.Id);

            // Assert
            this.VerifyIfValidationsWereCalled(contact, command, entityValidator, collectionValidator);
          
            contactRepository.Verify(r => r.Save(), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_ValidCommand_When_DeletingSecondaryNegotiator_Then_ShouldBeMarkedAsDeleted(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<ContactUser>> contactUserRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            ContactUser leadNegotiator,
            UpdateContactCommandHandler handler,
            ContactUser secondaryNegotiatorToDelete,
            IFixture fixture)
        {
            // Arrange
            UpdateContactCommand command = this.CreateUpdatContactCommand(fixture);
            Contact contact = this.GetContact(fixture, leadNegotiator);

            secondaryNegotiatorToDelete.UserType = this.GetSecondaryNegotiatorUserType(fixture);
            contact.ContactUsers.Add(secondaryNegotiatorToDelete);
            this.SetupContact(contact, fixture);

            contactRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<Expression<Func<Contact, object>>[]>()))
                             .Returns(new List<Contact> { contact });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);
            this.SetupUserRepository(userRepository, command, contact, fixture);

            // Act
            Guid updatedGuid = handler.Handle(command);

            // Assert
            updatedGuid.ShouldBeEquivalentTo(contact.Id);

            contactUserRepository.Verify(r => r.Delete(secondaryNegotiatorToDelete), Times.Once);
            contactRepository.Verify(r => r.Save(), Times.Once());
        }

        [AutoMoqData]
        public void Given_ValidCommand_When_AddingSecondaryNegotiator_Then_ShouldBeSaved(
           [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
           [Frozen] Mock<IGenericRepository<User>> userRepository,
           [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
           ContactUser leadNegotiator,
           UpdateContactCommandHandler handler,
           Guid secondaryNegotiatorIdToAdd,
           IFixture fixture)
        {

            // Arrange
            UpdateContactCommand command = this.CreateUpdatContactCommand(fixture);
            Contact contact = this.GetContact(fixture, leadNegotiator);
            this.SetupContact(contact, fixture);

            command.SecondaryNegotiators.Add(new ContactUserCommand { UserId = secondaryNegotiatorIdToAdd });

            contactRepository.Setup(p => p.GetWithInclude(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<Expression<Func<Contact, object>>[]>())).Returns(new List<Contact> { contact });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);
            this.SetupUserRepository(userRepository, command, contact, fixture);

            // Act
            Guid updatedGuid = handler.Handle(command);

            // Assert
            updatedGuid.ShouldBeEquivalentTo(contact.Id);

            ContactUser negotiator = contact.ContactUsers.SingleOrDefault(u => u.UserId == secondaryNegotiatorIdToAdd);
            negotiator.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should().Be(UserType.SecondaryNegotiator.ToString());

           contactRepository.Verify(r => r.Save(), Times.Once());

        }

        [Theory]
        [InlineAutoMoqData(false, true)]
        [InlineAutoMoqData(false, false)]
        [InlineAutoMoqData(true, true)]
        [InlineAutoMoqData(true, false)]
        public void Given_ValidCommand_When_UpdatingNegotiator_Then_ShouldBeUpdated_WithProperUserType(
            bool isLeadNegotiatorToBeChanged,
            bool isNegotiatorTypeToChange,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository,
            ContactUser leadNegotiator,
            List<ContactUser> secondaryNegotiators,
            UpdateContactCommandHandler handler,
            Guid negotiatorIdToUpdate,
            IFixture fixture)
        {

            // Arrange
            bool isNegotiatorToBeChangedToLead = (isLeadNegotiatorToBeChanged && !isNegotiatorTypeToChange) ||
                                                 (!isLeadNegotiatorToBeChanged && isNegotiatorTypeToChange);

            UpdateContactCommand command = this.CreateUpdatContactCommand(fixture);
            Contact contact = this.GetContact(fixture, leadNegotiator, secondaryNegotiators);
            this.SetupContact(contact, fixture);

            if (isNegotiatorToBeChangedToLead)
            {
                command.LeadNegotiator = this.CreateUpdateContactUser(fixture, negotiatorIdToUpdate).Create();
            }
            else
            {
                command.SecondaryNegotiators.Add(this.CreateUpdateContactUser(fixture, negotiatorIdToUpdate).Create());
            }

            if (isLeadNegotiatorToBeChanged)
            {
                leadNegotiator.UserId = negotiatorIdToUpdate;
            }
            else
            {
                secondaryNegotiators[1].UserId = negotiatorIdToUpdate;
            }

            contactRepository.Setup(
                p => p.GetWithInclude(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<Expression<Func<Contact, object>>[]>()))
                             .Returns(new List<Contact> { contact });
            this.SetupEnumTypeItemRepository(enumTypeItemRepository, fixture);

            this.SetupUserRepository(userRepository, command, contact, fixture);

            // Act
            handler.Handle(command);

            // Assert
            ContactUser negotiator = contact.ContactUsers.SingleOrDefault(u => u.UserId == negotiatorIdToUpdate);
            negotiator.Should().NotBeNull();

            // ReSharper disable once PossibleNullReferenceException
            negotiator.UserType.Code.Should()
                      .Be(isNegotiatorToBeChangedToLead
                          ? UserType.LeadNegotiator.ToString()
                          : UserType.SecondaryNegotiator.ToString());

        }

        
        private Contact GetContact(IFixture fixture, ContactUser leadNegotiator = null, List<ContactUser> secondaryNegotiators = null)
        {
            var contact = fixture.Create<Contact>();
         
            if (leadNegotiator == null)
            {
               leadNegotiator = fixture.Build<ContactUser>().With(i => i.UserType, this.GetLeadNegotiatorUserType(fixture)).Create();
            }
            else
            {
                leadNegotiator.UserType = this.GetLeadNegotiatorUserType(fixture);
            }

            contact.ContactUsers = new List<ContactUser> { leadNegotiator };

            secondaryNegotiators?.ForEach(negotiator =>
            {
                negotiator.UserType = this.GetSecondaryNegotiatorUserType(fixture);
                contact.ContactUsers.Add(negotiator);
            });

            return contact;
        }

        private EnumTypeItem GetLeadNegotiatorUserType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, UserType.LeadNegotiator.ToString()).Create();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType(IFixture fixture)
        {
            return fixture.Build<EnumTypeItem>().With(i => i.Code, UserType.SecondaryNegotiator.ToString()).Create();
        }

 
        private IPostprocessComposer<ContactUserCommand> CreateUpdateContactUser(IFixture fixture, Guid userId)
        {
            return fixture.Build<ContactUserCommand>().With(i => i.UserId, userId);
        }

        private UpdateContactCommand CreateUpdatContactCommand(IFixture fixture)
        {
            var rnd = new Random();
            return fixture.Build<UpdateContactCommand>()
                          .With(i => i.LeadNegotiator, this.CreateUpdateContactUser(fixture, fixture.Create<Guid>()).Create())
                          .With(i => i.SecondaryNegotiators, this.CreateUpdateContactUser(fixture, fixture.Create<Guid>()).CreateMany().ToList())
                         .Create();
        }

        private void SetupEnumTypeItemRepository(Mock<IGenericRepository<EnumTypeItem>> enumTypeItemRepository, IFixture fixture)
        {
            enumTypeItemRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<EnumTypeItem, bool>>>()))
                                  .Returns((Expression<Func<EnumTypeItem, bool>> expr) =>
                                      new[]
                                      {
                                          this.GetLeadNegotiatorUserType(fixture),
                                          this.GetSecondaryNegotiatorUserType(fixture)
                                       }.Where(expr.Compile()));
        }

        private void VerifyIfValidationsWereCalled(Contact contact, UpdateContactCommand command, Mock<IEntityValidator> entityValidator, Mock<ICollectionValidator> collectionValidator)
        {
            entityValidator.Verify(x => x.EntityExists(It.Is<Contact>(a => a.Id == contact.Id), command.Id), Times.Once);
            entityValidator.Verify(x => x.EntityExists<User>(command.LeadNegotiator.UserId), Times.Once);
            entityValidator.Verify(x => x.EntitiesExist<User>(It.Is<List<Guid>>(list => list.SequenceEqual(command.SecondaryNegotiators.Select(n => n.UserId).ToList()))), Times.Once);

            List<Guid> expectedNegotiators = command.SecondaryNegotiators.Select(n => n.UserId).ToList();
            expectedNegotiators.Add(command.LeadNegotiator.UserId);

            collectionValidator.Verify(
                x => x.CollectionIsUnique(
                        It.Is<ICollection<Guid>>(list => list.OrderBy(i => i).SequenceEqual(expectedNegotiators.OrderBy(i => i))),
                        ErrorMessage.Negotiators_Not_Unique), Times.Once);

        }

        private User CreateUser(Guid userId, IFixture fixture)
        {
            return fixture.Build<User>()
                .With(x => x.Id, userId)
                .Create();
        }

        private void SetupUserRepository(Mock<IGenericRepository<User>> userRepository, List<Guid> userIds,  IFixture fixture)

        {
            var availableUsers = new List<User>();
            userIds.ForEach(userId => availableUsers.Add(this.CreateUser(userId, fixture)));

            userRepository.Setup(
                x => x.GetWithInclude(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                          .Returns(
                              (Expression<Func<User, bool>> expr, Expression<Func<User, object>>[] expr2) =>
                              availableUsers.Where(expr.Compile()));
        }

        private void SetupUserRepository(
            Mock<IGenericRepository<User>> userRepository,
            UpdateContactCommand command,
            Contact contact,
            IFixture fixture)
        {
            var userIds = new List<Guid> { command.LeadNegotiator.UserId };
            foreach (ContactUserCommand contactuserCommand in command.SecondaryNegotiators)
            {
                userIds.Add(contactuserCommand.UserId);
            }
            foreach (ContactUser contactUser in contact.ContactUsers)
            {
                userIds.Add(contactUser.UserId);
            }

            this.SetupUserRepository(userRepository, userIds.Distinct().ToList(), fixture);
        }

        private void SetupContact(Contact contact, IFixture fixture)
        {
            foreach (ContactUser contactUser in contact.ContactUsers)
            {
                contactUser.User = this.CreateUser(contactUser.UserId, fixture);
            }
        }

       }
}