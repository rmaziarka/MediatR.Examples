/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreatePropertyAreaBreakdownResource implements Dto.ICreatePropertyAreaBreakdownResource {
        name: string;
        size: number;

        constructor(propertyArea: Business.PropertyArea) {
            if (propertyArea) {
                angular.extend(this, propertyArea);
            }
        }
    }
}