namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Ownership.CommandHandlers;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    [Collection("CreateOwnershipCommandHandler")]
    [Trait("FeatureTitle", "Ownership")]
    public class CreateOwnershipCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IGenericRepository<Property>> propertyRepository,
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeItemValidator,
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<ICollectionValidator> collectionValidator,
            CreateOwnershipCommand command,
            CreateOwnershipCommandHandler commandHandler,
            List<Ownership> ownerships)
        {
            var property = new Property
            {
                Ownerships = ownerships,
                Id = command.PropertyId
            };

            propertyRepository.Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Property, bool>>>(), It.IsAny<Expression<Func<Property, Object>>>()))
                .Returns(new List<Property> { property });
            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(new List<Contact>().AsQueryable());
            entityValidator.Setup(x => x.EntityExists(It.IsAny<Property>(), It.IsAny<Guid>()));
            enumTypeItemValidator.Setup(x => x.ItemExists(It.IsAny<EnumType>(), It.IsAny<Guid>()));

            // Act
            commandHandler.Handle(command);

            // Assert
            enumTypeItemValidator.Verify(x => x.ItemExists(It.IsAny<EnumType>(), It.IsAny<Guid>()), Times.Once);
            entityValidator.Verify(x => x.EntityExists(It.IsAny<Property>(), It.IsAny<Guid>()), Times.Once);
            collectionValidator.Verify(x => x.RangeDoesNotOverlap(It.IsAny<List<Range<DateTime>>>(), It.IsAny<Range<DateTime>>(), It.IsAny<ErrorMessage>()), Times.Once);

            ownershipRepository.Verify(x => x.Add(It.IsAny<Ownership>()), Times.Once);
            ownershipRepository.Verify(x => x.Save(), Times.Once);
        }
    }
}
