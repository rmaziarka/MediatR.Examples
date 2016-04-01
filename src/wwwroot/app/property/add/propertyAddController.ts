/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Antares.Common.Models.Dto;

    export class PropertyAddController {
        public entityTypeCode: string = 'Property';
        public property: Dto.Property = new Dto.Property();

        private propertyResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IPropertyResource>;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService){

            this.propertyResource = dataAccessService.getPropertyResource();
        }

        public save() {
            this.propertyResource
                .save(this.property)
                .$promise
                .then((property: Dto.IProperty) => {
                    this.$state.go('app.property-view', property);
                });
        }
    }

    angular.module('app').controller('PropertyAddController', PropertyAddController);
}