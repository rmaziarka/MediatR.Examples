module Antares.Common.Models.Dto {
    export class Property implements IProperty {
        id: string = null;
        propertyTypeId: string = null;
        address: Dto.Address = new Dto.Address();
        ownerships: Business.Ownership[] = [];
        activities: Business.Activity[] = [];

        constructor (property?: IProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Address();
                angular.extend(this.address, property.address);

                this.ownerships = property.ownerships.map((ownership: IOwnership) => { return new Business.Ownership(ownership) });
                this.activities = property.activities.map((activity: IActivity) => { return new Business.Activity(activity) });
            }
        }
    }
}