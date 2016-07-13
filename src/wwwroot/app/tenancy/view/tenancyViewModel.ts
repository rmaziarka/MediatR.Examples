/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Antares.Common.Models.Dto;

    export class TenancyViewModel {
        id: string;
        constructor(dto: Dto.ITenancy) {
            if (dto) {
                this.id = dto.id;
            }
        }
    }
}