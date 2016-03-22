module Antares.Common.Models.Dto {
    export interface IProperty {
        id: string;
        address: Dto.Address;
    }

    export class Property implements IProperty {
        constructor(
            public id: string = '',
            public address: Dto.Address = new Address()
        ){}
    }
}