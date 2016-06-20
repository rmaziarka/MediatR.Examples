namespace KnightFrank.Antares.Domain.UnitTests.Viewing.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Viewing.CommandHandlers;
    using KnightFrank.Antares.Domain.Viewing.Commands;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("UpdateViewingCommandHandler")]
    [Trait("FeatureTitle", "Viewing ")]
    public class UpdateViewingCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_UpdateViewingCommand_When_Handle_Then_ShouldUpdateViewing(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            [Frozen] Mock<IGenericRepository<Viewing>> viewingRepository,
            UpdateViewingCommand command,
            UpdateViewingCommandHandler handler,
            Viewing viewing,
            List<Contact> contacts)
        {
            // Arrange
            viewing.Requirement = new Requirement { Id = viewing.RequirementId, Contacts = contacts };
            viewingRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Viewing, bool>>>(), It.IsAny<Expression<Func<Viewing, object>>[]>())).Returns(new List<Viewing> { viewing });

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Viewing>(), command.Id), Times.Once);
            collectionValidator.Verify(x => x.CollectionContainsAll(It.IsAny<IEnumerable<Guid>>(), command.AttendeesIds, It.IsAny<ErrorMessage>()), Times.Once);
            viewingRepository.Verify(x => x.GetWithInclude(It.IsAny<Expression<Func<Viewing, bool>>>(), It.IsAny<Expression<Func<Viewing, object>>[]>()), Times.Once);
            viewingRepository.Verify(x => x.Save(), Times.Once());
        }
    }
}