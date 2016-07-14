/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class TenancyPreviewModel {
        id: string = null;
        activity: Business.ActivityPreviewModel;
        startDate: Date = null;
        endDate: Date = null;

        constructor(dto: Antares.Common.Models.Dto.ITenancy) {
            if (dto) {
                this.id = dto.id;
                this.activity = new Business.ActivityPreviewModel(dto.activity);

                this.startDate = Core.DateTimeUtils.convertDateToUtc(dto.terms[0].startDate);
                this.endDate = Core.DateTimeUtils.convertDateToUtc(dto.terms[0].endDate);
            }
        }   
    }
}