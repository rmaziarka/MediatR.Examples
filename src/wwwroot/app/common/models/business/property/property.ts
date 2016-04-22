module Antares.Common.Models.Business {

    export class Property {
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

                property.propertyCharacteristics = [
                    { id: '1', propertyTypeId: '33', characteristicId: 'f1be3793-a707-e611-829c-80c16efdf78c', text: '' },
                    { id: '2', propertyTypeId: '33', characteristicId: 'f3be3793-a707-e611-829c-80c16efdf78c', text: 'def' },
                    { id: '3', propertyTypeId: '33', characteristicId: 'f7be3793-a707-e611-829c-80c16efdf78c', text: 'sss' }
                ];

                _.reduce(property.propertyCharacteristics, (propertyCharacteristicObject, characteristicItem) => {
                    propertyCharacteristicObject[characteristicItem.characteristicId] = new CharacteristicSelect(characteristicItem);
                    return propertyCharacteristicObject;
                }, this.propertyCharacteristicsMap);
            }
        }
    }
}