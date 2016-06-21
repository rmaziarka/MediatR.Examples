/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Activity {
    import Attributes = Antares.Attributes;

    interface IActivityEditConfig extends IActivityConfig {
        activityStatus: Attributes.IActivityStatusEditControlConfig;
        departments: Attributes.IActivityDepartmentsViewControlConfig;
        negotiators: Attributes.IActivityNegotiatorsControlConfig;
        property: Attributes.IPropertyViewControlConfig
    }
}