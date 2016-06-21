/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityLandlordsEditControlController {
        // binding
        landlordsContacts: Business.Contact[];
        config: IActivityLandlordsControlConfig;
    }

    angular.module('app').controller('ActivityLandlordsEditControlController', ActivityLandlordsEditControlController);
};