/// <reference path='../../typings/_all.d.ts' />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {
        config: Antares.Activity.IActivityPreviewPanelConfig;
        activity: Business.Activity;
        propertyTypeId: string;
        constructor(private configService: Services.ConfigService) {
            super();
        }

        panelShown = () => {
            this.config= null;
            this.activity = angular.copy(this.activity);
            this.activity.property = null;
            this.configService.getActivity(
                Common.Models.Enums.PageTypeEnum.Preview,
                 this.propertyTypeId, 
                 this.activity.activityTypeId,
                 this.activity)
                .then(this.configLoaded);
        }
        configLoaded = (newConfig: IActivityAddPanelConfig) => {
            this.config = newConfig;
        }
    }

    angular.module('app').controller('activityPreviewPanelController', ActivityPreviewPanelController);
}