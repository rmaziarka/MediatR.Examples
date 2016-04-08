/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.Preview {
    import Activity = Common.Models.Business.Activity;

    export class ActivityPreviewController {
        componentId: string;
        activity: Activity = <Activity>{};

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private $state: ng.ui.IStateService) {

            componentRegistry.register(this, this.componentId);
        }

        setActivity = (activity: Activity) => {
            this.activity = activity;
        }

        goToActivityView = () => {
            this.$state.go('app.activity-view', { id: this.activity.id });
        }
    }

    angular.module('app').controller('ActivityPreviewController', ActivityPreviewController);
}