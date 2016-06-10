/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddPanelController extends Antares.Common.Component.BaseSidePanelController {
        // binding
        propertyTypeId: string;
        propertyId: string;
        ownerships: Business.Ownership[];
        onActivityAdded

        constructor(private activityService: Activity.ActivityService, private pubSub: Antares.Core.PubSub) {
            super();
        }

        save = (activity: AddCard.ActivityAddCardModel) => {
            var command = new AddPanel.ActivityAddPanelCommand(activity, this.propertyId);

            this.activityService.addActivityPanel(command).then((activityDto: Dto.IActivity) => {
                this.pubSub.publish(new Antares.Common.Component.ActivityAdded(activityDto));
                this.pubSub.publish(new Antares.Common.Component.CloseSidePanelMessage());
            });
        }
    }

    angular.module('app').controller('ActivityAddPanelController', ActivityAddPanelController);
}