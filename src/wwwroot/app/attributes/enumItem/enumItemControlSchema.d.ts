/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IEnumItemControlSchema {
        controlId: string,
        translationKey: string;
        enumTypeCode: Common.Models.Dto.EnumTypeCode;
    }
}