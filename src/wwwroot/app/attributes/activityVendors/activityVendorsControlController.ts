/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;

    export class ActivityVendorsControlController {
        // binding
        vendorContacts: Business.Contact[];
        activityTypeId: string;

        $onChanges = (obj: any) => {
            // TODO check code
            console.log(obj.activityTypeId);
        }
    }

    angular.module('app').controller('ActivityVendorsControlController', ActivityVendorsControlController);
};