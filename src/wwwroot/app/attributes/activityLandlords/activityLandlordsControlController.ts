/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityLandlordsControlController {
        // binding
        landlordsContacts: Business.Contact[];
        config: IActivityLandlordsControlConfig;
    }

    angular.module('app').controller('ActivityLandlordsControlController', ActivityLandlordsControlController);
};