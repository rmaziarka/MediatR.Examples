/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/models/resources.d.ts" />

module Antares.Property.View.AreaBreakdown {
    import Resources = Common.Models.Resources;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class AreaBreakdownAddController {
        private componentId: string;
        private propertyAreaBreakdownResourceService: Resources.IPropertyAreaBreakdownResourceClass;
        areas: Business.PropertyArea[];

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

        addNewArea(): Business.PropertyArea {
            var area: Business.PropertyArea = new Business.PropertyArea();

            this.areas.push(area);
            return area;
        }

        removeArea(area: Business.PropertyArea): void {
            _.pull(this.areas, area);
        }

        saveAreas(propertyId: string): ng.IPromise<Common.Models.Dto.IPropertyAreaBreakdown[]> {
            var params: Resources.IPropertyAreaBreakdownResourceClassParameters = { propertyId: propertyId };
            var data: Resources.IPropertyAreaBreakdownResourceClassData = { areas: [] };
            data.areas = this.areas.map(area => new Business.CreatePropertyAreaBreakdownResource(area));

            var onSuccess = (areas: Common.Models.Dto.IPropertyAreaBreakdown[]) => {
                var propertyAreas: Business.PropertyArea[] =  areas.map(area => new Business.PropertyArea(area));
                return propertyAreas;
            };
            var onError = (reason: any) => { return reason;};

            return this.propertyAreaBreakdownResourceService.createPropertyAreaBreakdowns(params, data).$promise.then(onSuccess, onError);
        }
    }

    angular.module('app').controller('AreaBreakdownAddController', AreaBreakdownAddController);
}