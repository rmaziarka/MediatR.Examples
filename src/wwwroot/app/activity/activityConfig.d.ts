/// <reference path="../typings/_all.d.ts" />

declare module Antares.Activity {
    interface IActivityConfig {
        [key: string]: any;
        activityStatus: Attributes.IActivityStatusEditControlConfig;
        activityType: Attributes.IActivityTypeEditControlConfig;
        vendors: Attributes.IActivityVendorsControlConfig;
        landlords: Attributes.IActivityLandlordsControlConfig;
		askingPrice?: Attributes.IActivityAskingPriceControlConfig;
		shortLetPricePerWeek?: Attributes.IActivityShortLetPricePerWeekControlConfig;
    }
}