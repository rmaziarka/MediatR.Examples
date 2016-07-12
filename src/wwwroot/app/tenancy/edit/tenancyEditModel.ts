/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    import Business = Common.Models.Business;

    export class TenancyEditModel {
        id: string = null;
        activity: Business.ActivityPreviewModel;
        requirement: Business.RequirementPreviewModel;
        landlords: Business.Contact[];
        tenants: Business.Contact[];
        startDate: Date  = null;
        ednDate: Date  = null;
        rent: number; 

        constructor(dto?: Antares.Common.Models.Dto.ITenancy) {
            if (dto) {
                this.id = dto.id;

            }
        }
    }
}