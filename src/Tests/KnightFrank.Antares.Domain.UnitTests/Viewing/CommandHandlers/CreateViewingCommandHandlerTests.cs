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
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

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
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            [Frozen] Mock<IGenericRepository<Requirement>> requirementRepository,
            [Frozen] Mock<IGenericRepository<Viewing>> viewingRepository,
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            CreateViewingCommand command,
            CreateViewingCommandHandler handler,
            Requirement requirement,
            List<Contact> contacts,
            List<User> users)
        {
            // Arrange
            requirement.Contacts = contacts;

            // TODO remove userRepository after userRepository is removed from tested method
            userRepository.Setup(u => u.Get()).Returns(users.AsQueryable());

            requirementRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Requirement, bool>>>(), It.IsAny<Expression<Func<Requirement, object>>[]>())).Returns(new List<Requirement> { requirement });
            viewingRepository.Setup(r => r.Add(It.IsAny<Viewing>())).Returns((Viewing a) => a);

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Requirement>(), command.RequirementId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Activity>(command.ActivityId), Times.Once);
            collectionValidator.Verify(x => x.CollectionContainsAll(It.IsAny<IEnumerable<Guid>>(), command.AttendeesIds, It.IsAny<ErrorMessage>()), Times.Once);
            viewingRepository.Verify(r => r.Add(It.IsAny<Viewing>()), Times.Once());
            viewingRepository.Verify(r => r.Save(), Times.Once());
        }
    }
}
