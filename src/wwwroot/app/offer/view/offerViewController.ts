///<reference path="../../typings/_all.d.ts"/>

module Antares.Component {
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import Dto = Common.Models.Dto;
    import OfferViewConfig = Offer.IOfferViewConfig;
    import CompanyContactConnection = Antares.Common.Models.Business.CompanyContactRelation;

    export class OfferViewController extends Core.WithPanelsBaseController {
        // bindings
        offer: Business.Offer;
        config: OfferViewConfig;

        private offerStatuses: Common.Models.Dto.IEnumItem[];

        // controls
        controlSchemas: any = {
            broker: <Attributes.ICompanyContactViewControlSchema>{
                controlId: 'broker',
                translationKey: 'OFFER.EDIT.BROKER',
                emptyTranslationKey: 'OFFER.EDIT.NO_BROKER'
            },
            lender: <Attributes.ICompanyContactViewControlSchema>{
                controlId: 'lender',
                translationKey: 'OFFER.EDIT.LENDER',
                emptyTranslationKey: 'OFFER.EDIT.NO_LENDER'
            },
            surveyor: <Attributes.ICompanyContactViewControlSchema>{
                controlId: 'surveyor',
                translationKey: 'OFFER.EDIT.SURVEYOR',
                emptyTranslationKey: 'OFFER.EDIT.NO_SURVEYOR'
            },
            additionalSurveyor: <Attributes.ICompanyContactViewControlSchema>{
                controlId: 'additionalSurveyor',
                translationKey: 'OFFER.EDIT.SURVEYOR',
                emptyTranslationKey: 'OFFER.EDIT.NO_SURVEYOR'
            },
            mortgageLoanToValue: <Attributes.IPercentNumberControlSchema>{
                controlId: "mortgage-loan-to-value",
                translationKey: "OFFER.VIEW.MORTGAGE_LOAN_TO_VALUE",
                fieldName: "mortgageLoanToValue"
            },
            contractApproved: <Attributes.IRadioButtonsViewControlSchema>{
                fieldName: "offerContractApproved",
                translationKey: "OFFER.VIEW.CONTRACT_APPROVED",
                templateUrl: "app/attributes/radioButtons/templates/radioButtonsViewBoolean.html"
            },
            status: <Attributes.IEnumItemControlSchema>{
                controlId: "offer-status",
                translationKey: "OFFER.VIEW.STATUS",
                enumTypeCode: Dto.EnumTypeCode.OfferStatus
            },
            price: <Attributes.IPriceControlSchema>{
                controlId: "offer-price",
                translationKey: "OFFER.VIEW.OFFER",
                fieldName: "price"
            },
            pricePerWeek: <Attributes.IPriceControlSchema>{
                controlId: "offer-price-per-week",
                translationKey: "OFFER.VIEW.OFFER",
                fieldName: "pricePerWeek",
                suffix: "OFFER.EDIT.OFFER_PER_WEEK"
            },
            offerDate: <Attributes.IDateControlSchema>{
                controlId: "offer-date",
                translationKey: "OFFER.VIEW.OFFER_DATE",
                fieldName: "offerDate"
            },
            exchangeDate: <Attributes.IDateControlSchema>{
                controlId: "offer-exchange-date",
                translationKey: "OFFER.VIEW.EXCHANGE_DATE",
                fieldName: "exchangeDate"
            },
            completionDate: <Attributes.IDateControlSchema>{
                controlId: "offer-completion-date",
                translationKey: "OFFER.VIEW.COMPLETION_DATE",
                fieldName: "completionDate"
            },
            specialConditions: <Attributes.ITextControlSchema>{
                controlId: "offer-special-conditions",
                translationKey: "OFFER.VIEW.SPECIAL_CONDITIONS",
                fieldName: "specialConditions"
            }
        };

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private latestViewsProvider: LatestViewsProvider,
            private enumService: Services.EnumService) {
            super(componentRegistry, $scope);
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        navigateToActivity = (ativity: Business.Activity) => {
            this.$state.go('app.activity-view', { id: ativity.id });
        }

        navigateToRequirement = (requirement: Business.Requirement) => {
            this.$state.go('app.requirement-view', { id: requirement.id });
        }

        showActivityPreview = (offer: Common.Models.Business.Offer) => {
            this.showPanel(this.components.activityPreview);

            this.latestViewsProvider.addView({
                entityId: offer.activity.id,
                entityType: EntityType.Activity
            });
        }

        goToEdit = () => {
            this.$state.go('app.offer-edit', { id: this.$state.params['id'] });
        }

        defineComponentIds() {
            this.componentIds = {
                activityPreviewSidePanelId: 'viewOffer:activityPreviewSidePanelComponent'
            };
        }

        defineComponents() {
            this.components = {
                activityPreview: () => { return this.componentRegistry.get(this.componentIds.activityPreviewSidePanelId); }
            };
        }

        onEnumLoaded = (result: any) => {
            this.offerStatuses = result[Dto.EnumTypeCode.OfferStatus];
        }

        isOfferNew = (): boolean => {
            var selectedOfferStatus: Common.Models.Dto.IEnumItem = _.find(this.offerStatuses, (status: Common.Models.Dto.IEnumItem) => status.id === this.offer.statusId);
            if (selectedOfferStatus) {
                return selectedOfferStatus.code ===
                    Common.Models.Enums.OfferStatus[Common.Models.Enums.OfferStatus.New];
            }

            return false;
        }
    }

    angular.module('app').controller('offerViewController', OfferViewController);
}