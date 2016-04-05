/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Antares.Common.Models.Dto;

    export class PropertyAddController {
        public entityTypeCode: string = 'Property';
        public property: Dto.Property = new Dto.Property();

        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private propertyTypes: any[];
        private userData: Antares.Common.Models.Dto.IUserData;
        private divisionCode: string;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService){

            this.divisionCode = this.userData.division.code;
            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadPropertyTypes();
        }

        changeDivision = (divisionCode: string) =>{
            this.divisionCode = divisionCode;
            this.property.propertyTypeId = null;
            this.loadPropertyTypes();
        }

        loadPropertyTypes = () => {
            this.propertyResource
                .getPropertyTypes({
                    countryCode: this.userData.country, divisionCode: this.divisionCode, localeCode: 'en'
                }, null)
                .$promise
                .then((propertyTypes: any) => {
                    this.propertyTypes = propertyTypes.propertyTypes;
                });
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