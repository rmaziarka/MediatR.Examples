/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface ICheckboxListEditControlSchema {
        fieldName: string;
        translationKey: string,
        formName: string,
        checkboxes: ICheckboxSchema[]
    }

    interface ICheckboxSchema {
        selected?: boolean;
        value: any,
        translationKey: string;
    }
}