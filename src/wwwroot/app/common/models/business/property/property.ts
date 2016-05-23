module Antares.Common.Models.Business {
    export class Property {
        id: string = null;
        propertyTypeId: string = null;
        address: Business.Address = new Business.Address();
        divisionId: string = null;
        division: Business.EnumTypeItem = new Business.EnumTypeItem();
        attributeValues: any = {};
        // dynamic object created basing on list of characteristic (with characteristicId as key)
        propertyCharacteristicsMap: any = {};
        propertyCharacteristics: Models.Dto.IPropertyCharacteristic[];
        propertyAreaBreakdowns: Dto.IPropertyAreaBreakdown[];
        totalAreaBreakdown: number = 0;

        constructor(property?: Dto.IProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Business.Address();
                angular.extend(this.address, property.address);

                this.divisionId = property.divisionId;
                this.division = property.division;
                this.propertyTypeId = property.propertyTypeId;
                this.attributeValues = property.attributeValues;

                _.reduce(property.propertyCharacteristics, (propertyCharacteristicObject, characteristicItem) => {
                    propertyCharacteristicObject[characteristicItem.characteristicId] = new CharacteristicSelect(characteristicItem);
                    return propertyCharacteristicObject;
                }, this.propertyCharacteristicsMap);
            }
        }
    }
}