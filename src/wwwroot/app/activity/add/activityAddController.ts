/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;

    export class ActivityAddController {

        componentId: string;
        activityStatuses: any;
        selectedActivityStatus: any;
        defaultActivityStatusCode: string = 'PreAppraisal';

        private vendors: Array<Dto.Contact>;

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

        setVendors(vendors: Array<Dto.Contact>) {
            this.vendors = vendors;
        }

        getVendors(): Array<Dto.Contact> {
            return this.vendors;
        }

        isDataValid = (): boolean => {
            var form = this.$scope["addActivityForm"];
            form.$setSubmitted();
            return form.$valid;
        }
    }

    angular.module('app').controller('ActivityAddController', ActivityAddController);
}