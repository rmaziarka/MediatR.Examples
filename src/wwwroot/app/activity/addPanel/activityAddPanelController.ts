/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddPanelController extends Antares.Common.Component.BaseSidePanelController {
        // binding
        propertyTypeId: string;
        propertyId: string;
        ownerships: Business.Ownership[];
        config: IActivityAddPanelConfig;

        // properties
        activityTypeId: string;
        activityStatusId: string;
        cardPristine: any;

        constructor(private activityService: Activity.ActivityService, private configService: Services.ConfigService, private eventAggregator: Antares.Core.EventAggregator) {
            super();
        }

        $onInit = () => {
            this.loadConfig();
        }


        loadConfig = () => {
            this.configService
                .getActivity(this.propertyTypeId, this.activityTypeId, this.activityStatusId)
                .then(this.configLoaded);
        }

        configLoaded = (newConfig: IActivityAddPanelConfig) => {
            this.config = newConfig;
        }

        save = (activity: AddCard.ActivityAddCardModel) => {
            var command = new AddPanel.ActivityAddPanelCommand(activity, this.propertyId);

            this.activityService.addActivityPanel(command).then((activityDto: Dto.IActivity) => {
                this.cardPristine = new Object();
                this.eventAggregator.publish(new Antares.Activity.ActivityAddedSidePanelEvent(activityDto));
                this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
            });
        }

        cancel = () => {
            this.cardPristine = new Object();
           this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
        }
    }

    angular.module('app').controller('ActivityAddPanelController', ActivityAddPanelController);
}