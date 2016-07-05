/// <reference path="../../../typings/_all.d.ts" />

declare module Antares.Attributes {
    interface IRequirementDescriptionEditControlConfig extends Antares.Common.Models.Dto.IControlConfig {
        description: IRequirementDescriptionEditFieldConfig
    }
}