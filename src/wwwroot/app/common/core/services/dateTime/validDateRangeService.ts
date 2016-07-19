/// <reference path="../../../../typings/_all.d.ts" />
module Antares.Core.Service {
    export class ValidDateRange {
        private isValidDate = (date: Date): boolean => {
            if (date === undefined) {
                return false;
            }
            var dateTime: number = Date.parse(date.toString());

            if (isNaN(dateTime)) {
                return false;
            }
            return true;
        };

        private getDateDifference = (fromDate: Date, toDate: Date): number => {
            return Date.parse(toDate.toDateString()) - Date.parse(fromDate.toDateString());
        };

        public isValidDateRange = (fromDate: Date, toDate: Date, equalityAllowed: boolean): boolean => {
            if (!(fromDate && toDate)) {
                return true;
            }
            if (!this.isValidDate(fromDate)) {
                return false;
            }
            if (this.isValidDate(toDate)) {
                var days: number = this.getDateDifference(fromDate, toDate);
                return equalityAllowed ? days >= 0 : days > 0;
            }
            return true;
        };
    }

    angular.module('app').factory('validDateRange', () => new ValidDateRange());
}