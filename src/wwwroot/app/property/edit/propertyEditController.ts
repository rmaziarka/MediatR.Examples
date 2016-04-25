/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import EnumTypeItem = Common.Models.Business.EnumTypeItem;

    export class PropertyEditController extends Core.WithPanelsBaseController {
        public entityTypeCode: string = 'Property';
        public property: Business.Property;
        public userData: Dto.IUserData;

        components: any;
        componentIds: any;

        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private propertyTypes: any[];
        private divisions: EnumTypeItem[];
        private attributes: Dto.IAttribute[];

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);

            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadDivisions();
            this.loadPropertyTypes();
        }

        changeDivision = (division: EnumTypeItem) => {
            this.property.divisionId = division.id;
            this.property.division = division;
            this.property.propertyTypeId = null;
            if (this.components.attributeList()) {
                this.components.attributeList().clearAttributes();
            }
            if (this.components.characteristicList()) {
                this.components.characteristicList().clearCharacteristics();
            }
            this.loadPropertyTypes();
        }

        loadDivisions = () => {
            this.dataAccessService.getEnumResource().get({ code: 'Division' }).$promise.then((divisions: any) => {
                this.divisions = divisions.items;
            });
        };

        changePropertyType = () => {
            this.loadAttributes();
            this.loadCharacteristics();
        };

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

        loadCharacteristics = () => {
            this.components.characteristicList().loadCharacteristics();
        }

        loadAttributes = () => {
            this.components.attributeList().loadAttributes();
        }

        public save() {
            this.components.attributeList().clearHiddenAttributesFromProperty();
            this.components.characteristicList().clearHiddenCharacteristicsDataFromProperty();

            this.propertyResource
                .update(new Business.CreateOrUpdatePropertyResource(this.property))
                .$promise
                .then((property: Dto.IProperty) => {
                    this.$state.go('app.property-view', property);
                });
        }

        defineComponentIds() {
            this.componentIds = {
                attributeListId: 'editProperty:attributeListComponent',
                characteristicListId: 'editProperty:characteristicListComponent'
            };
        }

        defineComponents() {
            this.components = {
                attributeList: () => { return this.componentRegistry.get(this.componentIds.attributeListId); },
                characteristicList: () => { return this.componentRegistry.get(this.componentIds.characteristicListId); }
            };
        }
    }

    angular.module('app').controller('PropertyEditController', PropertyEditController);
};