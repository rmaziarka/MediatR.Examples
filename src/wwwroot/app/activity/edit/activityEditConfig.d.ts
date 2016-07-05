/// <reference path="../../typings/_all.d.ts" />

declare module Antares.Activity {
    import Attributes = Antares.Attributes;

    interface IActivityEditConfig extends IActivityConfig {
        activityStatus: Attributes.IActivityStatusEditControlConfig;
        agreedInitialMarketingPrice: any;
        decoration: any;
        departments: Attributes.IActivityDepartmentsViewControlConfig;
        groundRentAmount: any;
        groundRentNote: any;
        kfValuationPrice: any;
        longAgreedInitialMarketingPrice: any;
        longKfValuationPrice: any;
        longVendorValuationPrice: any;
        negotiators: Attributes.IActivityNegotiatorsControlConfig;
        otherCondition: any;
        property: Attributes.IPropertyViewControlConfig;
        serviceChargeAmount: any;
        shortAgreedInitialMarketingPrice: any;
        shortKfValuationPrice: any;
        shortVendorValuationPrice: any;
        vendorValuationPrice: any;
    }
}