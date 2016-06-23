/// <reference path='../../typings/_all.d.ts' />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityPreviewCardController {
        propertyTypeId:string;
        activity:Business.Activity;
    }

    angular.module('app').controller('activityPreviewCardController', ActivityPreviewCardController);
}