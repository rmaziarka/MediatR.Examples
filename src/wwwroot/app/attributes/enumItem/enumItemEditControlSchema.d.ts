/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IEnumItemEditControlSchema extends IEnumItemControlSchema {
        formName: string;
        fieldName: string;
        enumTypeCode: Common.Models.Dto.EnumTypeCode;
    }
}