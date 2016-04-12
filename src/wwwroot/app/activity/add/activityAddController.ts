/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddController {

        componentId: string;
        activities: Common.Models.Dto.IActivity[];
        activityStatuses: any;
        selectedActivityStatus: any;
        defaultActivityStatusCode: string = 'PreAppraisal';
        activityResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityResource>;

        public vendors: Array<Business.Contact> = [];

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $q: ng.IQService) {

            componentRegistry.register(this, this.componentId);

            this.activityResource = dataAccessService.getActivityResource();
            this.dataAccessService.getEnumResource().get({ code: 'ActivityStatus' }).$promise.then(this.onActivityStatusLoaded);
        }

        onActivityStatusLoaded = (result: any) => {
            this.setDefaultActivityStatus(result.items);

            this.activityStatuses = result.items;
        }

        setVendors(vendors: Array<Business.Contact>){
            this.vendors = vendors || [];
        }

        saveActivity = (propertyId: string): ng.IPromise<void> => {
            if (!this.isDataValid()) {
                return this.$q.reject();
            }

            var activity = new Business.Activity();
            activity.propertyId = propertyId;
            activity.activityStatusId = this.selectedActivityStatus.id;
            activity.contacts = this.vendors;

            return this.activityResource.save(new Business.CreateActivityResource(activity)).$promise.then((result: Dto.IActivity) => {
                var addedActivity = new Business.Activity(result);

                this.activities.push(addedActivity);
            });
        }

        isDataValid = (): boolean => {
            var form = this.$scope["addActivityForm"];
            form.$setSubmitted();
            return form.$valid;
        }

        setDefaultActivityStatus = (result: any) => {
            var defaultActivityStatus: any = _.find(result, { 'code': this.defaultActivityStatusCode });

            if (defaultActivityStatus) {
                this.selectedActivityStatus = defaultActivityStatus;
            }
        }

        clearActivity = () => {
            this.setDefaultActivityStatus(this.activityStatuses);
            var form = this.$scope["addActivityForm"];
            form.$setPristine();
        }
    }

    angular.module('app').controller('ActivityAddController', ActivityAddController);
}