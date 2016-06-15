///<reference path="../../typings/_all.d.ts"/>

module Antares.Component {
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
	import Dto = Common.Models.Dto;

    export class OfferViewController extends Core.WithPanelsBaseController {
		public offer: Business.Offer;
        private offerStatuses: Common.Models.Dto.IEnumItem[];
		
        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private latestViewsProvider: LatestViewsProvider,
			private enumService: Services.EnumService) {
            super(componentRegistry, $scope);
			this.enumService.getEnumPromise().then(this.onEnumLoaded);
        }

        navigateToActivity = (ativity: Business.Activity) =>{
            this.$state.go('app.activity-view', { id: ativity.id });
        }

        navigateToRequirement = (requirement: Business.Requirement) => {
            this.$state.go('app.requirement-view', { id: requirement.id});
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
                activityPreviewSidePanelId : 'viewOffer:activityPreviewSidePanelComponent'
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
            var selectedOfferStatus: Common.Models.Dto.IEnumItem = _.find(this.offerStatuses, (status:  Common.Models.Dto.IEnumItem) => status.id === this.offer.statusId);
            if (selectedOfferStatus) {
                return selectedOfferStatus.code === "New";
            }

            return false;
        }
    }

    angular.module('app').controller('offerViewController', OfferViewController);
}