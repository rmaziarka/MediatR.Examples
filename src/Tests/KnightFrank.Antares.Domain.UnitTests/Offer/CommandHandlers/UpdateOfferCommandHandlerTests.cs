namespace KnightFrank.Antares.Domain.UnitTests.Offer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.CommandHandlers;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.Offer.OfferHelpers;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;
    using OfferType = KnightFrank.Antares.Dal.Model.Offer.OfferType;
    using RequirementType = KnightFrank.Antares.Dal.Model.Property.RequirementType;

    [Collection("UpdateOfferCommandHandler")]
    [Trait("FeatureTitle", "Offer")]
    public class UpdateOfferCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Dal.Model.Enum.EnumType acceptedEnumType;

        public UpdateOfferCommandHandlerTests()
        {
            IFixture fixture = new Fixture().Customize();

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
            [Frozen] Mock<IAttributeValidator<Tuple<Domain.Common.Enums.OfferType, Domain.Common.Enums.RequirementType>>> attributeValidator,
            [Frozen] Mock<IEntityMapper<Offer>> offerEntityMapper,
            UpdateOfferCommand command,
            UpdateOfferCommandHandler handler,
            Offer offer,
            OfferType offerType,
            Requirement requirement,
            RequirementType requirementType,
            Activity activity)
        {
            this.SetupOffer(command.Id, offer, offerType, requirement, requirementType, activity);
            offer.CreatedDate = DateTime.Now;
            command.OfferDate = DateTime.Now.AddDays(-1);
            command.ExchangeDate = DateTime.Now.AddDays(1);
            command.CompletionDate = DateTime.Now.AddDays(1);
            command.MortgageSurveyDate = DateTime.Now.AddDays(1);
            command.AdditionalSurveyDate = DateTime.Now.AddDays(1);

            offerRepository
                .Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Offer, bool>>>(), It.IsAny<Expression<Func<Offer, object>>[]>()))
                .Returns(new[] { offer });

            enumTypeRepository
                .Setup(x => x.GetWithInclude(It.IsAny<Expression<Func<Dal.Model.Enum.EnumType, bool>>>(), It.IsAny<Expression<Func<Dal.Model.Enum.EnumType, object>>>()))
                .Returns(new List<Dal.Model.Enum.EnumType> { this.acceptedEnumType });

            offerEntityMapper.Setup(
                m => m.MapAllowedValues(command, offer, PageType.Update)).Returns(offer);

            attributeValidator.Setup(
                av => av.Validate(PageType.Update, It.IsAny<Tuple<Domain.Common.Enums.OfferType, Domain.Common.Enums.RequirementType>>(), command));

            // Act
            Guid offerId = handler.Handle(command);

            // Assert
            Assert.Equal(command.Id, offerId);
            entityValidator.Verify(x => x.EntityExists(offer, command.Id), Times.Once);
            enumTypeValidator.Verify(x => x.ItemExists(EnumType.OfferStatus, command.StatusId), Times.Once);
            offerRepository.Verify(r => r.Save(), Times.Once());
            offerEntityMapper.Verify(m => m.MapAllowedValues(It.IsAny<UpdateOfferCommand>(), It.IsAny<Offer>(), It.IsAny<PageType>()), Times.Once);
            entityValidator.Verify(x => x.EntityExists<Contact>(command.ApplicantSolicitorId));
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Company.Company>(command.ApplicantSolicitorCompanyId));
            entityValidator.Verify(x => x.EntityExists<Contact>(command.VendorSolicitorId));
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Company.Company>(command.VendorSolicitorCompanyId));
            attributeValidator.Verify(
                av => av.Validate(PageType.Update, It.IsAny<Tuple<Domain.Common.Enums.OfferType, Domain.Common.Enums.RequirementType>>(), command));
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdateOfferCommandWithAcceptedStatus_When_Handle_Then_ShouldUpdateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            [Frozen] Mock<IOfferProgressStatusHelper> offerProgressStatusHelper,
            [Frozen] Mock<IEntityMapper<Offer>> offerEntityMapper,
            UpdateOfferCommand command,
            UpdateOfferCommandHandler handler,
            Offer offer,
            OfferType offerType,
            Requirement requirement,
            RequirementType requirementType,
            Activity activity)
        {
            this.SetupOffer(command.Id, offer, offerType, requirement, requirementType, activity);
            offer.CreatedDate = DateTime.Now;
            command.OfferDate = DateTime.Now.AddDays(-1);
            command.ExchangeDate = DateTime.Now.AddDays(1);
            command.CompletionDate = DateTime.Now.AddDays(1);
            command.MortgageSurveyDate = DateTime.Now.AddDays(1);
            command.AdditionalSurveyDate = DateTime.Now.AddDays(1);

            offerProgressStatusHelper.Setup(x => x.IsOfferInAcceptedStatus(It.IsAny<List<Dal.Model.Enum.EnumType>>(), It.IsAny<Guid>())).Returns(true);

            offerRepository
                .Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Offer, bool>>>(), It.IsAny<Expression<Func<Offer, object>>[]>()))
                .Returns(new[] { offer });

            offerEntityMapper.Setup(m => m.MapAllowedValues(command, offer, PageType.Update)).Returns(offer);

            // Act
            Guid offerId = handler.Handle(command);

            // Assert
            Assert.Equal(command.Id, offerId);

            enumTypeValidator.Verify(x => x.ItemExists(EnumType.OfferStatus, command.StatusId), Times.Once);

            offerProgressStatusHelper.Verify(x => x.IsOfferInAcceptedStatus(It.IsAny<List<Dal.Model.Enum.EnumType>>(), It.IsAny<Guid>()), Times.Once);

            enumTypeValidator.Verify(x => x.ValidateMandatoryIfItemExists(EnumType.MortgageStatus, command.MortgageStatusId), Times.Once);
            enumTypeValidator.Verify(x => x.ValidateMandatoryIfItemExists(EnumType.MortgageSurveyStatus, command.MortgageSurveyStatusId), Times.Once);
            enumTypeValidator.Verify(x => x.ValidateMandatoryIfItemExists(EnumType.AdditionalSurveyStatus, command.AdditionalSurveyStatusId), Times.Once);
            enumTypeValidator.Verify(x => x.ValidateMandatoryIfItemExists(EnumType.SearchStatus, command.SearchStatusId), Times.Once);
            enumTypeValidator.Verify(x => x.ValidateMandatoryIfItemExists(EnumType.Enquiries, command.EnquiriesId), Times.Once);

            entityValidator.Verify(x => x.EntityExists<Contact>(command.BrokerId));
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Company.Company>(command.BrokerCompanyId));

            entityValidator.Verify(x => x.EntityExists<Contact>(command.LenderId));
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Company.Company>(command.LenderCompanyId));

            entityValidator.Verify(x => x.EntityExists<Contact>(command.SurveyorId));
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Company.Company>(command.SurveyorCompanyId));

            entityValidator.Verify(x => x.EntityExists<Contact>(command.AdditionalSurveyorId));
            entityValidator.Verify(x => x.EntityExists<Dal.Model.Company.Company>(command.AdditionalSurveyorCompanyId));

            offerRepository.Verify(r => r.Save(), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void Given_UpdateOfferCommandWithOtherThanAcceptedStatus_When_Handle_Then_ShouldUpdateOffer(
            [Frozen] Mock<IEntityValidator> entityValidator,
            [Frozen] Mock<IEnumTypeItemValidator> enumTypeValidator,
            [Frozen] Mock<IGenericRepository<Offer>> offerRepository,
            [Frozen] Mock<IOfferProgressStatusHelper> offerProgressStatusHelper,
            [Frozen] Mock<IEntityMapper<Offer>> offerEntityMapper,
            UpdateOfferCommand command,
            UpdateOfferCommandHandler handler,
            Offer offer,
            OfferType offerType,
            Requirement requirement,
            RequirementType requirementType,
            Activity activity)
        {
            this.SetupOffer(command.Id, offer, offerType, requirement, requirementType, activity);
            offer.CreatedDate = DateTime.Now;
            command.OfferDate = DateTime.Now.AddDays(-1);
            command.ExchangeDate = DateTime.Now.AddDays(1);
            command.CompletionDate = DateTime.Now.AddDays(1);
            command.MortgageSurveyDate = DateTime.Now.AddDays(1);
            command.AdditionalSurveyDate = DateTime.Now.AddDays(1);

            offerProgressStatusHelper.Setup(x => x.IsOfferInAcceptedStatus(It.IsAny<List<Dal.Model.Enum.EnumType>>(), It.IsAny<Guid>())).Returns(false);

            offerRepository.Setup(r => r.GetWithInclude(It.IsAny<Expression<Func<Offer, bool>>>(), It.IsAny<Expression<Func<Offer, object>>[]>())).Returns(new[] { offer });

            offerEntityMapper.Setup(m => m.MapAllowedValues(command, offer, PageType.Update)).Returns(offer);

            // Act
            Guid offerId = handler.Handle(command);

            // Assert
            Assert.Equal(command.Id, offerId);
            entityValidator.Verify(x => x.EntityExists(offer, command.Id), Times.Once);
            enumTypeValidator.Verify(x => x.ItemExists(EnumType.OfferStatus, command.StatusId), Times.Once);

            offerProgressStatusHelper.Verify(x => x.IsOfferInAcceptedStatus(It.IsAny<List<Dal.Model.Enum.EnumType>>(), It.IsAny<Guid>()), Times.Once);

            offerProgressStatusHelper.Verify(x => x.KeepOfferProgressStatusesInMessage(offer, command), Times.Once);
            offerProgressStatusHelper.Verify(x => x.KeepOfferMortgageDetailsInMessage(offer, command), Times.Once);
            offerProgressStatusHelper.Verify(x => x.KeepOfferAdditionalSurveyInMessage(offer, command), Times.Once);
            offerProgressStatusHelper.Verify(x => x.KeepOfferOtherDetailsInMessage(offer, command), Times.Once);

            offerRepository.Verify(r => r.Save(), Times.Once());
        }

        private void SetupOffer(Guid offerId, Offer offer, OfferType offerType, Requirement requirement, RequirementType requirementType, Activity activity)
        {
            requirementType.EnumCode = Domain.Common.Enums.RequirementType.ResidentialLetting.ToString();
            requirement.RequirementType = requirementType;
            offer.Requirement = requirement;
            offerType.EnumCode = Domain.Common.Enums.OfferType.ResidentialLetting.ToString();
            offer.OfferType = offerType;
            offer.Id = offerId;
            offer.Activity = activity;
        }
    }
}
