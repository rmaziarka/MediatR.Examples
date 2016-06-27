/// <reference path="../../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IRequirementTypeEditFieldConfig extends Antares.Common.Models.Dto.IFieldConfig {
        allowedCodes: string[];
    }
}