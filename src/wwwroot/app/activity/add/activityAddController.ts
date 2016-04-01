/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {

    export class ActivityAddController {

        componentId: string;
        activityStatuses: any;
        selectedActivityStatus: any;
        defaultActivityStatusCode: string = 'PreAppraisal';

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry,
            private dataAccessService: Antares.Services.DataAccessService,
            private $scope: ng.IScope) {

            componentRegistry.register(this, this.componentId);

            this.dataAccessService.getEnumResource().get({ code: 'ActivityStatus' }).$promise.then(this.onActivityStatusLoaded);
        }

        onActivityStatusLoaded = (result: any) => {
            var defaultActivityStatus: any = _.find(result.items, { 'code' : this.defaultActivityStatusCode });

            if (defaultActivityStatus) {
                this.selectedActivityStatus = defaultActivityStatus;
            }

            this.activityStatuses = result.items;
        }

        isDataValid = (): boolean => {
            var form = this.$scope["addActivityForm"];
            form.$setSubmitted();
            return form.$valid;
        }
    }

    angular.module('app').controller('ActivityAddController', ActivityAddController);
}