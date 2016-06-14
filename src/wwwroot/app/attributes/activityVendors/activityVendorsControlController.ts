/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityVendorsControlController {
        // binding
        vendorContacts: Business.Contact[];
        config: IActivityVendorsControlConfig;
    }

    angular.module('app').controller('ActivityVendorsControlController', ActivityVendorsControlController);
};