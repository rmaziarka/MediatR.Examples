/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyEditController {
        public entityTypeCode: string = 'Property';
        public property: Business.Property;

        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private propertyTypes: any[];
        private attributes: Dto.IAttribute[];
        private userData: Dto.IUserData;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService) {

            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadPropertyTypes();
        }

        changeDivision = (divisionCode: string) => {
            this.property.division.code = divisionCode;
            this.property.propertyTypeId = null;
            this.loadPropertyTypes();
        }

        loadPropertyTypes = () => {
            this.propertyResource
                .getPropertyTypes({
                    countryCode: this.userData.country, divisionCode: this.property.division.code, localeCode: 'en'
                }, null)
                .$promise
                .then((propertyTypes: any) => {
                    this.propertyTypes = propertyTypes.propertyTypes;
                });
        }

        loadAttributes = () => {
            this.propertyResource
                .getAttributes({
                    countryCode: this.userData.country, propertyTypeId: this.property.propertyTypeId
                }, null)
                .$promise
                .then((attributes: any) => {
                    this.attributes = attributes.attributes;
                });
        }

        public save() {
            this.propertyResource
                .update(this.property)
                .$promise
                .then((property: Dto.IProperty) => {
                    this.$state.go('app.property-view', property);
                });
        }
    }

    angular.module('app').controller('PropertyEditController', PropertyEditController);
};