/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IDateRangeControlSchema {
        dateFromControlId: string,
        dateToControlId: string,
        formName: string,
        dateFromTranslationKey: string;
        dateToTranslationKey: string;
        fieldName: string;
    }
}