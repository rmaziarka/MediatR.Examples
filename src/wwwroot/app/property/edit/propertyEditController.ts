/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class PropertyEditController extends Core.WithPanelsBaseController {
        public entityTypeCode: string = 'Property';
        public property: Business.Property;

        components: any;
        componentIds: any;

        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private propertyTypes: any[];
        private attributes: Dto.IAttribute[];
        private userData: Dto.IUserData;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);

            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadPropertyTypes();
        }

        changeDivision = (divisionCode: string) => {
            this.property.division.code = divisionCode;
            this.property.propertyTypeId = null;
            this.components.attributeList().clearAttributes();
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
            this.components.attributeList().loadAttributes();
        }

        public save() {
            this.propertyResource
                .update(this.property)
                .$promise
                .then((property: Dto.IProperty) => {
                    this.$state.go('app.property-view', property);
                });
        }

        defineComponentIds() {
            this.componentIds = {
                attributeListId: 'editProperty:attributeListComponent'
            };
        }

        defineComponents() {
            this.components = {
                attributeList: () => { return this.componentRegistry.get(this.componentIds.attributeListId); }
            };
        }
    }

    angular.module('app').controller('PropertyEditController', PropertyEditController);
};