/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Tenancy {
        id: string = null;
        requirementId: string = null;
        requirement: Requirement = null;
        activityId: string = null;
        activity: Activity = null;
        contacts: TenancyContact[];
        terms: TenancyTerm[];
        tenancyTypeId: string = null;
        tenancyType: EnumTypeItem = null;
        landlords: Contact[];
        tenants: Contact[];        
    }
}