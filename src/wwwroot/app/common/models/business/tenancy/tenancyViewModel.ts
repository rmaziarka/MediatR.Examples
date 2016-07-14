/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Antares.Common.Models.Dto;

    export class TenancyViewModel extends TenancyBaseModel {
        constructor(dto?: Antares.Common.Models.Dto.ITenancy) {
            super(dto);
        }
    }
}