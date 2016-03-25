/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {

    export class ActivityAddController {

        componentId: string;
        activityStatuses: any;
        selectedActivityStatusId: number;

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry,
            private dataAccessService: Antares.Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);

            this.dataAccessService.getEnumResource().get({ code: 'ActivityStatus' }).$promise.then(this.onActivityStatusLoaded);
        }

        onActivityStatusLoaded = (result: any) => {
            this.activityStatuses = result.items;
        }
    }

    angular.module('app').controller('ActivityAddController', ActivityAddController);
}