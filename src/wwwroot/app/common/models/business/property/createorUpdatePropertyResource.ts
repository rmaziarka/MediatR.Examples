/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateOrUpdatePropertyResource {
        id: string = null;
        propertyTypeId: string = null;
        address: Business.Address = new Business.Address();
        ownerships: Business.Ownership[] = [];
        activities: Business.Activity[] = [];
        divisionId: string = null;
        division: Business.EnumTypeItem = new Business.EnumTypeItem();
        attributeValues: any = {};
        propertyCharacteristics: Business.CreateOrUpdatePropertyCharacteristicResource[];

        //TODO: temporary solution - class should be rewritten so it is clearly defined what is sent to server
        constructor(property: Business.Property){
            if (property) {
                angular.extend(this, property);
                this.propertyCharacteristics = [];

                this.propertyCharacteristics =
                    _.chain(_.values(property.propertyCharacteristicsMap))
                    .filter((item: CharacteristicSelect) =>{
                        return item.isSelected;
                    })
                    .map((item: CharacteristicSelect) =>{
                        return new Business.CreateOrUpdatePropertyCharacteristicResource(item);
                    }).value();
            }
        }
    }
}