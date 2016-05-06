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
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateViewingCommandHandler")]
    [Trait("FeatureTitle", "Viewing ")]
    public class CreateViewingCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CreateViewingCommand_When_Handle_Then_ShouldCreateViewing(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<Viewing>> viewingRepository,
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            CreateViewingCommand command,
            CreateViewingCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            command.AttendeesIds = fixture.CreateMany<Guid>(2).ToList();
            List<Contact> contacts = command.AttendeesIds.Select(x => new Contact { Id = x }).ToList();

            // TODO remove userRepository after userRepository is removed from tested method
            userRepository.Setup(u => u.Get()).Returns((fixture.CreateMany<User>()).AsQueryable());

            requirementRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Requirement, bool>>>(), It.IsAny<Expression<Func<Requirement, object>>[]>())).Returns(new Requirement[]
            {
                new Requirement
                {
                    Contacts = contacts
                }
            });
            viewingRepository.Setup(r => r.Add(It.IsAny<Viewing>())).Returns((Viewing a) => a);

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Requirement>(), command.RequirementId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Activity>(command.ActivityId), Times.Once);
            viewingRepository.Verify(r => r.Add(It.IsAny<Viewing>()), Times.Once());
            viewingRepository.Verify(r => r.Save(), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateViewingCommandWithInvalidAttendeesIds_When_Handle_Then_ShouldThrowBusinessException(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<Viewing>> viewingRepository,
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            CreateViewingCommand command,
            CreateViewingCommandHandler handler,
            IFixture fixture)
        {
            // Arrange
            command.AttendeesIds = fixture.CreateMany<Guid>(2).ToList();

            // TODO remove userRepository after userRepository is removed from tested method
            userRepository.Setup(u => u.Get()).Returns((fixture.CreateMany<User>()).AsQueryable());

            requirementRepository.Setup(
                r =>
                    r.GetWithInclude(It.IsAny<Expression<Func<Requirement, bool>>>(),
                        It.IsAny<Expression<Func<Requirement, object>>[]>())).Returns(new[]
                        {
                            new Requirement
                            {
                                Contacts = new List<Contact>
                                {
                                    fixture.Build<Contact>().With(x => x.Id, command.AttendeesIds.First()).Create()
                                }
                            }
                        });

            // Act + Assert
            var businessValidationException = Assert.Throws<BusinessValidationException>(() => { handler.Handle(command); });

            // Assert
            Assert.Equal(ErrorMessage.Missing_Requirement_Attendees_Id, businessValidationException.ErrorCode);
        }
    }
}
