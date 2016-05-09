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
            private enumService: Services.EnumService,
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
            this.enumService.getEnumPromise().then((result: any) => {
                this.divisions = result[Dto.EnumTypeCode.Division];
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
            // propertyTypeId and countryId must be passed directly to method even though they are binded to characteristicList
            // - binding on value objects doesn't work for example for select-onChange event (changed values are set to binded values later then onChange method is executed)
            this.components.characteristicList().loadCharacteristics(this.property.propertyTypeId, this.property.address.countryId);
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