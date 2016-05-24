/// <reference path="../../../../typings/_all.d.ts" />
/// <reference path="../../../../common/models/resources.d.ts" />

module Antares.Property.View.AreaBreakdown {
    import Resources = Common.Models.Resources;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class AreaBreakdownEditController {
        private componentId: string;
        private propertyAreaBreakdownResourceService: Resources.IPropertyAreaBreakdownResourceClass;

        public propertyAreaBreakdown: Business.UpdatePropertyAreaBreakdownResource;
        constructor(componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $q: ng.IQService,
            private $scope: ng.IScope) {

            componentRegistry.register(this, this.componentId);
            this.propertyAreaBreakdownResourceService = dataAccessService.getPropertyAreaBreakdownResource();
        }

        private isDataValid = (): boolean => {
            var form = this.$scope["editAreaBreakdownForm"];
            form.$setSubmitted();
            return form.$valid;
        }

        editPropertyAreaBreakdown(propertyAreaBreakdown: Dto.IPropertyAreaBreakdown) {
            this.propertyAreaBreakdown = new Business.UpdatePropertyAreaBreakdownResource(propertyAreaBreakdown);
        }

        updatePropertyAreaBreakdown(propertyId: string): ng.IPromise<Business.PropertyAreaBreakdown> {
            if (!this.isDataValid()) {
                return this.$q.reject();
            }

            var params: Resources.IPropertyAreaBreakdownResourceClassParameters = { propertyId: propertyId };

            var onSuccess = (area: Dto.IPropertyAreaBreakdown) => {
                return new Business.PropertyAreaBreakdown(area);
            };
            var onError = (reason: any) => { return reason; };

            return this.propertyAreaBreakdownResourceService.updatePropertyAreaBreakdown(params, this.propertyAreaBreakdown).$promise.then(onSuccess, onError);
        }
    }

    angular.module('app').controller('AreaBreakdownEditController', AreaBreakdownEditController);
}