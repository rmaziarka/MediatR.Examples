/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class ActivityCardController {
        activity: Business.Activity;
    }

    angular.module('app').controller('activityCardController', ActivityCardController);
};