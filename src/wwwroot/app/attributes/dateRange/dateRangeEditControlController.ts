/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class DateRangeEditControlController {
        // bindings 
        public dateFrom: Date;
        public dateTo: Date;
        public schema: IDateRangeControlSchema;
        public config: any;

        // controller
        isDateFromOpened: boolean = false;
        isDateToOpened: boolean = false;

        $onInit = () => {
        }

        openDateFromCalendar = () : void => {
            this.isDateFromOpened = true;
        }

        openDateToCalendar = (): void => {
            this.isDateToOpened = true;
        }
    }

    angular.module('app').controller('DateRangeEditControlController', DateRangeEditControlController);
};