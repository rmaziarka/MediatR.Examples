/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IRadioButtonsEditControlSchema extends IRadioButtonsViewControlSchema {
        formName: string,
        radioButtons: IRadioButtonSchema[]
    }

    interface IRadioButtonSchema {
        value: any,
        translationKey: string;
    }
}