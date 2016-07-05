/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IRadioButtonsEditControlSchema {
        formName: string,
        fieldName: string;
        translationKey: string,
        radioButtons: IRadioButtonSchema[]
    }

    interface IRadioButtonSchema {
        value: any,
        translationKey: string;
    }
}