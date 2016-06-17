using System.Collections.Generic;

namespace KnightFrank.Antares.Domain.UnitTests.Offer.CommandHandlers
{
    using System;
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
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    [Collection("UpdateOfferCommandHandler")]
    [Trait("FeatureTitle", "Offer")]
    public class UpdateOfferCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_UpdateOfferCommand_When_Handle_Then_ShouldUpdateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            UpdateOfferCommand command,
            UpdateOfferCommandHandler handler,
            Offer offer)
        {
            offer.CreatedDate = DateTime.Now;
            command.OfferDate = DateTime.Now.AddDays(-1);
            command.ExchangeDate = DateTime.Now.AddDays(1);
            command.CompletionDate = DateTime.Now.AddDays(1);

            offerRepository.Setup(r => r.GetById(command.Id)).Returns(offer);

            // Act
            Guid offerId = handler.Handle(command);

            // Assert
            Assert.Equal(command.Id, offerId);
            entityValidator.Verify(x => x.EntityExists(offer,command.Id), Times.Once);
            enumTypeValidator.Verify(x => x.ItemExists(EnumType.OfferStatus, command.StatusId), Times.Once);
            offerRepository.Verify(r => r.Save(), Times.Once());
        }
    }
}
