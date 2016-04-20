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

        constructor(property?: Dto.IProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Business.Address();
                angular.extend(this.address, property.address);

                this.ownerships = property.ownerships.map((ownership: Dto.IOwnership) => { return new Business.Ownership(ownership) });
                this.activities = property.activities.map((activity: Dto.IActivity) => { return new Business.Activity(activity) });

                this.propertyTypeId = property.propertyTypeId;
                this.attributeValues = property.attributeValues;
            }
        }
    }
}