/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface ITextEditControlSchema extends ITextControlSchema {
        formName: string;
        fieldName: string;
    }
}