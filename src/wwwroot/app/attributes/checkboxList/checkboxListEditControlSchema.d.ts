/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface ICheckboxListEditControlSchema extends IListViewControlSchema {
        formName: string,
        fieldName: string;
        compareMember: string;
        checkboxes: ICheckboxSchema[]
    }

    interface ICheckboxSchema {
        selected?: boolean;
        value: any,
        translationKey: string;
    }
}