/// <reference path='../../typings/_all.d.ts' />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
	import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ActivityPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {
        config: Antares.Activity.IActivityPreviewPanelConfig;
        activity: Business.Activity;
        isBusy: boolean;
        propertyTypeId: string;
        constructor(private configService: Services.ConfigService, private latestViewsProvider: LatestViewsProvider) {
            super();
        }

        panelShown = () => {
            this.config= null;
            this.activity = angular.copy(this.activity);
            this.activity.property = null;
            this.isBusy = true;

            this.configService.getActivity(
                Common.Models.Enums.PageTypeEnum.Preview,
                 this.propertyTypeId, 
                 this.activity.activityTypeId,
                 this.activity)
                .then(this.configLoaded);

            this.latestViewsProvider.addView({
                entityId: this.activity.id,
                entityType: EntityType.Activity
            });
        }
        configLoaded = (newConfig: IActivityPreviewPanelConfig) => {
            this.config = newConfig;
            this.isBusy = false;
        }
    }

    angular.module('app').controller('activityPreviewPanelController', ActivityPreviewPanelController);
}