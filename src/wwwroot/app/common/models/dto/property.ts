module Antares.Common.Models.Dto {
    export class Property implements IProperty {
        constructor(
            public id: string = '',
            public address: Dto.Address = new Address(),
            public ownerships?: Dto.Ownership[],
            public activities?: Dto.Activity[]
        ){}
    }
}