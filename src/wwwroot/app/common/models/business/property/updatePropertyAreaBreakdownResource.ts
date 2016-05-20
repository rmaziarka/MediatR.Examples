/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdatePropertyAreaBreakdownResource implements Dto.IUpdatePropertyAreaBreakdownResource {
        id: string;
        name: string;
        size: number;

        constructor(propertyArea: Dto.IPropertyAreaBreakdown) {
            if (propertyArea) {
                this.id = propertyArea.id;
                this.name = propertyArea.name;
                this.size = propertyArea.size;
            }
        }
    }
}