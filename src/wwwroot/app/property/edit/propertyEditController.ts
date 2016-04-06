/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyEditController {
        public entityTypeCode: string = 'Property';
        public property: Business.Property;

        private propertyResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IPropertyResource>;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService){

            this.propertyResource = dataAccessService.getPropertyResource();
        }

        public save(){
            this.propertyResource
                .update(this.property)
                .$promise
                .then((property: Dto.IProperty) =>{
                    this.$state.go('app.property-view', property);
                });
        }
    }

    angular.module('app').controller('PropertyEditController', PropertyEditController);
};