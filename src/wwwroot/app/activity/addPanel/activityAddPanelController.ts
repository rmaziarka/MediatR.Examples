/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;

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
        isBusy: boolean;

        constructor(private activityService: Activity.ActivityService, private configService: Services.ConfigService, private eventAggregator: Antares.Core.EventAggregator) {
            super();
        }

        panelShown = () =>{
            this.cardPristine = new Object();
            this.config = null;
        }

        loadConfig = (command: AddPanel.ActivityAddPanelCommand) => {
            this.isBusy = true;

            this.configService
                .getActivity(PageTypeEnum.Create, this.propertyTypeId, command.activityTypeId, command)
                .then(this.configLoaded)
                .finally(() => { this.isBusy = false; });
        }

        configLoaded = (newConfig: IActivityAddPanelConfig) => {
            this.config = newConfig;
        }

        save = (activity: AddCard.ActivityAddCardModel) => {
            var command = new AddPanel.ActivityAddPanelCommand(activity, this.propertyId);
            this.isBusy = true;

            this.activityService.addActivityPanel(command).then((activityDto: Dto.IActivity) => {
                this.eventAggregator.publish(new Antares.Activity.ActivityAddedSidePanelEvent(activityDto));
                this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
            }).finally(() => {
                this.isBusy = false;
            });
        }

        reloadConfig = (activity: AddCard.ActivityAddCardModel) => {
            var command = new AddPanel.ActivityAddPanelCommand(activity, this.propertyId);

            this.loadConfig(command);
        }

        cancel = () => {
            this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
        }
    }

    angular.module('app').controller('ActivityAddPanelController', ActivityAddPanelController);
}