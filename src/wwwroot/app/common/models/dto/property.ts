module Antares.Common.Models.Dto {
    export class Property implements IProperty {
        id: string = null;
        address: Dto.Address = new Dto.Address();
        ownerships: Dto.Ownership[] = [];
        activities: Dto.Activity[] = [];
        
        constructor (property?: IProperty)
        {
            if (property) {
                this.id = property.id;
                this.address = new Address();
                angular.extend(this.address, property.address);

                this.ownerships = property.ownerships.map((ownership: IOwnership) => { return new Dto.Ownership(ownership) });
            }
        }
    }
}