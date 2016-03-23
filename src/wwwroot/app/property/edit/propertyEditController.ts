/// <reference path="../../typings/_all.d.ts" />
/// <reference path="../../common/models/dto/property.ts" />
/// <reference path="../../common/models/resources.d.ts" />

module Antares.Property {
    import Dto = Antares.Common.Models.Dto;

    export class PropertyEditController {
        public entityTypeCode: string = 'Property';

        public isLoading: boolean = true;
        public property: Dto.Property = new Dto.Property();

        private propertyResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IPropertyResource>;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            private $state: ng.ui.IStateService) {

            this.propertyResource = dataAccessService.getPropertyResource();

            var propertyId: string = $state.params['id'];
            this.get(propertyId);
        }

        public get(id: string){
            this.propertyResource
                .get({ id : id })
                .$promise
                .then((data: Dto.IProperty) =>{
                    this.property = data;
                })
                .finally(() =>{
                    this.isLoading = false;
                });
        }

        public save() {
            this.propertyResource
                .update(this.property)
                .$promise
                .then((property: Dto.IProperty) => {
                    //TODO: change to property-view when ready
                    this.$state.go('app.property-edit', property);
                });
        }
    }

    angular.module('app').controller('PropertyEditController', PropertyEditController);
};