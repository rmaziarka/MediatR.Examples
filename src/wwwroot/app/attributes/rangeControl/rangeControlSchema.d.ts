/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IRangeControlSchema {
        minControlId: string,
        maxControlId: string,
        formName: string,
        unit: string,
        translationKey: string;
    }
}