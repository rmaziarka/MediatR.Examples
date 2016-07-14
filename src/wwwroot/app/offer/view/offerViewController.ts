///<reference path="../../typings/_all.d.ts"/>

module Antares.Component {
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import Commands = Common.Models.Commands;
    import Enums = Common.Models.Enums;
    import Dto = Common.Models.Dto;
    import Offer = Attributes.Offer;

    export class OfferViewController {
        // bindings
        offer: Business.Offer;
        config: Antares.Offer.IOfferViewConfig;

        //fields
        private offerStatuses: Common.Models.Dto.IEnumItem[];
        private offerChainsType: any = Enums.OfferChainsType;
        private activityEditCommand: Commands.IChainTransactionCommand;
        private isUpwardChainPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        private isActivityPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        // controls
        controlSchemas: OfferViewSchema = OfferViewSchema;

        constructor(
            private eventAggregator: Core.EventAggregator,
            private $state: ng.ui.IStateService,
            private latestViewsProvider: LatestViewsProvider,
            private enumProvider: Providers.EnumProvider){
            this.offerStatuses = this.enumProvider.enums.offerStatus;

            var activityEditModel = new Business.ActivityEditModel(this.offer.activity);
            this.activityEditCommand = new Commands.Activity.ActivityEditCommand(activityEditModel);

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, this.hidePanels);
            this.eventAggregator.with(this).subscribe(Offer.OpenChainPanelEvent, this.openChainPanel);
            this.eventAggregator.with(this).subscribe(Offer.ActivityUpdatedOfferChainsEvent, this.activityChanged);
            this.eventAggregator.with(this).subscribe(Offer.RequirementUpdatedOfferChainsEvent, this.requirementChanged);
        }

        hidePanels = () =>{
            this.isActivityPreviewPanelVisible = Enums.SidePanelState.Closed;
            this.isUpwardChainPanelVisible = Enums.SidePanelState.Closed;
        }

        openChainPanel = () =>{
            this.hidePanels();
            this.isUpwardChainPanelVisible = Enums.SidePanelState.Opened;
        }

        activityChanged = (event: Offer.ActivityUpdatedOfferChainsEvent) =>{
            var activity = new Business.Activity(event.updatedActivity);
            this.offer.activity = activity;

            var activityEditModel = new Business.ActivityEditModel(this.offer.activity);
            this.activityEditCommand = new Commands.Activity.ActivityEditCommand(activityEditModel);
        }

        requirementChanged = (event: Offer.RequirementUpdatedOfferChainsEvent) =>{
            throw new Error("Requirement changed not handled");
        }

        navigateToActivity = (ativity: Business.Activity) => {
            this.$state.go('app.activity-view', { id: ativity.id });
        }

        navigateToRequirement = (requirement: Business.Requirement) => {
            this.$state.go('app.requirement-view', { id: requirement.id });
        }

        showActivityPreview = (offer: Common.Models.Business.Offer) =>{
            this.hidePanels();
            this.isActivityPreviewPanelVisible = Enums.SidePanelState.Opened;

            this.latestViewsProvider.addView({
                entityId: offer.activity.id,
                entityType: EntityType.Activity
            });
        }

        goToEdit = () => {
            this.$state.go('app.offer-edit', { id: this.$state.params['id'] });
        }

        isMortgageDetailsSectionVisible = () => {
            return this.config.offer_Broker
                || this.config.offer_Lender
                || this.config.offer_MortgageSurveyDate
                || this.config.offer_Surveyor;
        }

        isProgressSummarySectionVisible = () => {
            return this.config.offer_MortgageStatus
                || this.config.offer_MortgageSurveyStatus
                || this.config.offer_SearchStatus
                || this.config.offer_Enquiries
                || this.config.offer_ContractApproved;
        }

        isAdditionalSurveySectionVisible = () => {
            return this.config.offer_AdditionalSurveyor
                || this.config.offer_AdditionalSurveyDate;
        }

        isOtherDetailsSectionVisible = () => {
            return this.config.offer_ProgressComment;
        }

        isProgressAndMortgageSectionVisible = () => {
            return this.isMortgageDetailsSectionVisible() ||
                this.isAdditionalSurveySectionVisible() ||
                this.isProgressSummarySectionVisible() ||
                this.isOtherDetailsSectionVisible();
        }
    }

    angular.module('app').controller('offerViewController', OfferViewController);
}