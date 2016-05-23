/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/models/resources.d.ts" />

module Antares.Property.View.AreaBreakdown {
    import Resources = Common.Models.Resources;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class AreaBreakdownAddController {
        private componentId: string;
        private propertyAreaBreakdownResourceService: Resources.IPropertyAreaBreakdownResourceClass;
        areas: Business.CreatePropertyAreaBreakdownResource[];

        constructor(componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $q: ng.IQService) {

            componentRegistry.register(this, this.componentId);
            this.propertyAreaBreakdownResourceService = dataAccessService.getPropertyAreaBreakdownResource();
            this.clearAreas();
        }

        clearAreas() {
            this.areas = [];
            this.addNewArea();
        }

        addNewArea(): void {
            var area: Business.CreatePropertyAreaBreakdownResource = new Business.CreatePropertyAreaBreakdownResource();
            this.areas.push(area);
        }

        removeArea(area: Business.CreatePropertyAreaBreakdownResource): void {
            _.pull(this.areas, area);
        }

        saveAreas(propertyId: string): ng.IPromise<Business.PropertyAreaBreakdown[]> {
            var params: Resources.IPropertyAreaBreakdownResourceClassParameters = { propertyId: propertyId };
            var data: Resources.ICreatePropertyAreaBreakdownResourceClassData = { areas: this.areas };

            var onSuccess = (areas: Dto.IPropertyAreaBreakdown[]) => {
                var propertyAreas: Business.PropertyAreaBreakdown[] = areas.map(area => new Business.PropertyAreaBreakdown(area));
                return propertyAreas;
            };
            var onError = (reason: any) => { return reason;};

            return this.propertyAreaBreakdownResourceService.createPropertyAreaBreakdowns(params, data).$promise.then(onSuccess, onError);
        }
    }

    angular.module('app').controller('AreaBreakdownAddController', AreaBreakdownAddController);
}