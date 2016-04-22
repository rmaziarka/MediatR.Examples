/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class CharacteristicListController {
        private componentId: string;
        private characteristicGroupUsageResource: Common.Models.Resources.ICharacteristicGroupUsageResourceClass;

        public property: Business.Property;
        public characteristicGroups: Business.CharacteristicGroup[] = [];
        public characteristicSelect: Business.CharacteristicSelect = new Business.CharacteristicSelect();

        countryCode: string = 'GB';  // TODO: hardcoded!!! - component commmunication needs to be discussed and maybe api should operate on guids instead of codes

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService) {

            componentRegistry.register(this, this.componentId);

            this.characteristicGroupUsageResource = dataAccessService.getCharacteristicGroupUsageResource();
            this.loadCharacteristics();

            // temporary mocked single characteristic data
            this.characteristicSelect.characteristicId = '3e84bbde-a807-e611-826f-8cdcd42e5436';
            this.characteristicSelect.text = 'kod';
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
    }

    angular.module('app').controller('CharacteristicListController', CharacteristicListController);
};