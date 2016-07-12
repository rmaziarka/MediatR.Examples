/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class TenancyPreviewModel {
        id: string = null;

        constructor(dto: Antares.Common.Models.Dto.ITenancy) {
            if (dto) {
                this.id = dto.id;
            }
        }   
    }
}