/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Characteristic {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicListController {
        private componentId: string;
        private characteristicGroupUsageResource: Common.Models.Resources.ICharacteristicGroupUsageResourceClass;
        private property: Business.Property;
        private characteristicGroups: Business.CharacteristicGroup[];

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);

            this.characteristicGroupUsageResource = dataAccessService.getCharacteristicGroupUsageResource();
            this.loadCharacteristics();
        }

        loadCharacteristics = () => {
            if (this.property.propertyTypeId && this.property.address.countryId) {
                this.characteristicGroupUsageResource
                        //this.property.country.code
                    .query({ countryCode: 'GB', propertyTypeId: this.property.propertyTypeId })
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