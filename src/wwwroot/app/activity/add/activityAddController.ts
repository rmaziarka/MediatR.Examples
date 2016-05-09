/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddController {
        propertyTypeId: string;
        componentId: string;
        defaultActivityStatusCode: string = 'PreAppraisal';
        activities: Common.Models.Business.Activity[];
        activityStatuses: any;
        activityResource: Common.Models.Resources.IActivityResourceClass;

        public enumTypeActivityStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityStatus;
        public selectedActivityStatusId: string;
        public selectedActivityType: any;
        public activityTypes: any[];
        public vendors: Array<Business.Contact> = [];

        constructor(
            private componentRegistry: Core.Service.ComponentRegistry,
            private enumService: Services.EnumService,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $q: ng.IQService) {

            componentRegistry.register(this, this.componentId);

            this.activityResource = dataAccessService.getActivityResource();
            this.enumService.getEnumPromise().then(this.onEnumLoaded);

            this.loadActivityTypes();
        }

        loadActivityTypes = () => {
            this.activityResource
                .getActivityTypes({
                    countryCode: "GB", propertyTypeId: this.propertyTypeId
                }, null)
                .$promise
                .then((activityTypes: any) => {
                    this.activityTypes = activityTypes;
                });
        }

        onEnumLoaded = (result: any) => {
            this.activityStatuses = result[Dto.EnumTypeCode.ActivityStatus];
            this.setDefaultActivityStatus(this.activityStatuses);
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
            activity.activityStatusId = this.selectedActivityStatusId;
            activity.activityTypeId = this.selectedActivityType.id;
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
                this.selectedActivityStatusId = defaultActivityStatus.id;
            }
        }

        clearActivity = () => {
            this.selectedActivityType = null;
            this.setDefaultActivityStatus(this.activityStatuses);
            var form = this.$scope["addActivityForm"];
            form.$setPristine();
        }
    }

    angular.module('app').controller('ActivityAddController', ActivityAddController);
}