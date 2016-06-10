/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddCardController {
        // bindings
        propertyTypeId: string;

        // controller
        selectedActivityTypeId: string;
        selectedActivityStatusId: string;

        constructor() { }

        save = () => {
            console.log('save');
            console.log('selectedActivityTypeId ' + this.selectedActivityTypeId);
            console.log('selectedActivityStatusId ' + this.selectedActivityStatusId);

        }
    }

    angular.module('app').controller('ActivityAddCardController', ActivityAddCardController);
}