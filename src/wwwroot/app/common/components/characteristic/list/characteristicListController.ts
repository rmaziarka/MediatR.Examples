/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicListController {
        private componentId: string;
        private characteristicGroupUsageResource: Common.Models.Resources.ICharacteristicGroupUsageResourceClass;

        public propertyTypeId: string;
        public countryId: string;
        public characteristicsMap: any;
        public characteristicGroups: Business.CharacteristicGroup[] = [];

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);

            this.characteristicGroupUsageResource = dataAccessService.getCharacteristicGroupUsageResource();
            this.loadCharacteristics();
        }

        loadCharacteristics = () => {
            if (this.propertyTypeId && this.countryId) {
                this.characteristicGroupUsageResource
                    .query({
                        countryId: this.countryId,
                        propertyTypeId: this.propertyTypeId
                    })
                    .$promise
                    .then((characteristicGroupUsages: any) => {
                        this.characteristicGroups = characteristicGroupUsages.map(
                            (item: Dto.ICharacteristicGroupUsage) => new Business.CharacteristicGroup(<Dto.ICharacteristicGroupUsage>item));
                    });
            }
            else {
                this.clearCharacteristics();
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

            for (var prop in this.characteristicsMap) {
                var characteristicId: string = prop;
                if (this.characteristicsMap.hasOwnProperty(characteristicId)) {
                    var clearCharacteristicData = characteristics.filter((item: Business.Characteristic) => { return item.id === characteristicId; }).length === 0;
                    if (clearCharacteristicData) {
                        this.characteristicsMap[characteristicId].clear();
                    }
                }
            }
        }

        isCharacteristicDataEmpty = (characteristic: Business.Characteristic): boolean => {
            return !this.characteristicsMap[characteristic.id];
        }

        isCharacteristicGroupDataEmpty = (group: Business.CharacteristicGroup): boolean => {
            return !_.any(group.characteristicGroupItems, (characteristic: Business.Characteristic) => {
                return !this.isCharacteristicDataEmpty(characteristic);
            });
        }
    }

    angular.module('app').controller('CharacteristicListController', CharacteristicListController);
};