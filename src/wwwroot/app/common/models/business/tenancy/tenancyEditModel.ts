/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    import Dto = Models.Dto;
    import Enums = Antares.Common.Models.Enums;

    export class TenancyEditModel extends TenancyBaseModel {
        constructor(dto?: Antares.Common.Models.Dto.ITenancy) {
            super(dto);
        }
    }
}