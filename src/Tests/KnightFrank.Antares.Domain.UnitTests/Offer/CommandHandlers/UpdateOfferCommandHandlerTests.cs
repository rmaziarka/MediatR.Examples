namespace KnightFrank.Antares.Domain.UnitTests.Offer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.CommandHandlers;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    [Collection("UpdateOfferCommandHandler")]
    [Trait("FeatureTitle", "Offer")]
    public class UpdateOfferCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Dal.Model.Enum.EnumType acceptedEnumType;

        public UpdateOfferCommandHandlerTests()
        {
            Fixture fixture = new Fixture().Customize();

            this.acceptedEnumType = fixture.Create<Dal.Model.Enum.EnumType>();
            this.acceptedEnumType.EnumTypeItems = fixture.CreateMany<EnumTypeItem>().ToList();

            this.acceptedEnumType.EnumTypeItems.First().Code = OfferStatus.Accepted.ToString();
            this.acceptedEnumType.Code = EnumType.OfferStatus.ToString();
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdateOfferCommand_When_Handle_Then_ShouldUpdateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            [Frozen] Mock<IGenericRepository<Dal.Model.Enum.EnumType>> enumTypeRepository,
            UpdateOfferCommand command,
            UpdateOfferCommandHandler handler,
            Offer offer)
        {
            offer.CreatedDate = DateTime.Now;
            command.OfferDate = DateTime.Now.AddDays(-1);
            command.ExchangeDate = DateTime.Now.AddDays(1);
            command.CompletionDate = DateTime.Now.AddDays(1);
            command.MortgageSurveyDate = DateTime.Now.AddDays(1);
            command.AdditionalSurveyDate = DateTime.Now.AddDays(1);

            offerRepository.Setup(r => r.GetById(command.Id)).Returns(offer);

            enumTypeRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Dal.Model.Enum.EnumType, bool>>>(), It.IsAny<Expression<Func<Dal.Model.Enum.EnumType, object>>>()))
                .Returns(new List<Dal.Model.Enum.EnumType> { this.acceptedEnumType });

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
