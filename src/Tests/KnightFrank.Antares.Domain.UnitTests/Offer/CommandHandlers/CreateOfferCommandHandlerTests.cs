using System.Collections.Generic;

namespace KnightFrank.Antares.Domain.UnitTests.Offer.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Offer.CommandHandlers;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;
    using FixtureExtension;

    using KnightFrank.Antares.Domain.Common.Enums;

    [Collection("CreateOfferCommandHandler")]
    [Trait("FeatureTitle", "Offer")]
    public class CreateOfferCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Dal.Model.Enum.EnumType acceptedEnumType;

        public CreateOfferCommandHandlerTests()
        {
            Fixture fixture = new Fixture().Customize();

            this.acceptedEnumType = fixture.Create<Dal.Model.Enum.EnumType>();
            this.acceptedEnumType.EnumTypeItems = fixture.CreateMany<EnumTypeItem>().ToList();

            this.acceptedEnumType.EnumTypeItems.First().Code = OfferStatus.Accepted.ToString();
            this.acceptedEnumType.Code = EnumType.OfferStatus.ToString();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CreateOfferCommand_When_Handle_Then_ShouldCreateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            [Frozen] Mock<IReadGenericRepository<User>> userRepository,
            [Frozen] Mock<IGenericRepository<Dal.Model.Enum.EnumType>> enumTypeRepository,
            CreateOfferCommand command,
            CreateOfferCommandHandler handler,
            List<User> users)
        {
            // TODO remove userRepository after userRepository is removed from tested method
            userRepository.Setup(u => u.Get()).Returns(users.AsQueryable());
            offerRepository.Setup(r => r.Add(It.IsAny<Offer>())).Returns((Offer a) => a);

            enumTypeRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Dal.Model.Enum.EnumType, bool>>>(), It.IsAny<Expression<Func<Dal.Model.Enum.EnumType, object>>>()))
                .Returns(new List<Dal.Model.Enum.EnumType> { this.acceptedEnumType});

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
