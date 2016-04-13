/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.Preview {
    import Business = Common.Models.Business;

    export class ActivityPreviewController {
        componentId: string;
        activity: Business.Activity = <Business.Activity>{};

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private $state: ng.ui.IStateService) {

            componentRegistry.register(this, this.componentId);
        }

        setActivity = (activity: Business.Activity) => {
            this.activity = activity;
        }

        goToActivityView = () => {
            this.$state.go('app.activity-view', { id: this.activity.id });
        }
    }

    angular.module('app').controller('ActivityPreviewController', ActivityPreviewController);
}