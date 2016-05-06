namespace KnightFrank.Antares.Domain.UnitTests.Viewing.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Viewing.CommandHandlers;
    using KnightFrank.Antares.Domain.Viewing.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Collection("CreateViewingCommandHandler")]
    [Trait("FeatureTitle", "Viewing ")]
    public class CreateViewingCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CreateViewingCommand command;
        private readonly CreateViewingCommandHandler handler;
        private readonly Mock<IEntityValidator> entityValidator;
        private readonly Mock<IGenericRepository<Requirement>> requirementRepository;
        private readonly Mock<IGenericRepository<Viewing>> viewingRepository;

        public CreateViewingCommandHandlerTests()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Clear();
            fixture.RepeatCount = 1;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            List<Contact> contacts = fixture.CreateMany<Contact>(5).ToList();
            List<Guid> attendeeIds = contacts.Select(x => x.Id).Take(2).ToList();

            this.command = fixture.Build<CreateViewingCommand>()
                                  .With(x => x.AttendeesIds, attendeeIds)
                                  .Create();

            // TODO remove userRepository after userRepository is removed from tested method
            var userRepository = fixture.Freeze<Mock<IReadGenericRepository<User>>>();
            userRepository.Setup(u => u.Get()).Returns((fixture.CreateMany<User>()).AsQueryable());

            this.requirementRepository = fixture.Freeze<Mock<IGenericRepository<Requirement>>>();
            this.viewingRepository = fixture.Freeze<Mock<IGenericRepository<Viewing>>>();
            this.entityValidator = fixture.Freeze<Mock<IEntityValidator>>();

            this.requirementRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns(new Requirement
            {
                Contacts = contacts
            });
            this.requirementRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Requirement, bool>>>(), It.IsAny<Expression<Func<Requirement, object>>[]>())).Returns(new Requirement[]
            {
                new Requirement()
                {
                    Contacts = contacts
                }
            });
            this.viewingRepository.Setup(r => r.Add(It.IsAny<Viewing>())).Returns((Viewing a) => a);

            this.handler = fixture.Create<CreateViewingCommandHandler>();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateViewingCommand_When_Handle_Then_ShouldCreateViewing()
        {
            // Act
            this.handler.Handle(this.command);

            // Assert
            this.entityValidator.Verify(x => x.EntityExists(It.IsAny<Requirement>(), this.command.RequirementId), Times.Once);
            this.entityValidator.Verify(x => x.EntityExists<Activity>(this.command.ActivityId), Times.Once);
            this.viewingRepository.Verify(r => r.Add(It.IsAny<Viewing>()), Times.Once());
            this.viewingRepository.Verify(r => r.Save(), Times.Once());
        }
        
        [Theory]
        [AutoMoqData]
        public void Given_CreateViewingCommandWithInvalidAttendeesIds_When_Handle_Then_ShouldThrowBusinessException()
        {
            //Arrange
            this.requirementRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Requirement, bool>>>(), It.IsAny<Expression<Func<Requirement, object>>[]>())).Returns(new Requirement[]
            {
                new Requirement() {
                Contacts = new List<Contact>
                {
                    new Contact { Id = this.command.AttendeesIds[0] }
                }
                }
            });

            // Act + Assert
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { this.handler.Handle(this.command); });
            
            // Assert
            Assert.Equal(ErrorMessage.Missing_Requirement_Attendees_Id, businessValidationException.ErrorCode);
        }
    }
}
