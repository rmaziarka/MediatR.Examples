/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IActivityStatusEditFieldConfig extends Antares.Common.Models.Dto.IFieldConfig {
        allowedCodes: string[];
    }
}