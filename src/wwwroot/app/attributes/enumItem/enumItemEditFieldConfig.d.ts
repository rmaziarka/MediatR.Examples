/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IEnumItemEditFieldConfig extends Antares.Common.Models.Dto.IFieldConfig {
        allowedCodes: string[];
    }
}