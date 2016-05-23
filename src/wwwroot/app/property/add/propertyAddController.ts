/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import EnumTypeItem = Common.Models.Business.EnumTypeItem;

    export class PropertyAddController extends Core.WithPanelsBaseController {
        public entityTypeCode: string = 'Property';
        public property: Business.Property = new Business.Property();

        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private propertyTypes: any[];
        private divisions: EnumTypeItem[];
        private attributes: Dto.IAttribute[];
        public userData: Dto.IUserData;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private kfMessageService: Services.KfMessageService) {

            super(componentRegistry, $scope);

            this.property.division.code = this.userData.division.code;
            this.property.divisionId = this.userData.division.id;
            this.property.division = this.userData.division;
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

        loadAttributes = () => {
            this.components.attributeList().loadAttributes();
        }

        loadCharacteristics = () => {
            // propertyTypeId and countryId must be passed directly to method even though they are binded to characteristicList
            // - binding on value objects doesn't work for example for select-onChange event (changed values are set to binded values later then onChange method is executed)
            this.components.characteristicList().loadCharacteristics(this.property.propertyTypeId, this.property.address.countryId);
        }

        public save() {
            this.components.attributeList().clearHiddenAttributesFromProperty();
            this.components.characteristicList().clearHiddenCharacteristicsDataFromProperty();

            this.propertyResource
                .save(new Business.CreateOrUpdatePropertyResource(this.property))
                .$promise
                .then((property: Dto.IProperty) =>{
                    this.$state
                        .go('app.property-view', property)
                        .then(() => this.kfMessageService.showSuccessByCode('PROPERTY.ADD.PROPERTY_ADD_SUCCESS'));
                });
        }

        defineComponentIds() {
            this.componentIds = {
                attributeListId: 'editProperty:attributeListComponent',
                characteristicListId: 'addProperty:characteristicListComponent'
            };
        }

        defineComponents() {
            this.components = {
                attributeList: () => { return this.componentRegistry.get(this.componentIds.attributeListId); },
                characteristicList: () => { return this.componentRegistry.get(this.componentIds.characteristicListId); }
            };
        }
    }

    angular.module('app').controller('PropertyAddController', PropertyAddController);
}