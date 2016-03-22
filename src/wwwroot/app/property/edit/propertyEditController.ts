/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Antares.Common.Models.Dto;

    export class PropertyEditController {
        public entityTypeCode: string = 'Property';
        public property: Dto.Property = new Dto.Property();

        private propertyResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IPropertyResource>;

        constructor(
            private dataAccessService: Antares.Services.DataAccessService,
            $state: angular.ui.IState) {

            this.propertyResource = dataAccessService.getPropertyResource();

            var propertyId = $state.params.id;
            this.get(propertyId);
        }

        public get(id: string) {
            this.propertyResource
                .get({ id: id })
                .$promise
                .then((data: Dto.IProperty) => {
                    this.property = data;
                });
        }

        public save() {
            alert("Saved: " + JSON.stringify(this.property.address));
            //redirect to view
        }
    }

    angular.module('app').controller('PropertyEditController', PropertyEditController);
};