/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreatePropertyAreaBreakdownResource implements Dto.ICreatePropertyAreaBreakdownResource {
        name: string = null;
        size: number = null;

        constructor(propertyArea?: Dto.ICreatePropertyAreaBreakdownResource) {
            if (propertyArea) {
                angular.extend(this, propertyArea);
            }
        }
    }
}