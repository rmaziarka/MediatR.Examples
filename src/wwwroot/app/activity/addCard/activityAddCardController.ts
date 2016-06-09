/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class ActivityAddCardController {
        // bindings
        data: any;
        activityAddCardForm: Antares.Common.Decorators.IKfFormController;

        // controller
        selectedActivityTypeId: string;
        selectedActivityStatusId: string;

        constructor() { }

        save = () => {
            console.log('save');
        }
    }

    angular.module('app').controller('ActivityAddCardController', ActivityAddCardController);
}