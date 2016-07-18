/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface ISearchUserControlSchema {
        fieldName: string;
        searchPlaceholderTranslationKey: string;
        formName: string,
        itemTemplateUrl: string;
        controlId: string;
        usersSearchMaxCount: number;
        emptyTranslationKey: string;
    }
}