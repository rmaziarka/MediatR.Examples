/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddPanelController extends Antares.Common.Component.BaseSidePanelController {
        // binding
        data: any;

        constructor() {
            super();
        }
    }

    angular.module('app').controller('ActivityAddPanelController', ActivityAddPanelController);
}