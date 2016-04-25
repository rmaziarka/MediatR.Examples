/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicListController {
        private componentId: string;
        private characteristicGroupUsageResource: Common.Models.Resources.ICharacteristicGroupUsageResourceClass;

        public property: Business.Property;
        public characteristicGroups: Business.CharacteristicGroup[] = [];

        countryCode: string = 'GB';  // TODO: hardcoded!!! - component commmunication needs to be discussed and maybe api should operate on guids instead of codes

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);

            this.characteristicGroupUsageResource = dataAccessService.getCharacteristicGroupUsageResource();
            this.loadCharacteristics();
        }

        loadCharacteristics = () => {
            if (this.property.propertyTypeId && this.countryCode) {
                this.characteristicGroupUsageResource
                    .query({
                         countryCode: this.countryCode, propertyTypeId: this.property.propertyTypeId
                    })
                    .$promise
                    .then((characteristicGroupUsages: any) => {
                        this.characteristicGroups = characteristicGroupUsages.map(
                            (item: Dto.ICharacteristicGroupUsage) => new Business.CharacteristicGroup(<Dto.ICharacteristicGroupUsage>item));
                    });
            }
        }

        clearCharacteristics = () => {
            this.characteristicGroups = [];
        }

        clearHiddenCharacteristicsDataFromProperty = () => {
            var characteristics: Business.Characteristic[] = _.chain(this.characteristicGroups)
                .map((group: Business.CharacteristicGroup) => {
                    return group.characteristicGroupItems;
                })
                .flatten<Business.Characteristic>()
                .value();

            for (var prop in this.property.propertyCharacteristicsMap) {
                var characteristicId: string = prop;
                if (this.property.propertyCharacteristicsMap.hasOwnProperty(characteristicId)) {
                    var clearCharacteristicData = characteristics.filter((item: Business.Characteristic) => { return item.id === characteristicId; }).length === 0;
                    if (clearCharacteristicData) {
                        this.property.propertyCharacteristicsMap[characteristicId].clear();
                    }
                }
            }
        }
    }

    angular.module('app').controller('CharacteristicListController', CharacteristicListController);
};