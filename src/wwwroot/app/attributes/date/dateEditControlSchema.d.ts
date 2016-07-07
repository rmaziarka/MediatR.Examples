/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IDateEditControlSchema extends IDateControlSchema {
        fieldName: string;
        formName: string;
    }
}