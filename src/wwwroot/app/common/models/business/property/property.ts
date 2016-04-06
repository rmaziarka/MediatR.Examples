module Antares.Common.Models.Business {
    import Dto = Models.Dto;

    export class Property implements Dto.IProperty {
        id: string = null;
        propertyTypeId: string = null;
        address: Dto.Address = new Dto.Address();
        ownerships: Business.Ownership[] = [];
        activities: Business.Activity[] = [];

        constructor(property?: Dto.IProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Dto.Address();
                angular.extend(this.address, property.address);

                this.ownerships = property.ownerships.map((ownership: Dto.IOwnership) => { return new Business.Ownership(ownership) });
                this.activities = property.activities.map((activity: Dto.IActivity) => { return new Dto.Activity(activity) });
            }
        }
    }
}