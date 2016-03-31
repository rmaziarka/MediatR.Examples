/// <reference path="../../typings/_all.d.ts" />
module Antares.Core{
    export class DateTimeUtils {
        static createDateAsUtc(date: any): Date {
            if (!date)
                return date;

            if (typeof (date) === "string")
                date = new Date(date);

            return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
        }

        static convertDateToUtc(date: any): Date {
            if (!date)
                return date;

            if (typeof (date) === "string")
                date = new Date(date);

            return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
        }
    }
}