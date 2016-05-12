/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/models/resources.d.ts" />

module Antares.Property.View.AreaBreakdown {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class AreaBreakdownAddController {
        areas: Business.PropertyArea[] = [];

        constructor() {
            this.addNewArea();
        }

        addNewArea(): Business.PropertyArea {
            var area: Business.PropertyArea = new Business.PropertyArea();

            this.areas.push(area);
            return area;
        }

        removeArea(area: Business.PropertyArea): void{
            _.pull(this.areas, area);
        }
    }

    angular.module('app').controller('AreaBreakdownAddController', AreaBreakdownAddController);
}