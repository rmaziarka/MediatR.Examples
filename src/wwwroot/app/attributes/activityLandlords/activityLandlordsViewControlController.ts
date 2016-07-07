/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityLandlordsViewControlController {
        // binding
        vendorContacts: Business.Contact[];
        config: IActivityVendorsControlConfig;
        hideHeader: boolean = false;
    }

    angular.module('app').controller('ActivityLandlordsViewControlController', ActivityLandlordsViewControlController);
};