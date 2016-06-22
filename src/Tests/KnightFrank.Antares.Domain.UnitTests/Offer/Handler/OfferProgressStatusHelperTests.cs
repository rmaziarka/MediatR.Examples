using System;

namespace KnightFrank.Antares.Domain.UnitTests.Offer.Handler
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.OfferHelpers;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;
    using Tests.Common.Extensions.AutoFixture;
    [Collection("OfferProgressStatusHelperTests")]
    [Trait("FeatureTitle", "OfferProgressStatusHelper")]
    public class OfferProgressStatusHelperTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly Offer offer;

        private List<Dal.Model.Enum.EnumType> enumTypes;

        private Guid offerStatusId;
        private Guid offerNewStatusId;
        private Guid offerAcceptedStatusId;
        private Guid offerRejectedStatusId;
        private Guid offerWithdrawnStatusId;

        private Guid offerMortgageStatusId;
        private Guid offerMortgageStatusUnknownId;

        private Guid offerMortgageSurveyStatusId;
        private Guid offerMortgageSurveyStatusUnknownId;

        private Guid offerAdditionalSurveyStatusId;
        private Guid offerAdditionalSurveyStatusUnknownId;

        private Guid offerSearchStatusId;
        private Guid offerSearchStatusNotStartedId;

        private Guid offerEnquiriesStatusId;
        private Guid offerEnquiriesStatusNotStartedId;

        public OfferProgressStatusHelperTests()
        {
            this.PrepareEnumTypesList();

            IFixture fixture = new Fixture().Customize();

            this.offer = fixture.Build<Offer>()
                              .With(x => x.Price, 1)
                              .With(x => x.OfferDate, DateTime.UtcNow.Date)
                              .With(x => x.CompletionDate, DateTime.UtcNow.Date)
                              .With(x => x.ExchangeDate, DateTime.UtcNow.Date)
                              .Create();
        }

        [Theory]
        [InlineAutoData(OfferStatus.New, false)]
        [InlineAutoData(OfferStatus.Accepted, true)]
        [InlineAutoData(OfferStatus.Rejected, false)]
        [InlineAutoData(OfferStatus.Withdrawn, false)]
        public void Given_IsOfferInAcceptedStatusMethod_When_Handling_Then_CorrectStatusIsReturned(
            OfferStatus offerStatus,
            bool expectedResult,
            OfferProgressStatusHelper offerProgressStatusHelper)
        {
            Guid currentOfferStatusId = this.enumTypes.Single(x => x.Code == EnumType.OfferStatus.ToString())
                                      .EnumTypeItems.Single(x => x.Code == offerStatus.ToString()).Id;

            // Act
            bool offerStatusResult = offerProgressStatusHelper.IsOfferInAcceptedStatus(this.enumTypes, currentOfferStatusId);

            // Assert
            offerStatusResult.Should().Be(expectedResult);
        }

        [Theory]
        [AutoData]
        public void Given_GetStatusIdMethodWithNewStatusPassed_When_Handling_Then_CorrectStatusIsReturned(OfferProgressStatusHelper offerProgressStatusHelper)
        {
            // Act
            Guid statusId = offerProgressStatusHelper.GetStatusId(EnumType.OfferStatus, OfferStatus.New.ToString(), this.enumTypes);

            // Assert
            statusId.Should().Be(this.offerNewStatusId);
        }

        [Theory]
        [AutoData]
        public void Given_GetStatusIdMethodWithAcceptedStatusPassed_When_Handling_Then_CorrectStatusIsReturned(OfferProgressStatusHelper offerProgressStatusHelper)
        {
            // Act
            Guid statusId = offerProgressStatusHelper.GetStatusId(EnumType.OfferStatus, OfferStatus.Accepted.ToString(), this.enumTypes);

            // Assert
            statusId.Should().Be(this.offerAcceptedStatusId);
        }

        [Theory]
        [AutoData]
        public void Given_GetStatusIdMethodWithRejectedStatusPassed_When_Handling_Then_CorrectStatusIsReturned(OfferProgressStatusHelper offerProgressStatusHelper)
        {
            // Act
            Guid statusId = offerProgressStatusHelper.GetStatusId(EnumType.OfferStatus, OfferStatus.Rejected.ToString(), this.enumTypes);

            // Assert
            statusId.Should().Be(this.offerRejectedStatusId);
        }

        [Theory]
        [AutoData]
        public void Given_GetStatusIdMethodWithWithdrawnStatusPassed_When_Handling_Then_CorrectStatusIsReturned(OfferProgressStatusHelper offerProgressStatusHelper)
        {
            // Act
            Guid statusId = offerProgressStatusHelper.GetStatusId(EnumType.OfferStatus, OfferStatus.Withdrawn.ToString(), this.enumTypes);

            // Assert
            statusId.Should().Be(this.offerWithdrawnStatusId);
        }

        [Theory]
        [AutoData]
        public void Given_OfferInAcceptedStatus_When_HandleStatuses_Then_DefaultStatusesAreSet(
            [Frozen] Mock<IOfferProgressStatusHelper> offerProgressStatusHelperMock,
            OfferProgressStatusHelper offerProgressStatusHelper)
        {
            //Arrange
            offerProgressStatusHelperMock.Setup(x => x.IsOfferInAcceptedStatus(It.IsAny<List<Dal.Model.Enum.EnumType>>(), It.IsAny<Guid>())).Returns(true);

            //Act
            Offer result = offerProgressStatusHelper.SetOfferProgressStatuses(this.offer, this.enumTypes);

            //Assert
            result.MortgageStatusId.Should().HaveValue(this.offerMortgageStatusId.ToString());
            result.MortgageSurveyStatusId.Should().HaveValue(this.offerMortgageSurveyStatusId.ToString());
            result.AdditionalSurveyStatusId.Should().HaveValue(this.offerAdditionalSurveyStatusId.ToString());
            result.SearchStatusId.Should().HaveValue(this.offerSearchStatusId.ToString());
            result.EnquiriesId.Should().HaveValue(this.offerEnquiriesStatusId.ToString());
        }

        [Theory]
        [AutoData]
        public void Given_OfferInOtherThanAcceptedStatus_When_HandleStatuses_Then_OfferProgressStatusesNotChanged(
            [Frozen] Mock<IOfferProgressStatusHelper> offerProgressStatusHelperMock,
            OfferProgressStatusHelper offerProgressStatusHelper)
        {
            //Arrange
            offerProgressStatusHelperMock.Setup(x => x.IsOfferInAcceptedStatus(It.IsAny<List<Dal.Model.Enum.EnumType>>(), It.IsAny<Guid>())).Returns(false);
            
            //Act
            Offer result = offerProgressStatusHelper.SetOfferProgressStatuses(this.offer, this.enumTypes);

            //Assert
            result.MortgageStatusId.Should().Be(this.offer.MortgageStatusId);
            result.MortgageSurveyStatusId.Should().Be(this.offer.MortgageSurveyStatusId);
            result.AdditionalSurveyStatusId.Should().Be(this.offer.AdditionalSurveyStatusId);
            result.SearchStatusId.Should().Be(this.offer.SearchStatusId);
            result.EnquiriesId.Should().Be(this.offer.EnquiriesId);
        }

        private void PrepareEnumTypesList()
        {
            this.offerStatusId = Guid.NewGuid();
            this.offerNewStatusId = Guid.NewGuid();
            this.offerAcceptedStatusId = Guid.NewGuid();
            this.offerRejectedStatusId = Guid.NewGuid();
            this.offerWithdrawnStatusId = Guid.NewGuid();

            this.offerMortgageStatusId = Guid.NewGuid();
            this.offerMortgageStatusUnknownId = Guid.NewGuid();

            this.offerMortgageSurveyStatusId = Guid.NewGuid();
            this.offerMortgageSurveyStatusUnknownId = Guid.NewGuid();

            this.offerAdditionalSurveyStatusId = Guid.NewGuid();
            this.offerAdditionalSurveyStatusUnknownId = Guid.NewGuid();

            this.offerSearchStatusId = Guid.NewGuid();
            this.offerSearchStatusNotStartedId = Guid.NewGuid();

            this.offerEnquiriesStatusId = Guid.NewGuid();
            this.offerEnquiriesStatusNotStartedId = Guid.NewGuid();

            this.enumTypes = new List<Dal.Model.Enum.EnumType>
            {
                new Dal.Model.Enum.EnumType
                {
                    Id = this.offerStatusId,
                    Code = EnumType.OfferStatus.ToString(),
                    EnumTypeItems = new List<EnumTypeItem>
                    {
                        new EnumTypeItem
                        {
                            Id = this.offerNewStatusId,
                            Code = OfferStatus.New.ToString(),
                            EnumTypeId = this.offerStatusId
                        },
                        new EnumTypeItem
                        {
                            Id = this.offerAcceptedStatusId,
                            Code = OfferStatus.Accepted.ToString(),
                            EnumTypeId = this.offerStatusId
                        },
                        new EnumTypeItem
                        {
                            Id = this.offerRejectedStatusId,
                            Code = OfferStatus.Rejected.ToString(),
                            EnumTypeId = this.offerStatusId
                        },
                        new EnumTypeItem
                        {
                            Id = this.offerWithdrawnStatusId,
                            Code = OfferStatus.Withdrawn.ToString(),
                            EnumTypeId = this.offerStatusId
                        }
                    }
                },
                new Dal.Model.Enum.EnumType
                {
                    Id = this.offerMortgageStatusId,
                    Code = EnumType.MortgageStatus.ToString(),
                    EnumTypeItems = new List<EnumTypeItem>
                    {
                        new EnumTypeItem
                        {
                            Id = this.offerMortgageStatusUnknownId,
                            Code = MortgageStatus.Unknown.ToString(),
                            EnumTypeId = this.offerMortgageStatusId
                        }
                    }
                },
                new Dal.Model.Enum.EnumType
                {
                    Id = this.offerMortgageSurveyStatusId,
                    Code = EnumType.MortgageSurveyStatus.ToString(),
                    EnumTypeItems = new List<EnumTypeItem>
                    {
                        new EnumTypeItem
                        {
                            Id = this.offerMortgageSurveyStatusUnknownId,
                            Code = MortgageSurveyStatus.Unknown.ToString(),
                            EnumTypeId = this.offerMortgageSurveyStatusId
                        }
                    }
                },
                new Dal.Model.Enum.EnumType
                {
                    Id = this.offerAdditionalSurveyStatusId,
                    Code = EnumType.AdditionalSurveyStatus.ToString(),
                    EnumTypeItems = new List<EnumTypeItem>
                    {
                        new EnumTypeItem
                        {
                            Id = this.offerAdditionalSurveyStatusUnknownId,
                            Code = AdditionalSurveyStatus.Unknown.ToString(),
                            EnumTypeId = this.offerAdditionalSurveyStatusId
                        }
                    }
                },
                new Dal.Model.Enum.EnumType
                {
                    Id = this.offerSearchStatusId,
                    Code = EnumType.SearchStatus.ToString(),
                    EnumTypeItems = new List<EnumTypeItem>
                    {
                        new EnumTypeItem
                        {
                            Id = this.offerSearchStatusNotStartedId,
                            Code = SearchStatus.NotStarted.ToString(),
                            EnumTypeId = this.offerSearchStatusId
                        }
                    }
                },
                new Dal.Model.Enum.EnumType
                {
                    Id = this.offerEnquiriesStatusId,
                    Code = EnumType.Enquiries.ToString(),
                    EnumTypeItems = new List<EnumTypeItem>
                    {
                        new EnumTypeItem
                        {
                            Id = this.offerEnquiriesStatusNotStartedId,
                            Code = Enquiries.NotStarted.ToString(),
                            EnumTypeId = this.offerEnquiriesStatusId
                        }
                    }
                }
            };
        }

    }
}
