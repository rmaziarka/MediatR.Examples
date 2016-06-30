/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Requirement {
    import Attributes = Antares.Attributes;

    interface IRequirementViewConfig extends IRequirementConfig {
        requirementType: Attributes.IRequirementTypeEditControlConfig;
    }
}