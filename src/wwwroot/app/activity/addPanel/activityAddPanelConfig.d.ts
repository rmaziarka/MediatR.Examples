/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Activity {
    import Attributes = Antares.Attributes;

    interface IActivityAddPanelConfig extends IActivityConfig {
        activityStatus: Attributes.IActivityStatusEditControlConfig;
        activityType: Attributes.IActivityTypeEditControlConfig;
        vendors: Attributes.IActivityVendorsControlConfig;
        landlords: Attributes.IActivityLandlordsControlConfig;
    }
}