/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.Preview {
    import Activity = Common.Models.Business.Activity;

    export class ActivityPreviewController {
        componentId: string;
        activity: Activity = <Activity>{};

        constructor(
            componentRegistry: Core.Service.ComponentRegistry) {
            componentRegistry.register(this, this.componentId);
        }

        setActivity = (activity: Activity) => {
            this.activity = activity;
        }
    }

    angular.module('app').controller('ActivityPreviewController', ActivityPreviewController);
}