/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityVendorsControlController {
        // binding
        vendorContacts: Business.Contact[];
        activityTypeId: string;

        // controller
        activeTranslationKey: string;

        vendorTranslationKey: string = "VENDORS";
        landLordTranslationKey: string = "LANDLORDS";
         
        constructor(private resourcesProvider: Antares.Providers.ResourcesProvider) { }

        $onInit = () => {
            this.activeTranslationKey = this.vendorTranslationKey;
            this.refreshData();
        }

        $onChanges = (obj: any) => {
            if (obj.activityTypeId.currentValue !== obj.activityTypeId.previousValue) {
                this.refreshData();
            }
        }

        refreshData = () => {
            var activityTypes = _.filter(this.resourcesProvider.activityTypes, (type: Dto.IActivityType) => {
                type.id === this.activityTypeId;
            });

            if (activityTypes.length != 1) {
                return;
            }

            var activityType: Dto.IActivityType = activityTypes[0];

            if (activityType.code === 'Open Market Letting' || activityType.code === 'Long Leasehold Sale') {
                this.activeTranslationKey = this.landLordTranslationKey;
            }
            else {
                this.activeTranslationKey = this.vendorTranslationKey;
            }
        }
    }

    angular.module('app').controller('ActivityVendorsControlController', ActivityVendorsControlController);
};