using System.Collections.Generic;

namespace KnightFrank.Antares.Domain.UnitTests.Offer.CommandHandlers
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.CommandHandlers;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("CreateOfferCommandHandler")]
    [Trait("FeatureTitle", "Offer")]
    public class CreateOfferCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CreateOfferCommand_When_Handle_Then_ShouldCreateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            CreateOfferCommand command,
            CreateOfferCommandHandler handler,
            List<User> users)
        {
            // TODO remove userRepository after userRepository is removed from tested method
            userRepository.Setup(u => u.Get()).Returns(users.AsQueryable());
            offerRepository.Setup(r => r.Add(It.IsAny<Offer>())).Returns((Offer a) => a);

            // Act
            handler.Handle(command);

            // Assert
            entityValidator.Verify(x => x.EntityExists<Activity>(command.ActivityId), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Requirement>(command.RequirementId), Times.Once);
            enumTypeValidator.Verify(x => x.ItemExists(EnumType.OfferStatus, command.StatusId), Times.Once);
            offerRepository.Verify(r => r.Add(It.IsAny<Offer>()), Times.Once());
            offerRepository.Verify(r => r.Save(), Times.Once());
        }
    }
}
