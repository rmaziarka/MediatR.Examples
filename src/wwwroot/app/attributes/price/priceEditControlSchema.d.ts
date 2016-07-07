/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IPriceEditControlSchema extends IPriceControlSchema {
        formName: string;
        fieldName: string;
    }
}