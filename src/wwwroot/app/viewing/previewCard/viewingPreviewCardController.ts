/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing.Preview {
    import Business = Common.Models.Business;

    export class ViewingPreviewCardController {
        // binding
        viewing: Business.Viewing;
    }

    angular.module('app').controller('ViewingPreviewCardController', ViewingPreviewCardController);
}