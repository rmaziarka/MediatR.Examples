/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/models/resources.d.ts" />

module Antares.Property.View.AreaBreakdown {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class AreaBreakdownAddController {
        private componentId: string;
        areas: Business.PropertyArea[] = [];

        constructor(componentRegistry: Core.Service.ComponentRegistry,
            private $q: ng.IQService) {
            componentRegistry.register(this, this.componentId);

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

        saveAreas(propertyId: string): ng.IPromise<Business.PropertyArea[]>{
            // TODO validate and send data
            var defer: any = this.$q.defer();
            defer.resolve(this.areas);

            return defer.promise;
        }
    }

    angular.module('app').controller('AreaBreakdownAddController', AreaBreakdownAddController);
}