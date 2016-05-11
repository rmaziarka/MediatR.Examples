/// <reference path="../../typings/_all.d.ts" />
module Antares.Core{
    declare var moment: any;
    export class DateTimeUtils {        
        static createDateAsUtc(date: any): Date {
            if (!date)
                return date;

            if (typeof (date) === "string")
                date = new Date(date.replace(new RegExp("\"","g"),""));

            var year = date.getUTCFullYear();
            if (year >= 0 && year < 100) {
                var utcDate = new Date(Date.UTC(year + 100, date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
                utcDate.setUTCFullYear(utcDate.getFullYear() - 100);
                return utcDate;
            }

            return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
        }

        static convertDateToUtc(date: any): Date {
            if (!date)
                return date;

            if (typeof (date) === "string")
                date = new Date(date);

            var year = date.getUTCFullYear();
            if (year >= 0 && year < 100) {
                var utcDate = new Date(Date.UTC(year + 100, date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
                utcDate.setUTCFullYear(utcDate.getFullYear() - 100);
                return utcDate;
            }

            return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
        }

        static getDatePart(datetime: any): any
        {
            return moment(datetime).format("YYYY-MM-DD");
        }
    }
}