/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class TenancyEditModel {
        id: string = null;
        activity: Business.ActivityPreviewModel;
        requirement: Business.RequirementPreviewModel;
        landlords: Business.Contact[];
        tenants: Business.Contact[];
        startDate: Date  = null;
        endDate: Date  = null;
        agreedRent: number; 

        constructor(dto?: Antares.Common.Models.Dto.ITenancy) {
            if (dto) {
                this.id = dto.id;
                this.activity = new Business.ActivityPreviewModel(dto.activity);
                this.requirement = new Business.RequirementPreviewModel(dto.requirement);
                this.startDate = Core.DateTimeUtils.convertDateToUtc(dto.terms[0].startDate);
                this.endDate = Core.DateTimeUtils.convertDateToUtc(dto.terms[0].endDate);
                this.agreedRent = dto.terms[0].price;
            }
            else {
                this.startDate = moment().toDate();
                this.endDate = moment().toDate();
            }
        }
    }
}