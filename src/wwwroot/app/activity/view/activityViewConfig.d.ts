/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Activity {
    import Attributes = Antares.Attributes;

    interface IActivityViewConfig extends IActivityConfig {
        departments: Attributes.IActivityDepartmentsViewControlConfig;
        vendors: Attributes.IActivityVendorsControlConfig;
        landlords: Attributes.IActivityLandlordsControlConfig;
        negotiators: Attributes.IActivityNegotiatorsControlConfig;
    }
}