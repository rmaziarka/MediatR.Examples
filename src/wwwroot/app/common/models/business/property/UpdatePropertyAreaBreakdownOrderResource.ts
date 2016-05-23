/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdatePropertyAreaBreakdownOrderResource implements Dto.IUpdatePropertyAreaBreakdownOrderResource {
        propertyId: string;
        areaId: string;
        order: number;

        constructor(propertyAreaBreakdown?: Business.PropertyAreaBreakdown) {
            if (propertyAreaBreakdown) {
                this.propertyId = propertyAreaBreakdown.propertyId;
                this.areaId = propertyAreaBreakdown.id;
                this.order = propertyAreaBreakdown.order;
            }
        }
    }
}