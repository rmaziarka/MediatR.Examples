/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Requirement {
    import Attributes = Antares.Attributes;

    interface IRequirementAddConfig extends IRequirementConfig {
        requirementType: Attributes.IRequirementTypeEditControlConfig;
        requirement_Description: Attributes.IRequirementDescriptionEditControlConfig;
    }
}