/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Tenancy {
        id: string = null;
        activity: Activity;
        requirement: Requirement;
        landlords: Business.Contact[];
        tenants: Business.Contact[];
        startDate: Date | string = null;
        ednDate: Date | string = null;
        rent: number; 
    }
}