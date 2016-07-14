/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IRadioButtonsEditControlSchema {
        fieldName: string;
        translationKey: string,
        formName: string,
        radioButtons: IRadioButtonSchema[],
        onChangeValue: () => void
    }

    interface IRadioButtonSchema {
        value: any,
        translationKey: string;
    }
}