/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Common.Models.Business;

    export class TenancyEditModel {
        id: string = null;
        activity: Business.ActivityPreviewModel;
        requirement: Business.RequirementPreviewModel;
        landlords: Business.Contact[];
        tenants: Business.Contact[];
        startDate: Date | string = null;
        ednDate: Date | string = null;
        rent: number; 
    }
}