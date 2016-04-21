/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Characteristic {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicListController {
        private componentId: string;
        private propertyResource: Common.Models.Resources.IPropertyResourceClass;
        private property: Business.Property;
        private characteristicGroups: Business.CharacteristicGroup[];

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);

            this.propertyResource = dataAccessService.getPropertyResource();
            this.loadChracteristics();
        }

        loadChracteristics = () => {
            if (this.property.propertyTypeId && this.property.address.countryId) {
                this.propertyResource
                        //this.property.country.code
                    .getCharacteristics({ countryCode: 'GB', propertyTypeId: this.property.propertyTypeId })
                    .$promise
                    .then((characteristicGroupUsages: any) => {
                        this.characteristicGroups = characteristicGroupUsages.map(
                            (item: Dto.ICharacteristicGroupUsage) => new Business.CharacteristicGroup(<Dto.ICharacteristicGroupUsage>item));
                    });
            }
        }
    }

    angular.module('app').controller('CharacteristicListController', CharacteristicListController);
};