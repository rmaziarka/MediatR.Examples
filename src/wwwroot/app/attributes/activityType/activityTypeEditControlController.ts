/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityTypeEditControlController {
        // bindings
        propertyTypeId: string;
        ngModel: string;
        config: IActivityTypeEditControlConfig;
        onActivityTypeChanged: Function;

        // controller
        private activityResource: Common.Models.Resources.IActivityResourceClass;
        public activityTypes: Dto.IActivityTypeQueryResult[];
        public selectedActivityType: Dto.IActivityTypeQueryResult;

        constructor(private dataAccessService: Antares.Services.DataAccessService) {
        }
        
        $onInit = () => {
            this.activityResource = this.dataAccessService.getActivityResource();
            this.loadActivityTypes();
        }

        loadActivityTypes = () => {
            this.activityResource
                .getActivityTypes({
                    countryCode: "GB", propertyTypeId: this.propertyTypeId
                }, null)
                .$promise
                .then((activityTypes: Dto.IActivityTypeQueryResult[]) => {
                    this.activityTypes = activityTypes;
                });
        }

        changeActivityType = () => {
            this.ngModel = this.selectedActivityType ? this.selectedActivityType.id : null;
            this.onActivityTypeChanged();
        }

    }

    angular.module('app').controller('ActivityTypeEditControlController', ActivityTypeEditControlController);
};