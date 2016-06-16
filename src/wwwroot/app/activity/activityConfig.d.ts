/// <reference path="../typings/_all.d.ts" />

declare module Antares.Activity {
    interface IActivityConfig {
        activityStatus: Attributes.IActivityStatusEditControlConfig;
        activityType: Attributes.IActivityTypeEditControlConfig;
        vendors: Attributes.IActivityVendorsControlConfig;
        landlords: Attributes.IActivityLandlordsControlConfig;
    }
}