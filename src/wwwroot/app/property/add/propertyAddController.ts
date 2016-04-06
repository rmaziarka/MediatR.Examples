﻿/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    export class PropertyAddController {
        public entityTypeCode: string = 'Property';
        public property: Business.Property = new Business.Property();

        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private propertyTypes: any[];
        private userData: Antares.Common.Models.Dto.IUserData;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private $state: ng.ui.IStateService) {

            this.propertyResource = dataAccessService.getPropertyResource();
            this.propertyResource
                .getPropertyTypes({
                    countryCode: this.userData.country, divisionCode: this.userData.division.code, localeCode: 'en' }, null)
                .$promise
                .then((propertyTypes: any) =>{
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