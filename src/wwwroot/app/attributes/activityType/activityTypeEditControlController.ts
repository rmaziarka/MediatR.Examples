/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityTypeEditControlController {
        // bindings
        propertyTypeId: string;
        ngModel: string;
        config: IActivityTypeEditControlConfig;
        onActivityTypeChanged: (obj: { activityTypeId: string }) => void;

        // controller
        private activityResource: Common.Models.Resources.IActivityResourceClass;
        public activityTypes: Dto.IActivityTypeQueryResult[];

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
                    if (!this.ngModel) {
                        this.setDefaultType();
                    }
                });
        }

        setDefaultType = () => {
            this.ngModel = this.activityTypes[0].id;
            this.changeActivityType();
        }

        changeActivityType = () => {
            this.onActivityTypeChanged({ activityTypeId: this.ngModel });
        }
    }

    angular.module('app').controller('ActivityTypeEditControlController', ActivityTypeEditControlController);
};