/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.Preview {
    import Business = Common.Models.Business;

    export class ActivityPreviewController {
        componentId: string;
        activity: Business.Activity = <Business.Activity>{};

        constructor() {
        }  
    }

    angular.module('app').controller('ActivityPreviewController', ActivityPreviewController);
}