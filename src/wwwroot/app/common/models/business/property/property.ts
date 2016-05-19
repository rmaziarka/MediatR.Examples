module Antares.Common.Models.Business {

    export class Property implements Dto.IProperty {
        id: string = null;
        propertyTypeId: string = null;
        address: Business.Address = new Business.Address();
        ownerships: Business.Ownership[] = [];
        activities: Business.Activity[] = [];
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

                this.ownerships = property.ownerships.map((ownership: Dto.IOwnership) => { return new Business.Ownership(ownership) });
                this.activities = property.activities.map((activity: Dto.IActivity) => { return new Business.Activity(activity) });

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