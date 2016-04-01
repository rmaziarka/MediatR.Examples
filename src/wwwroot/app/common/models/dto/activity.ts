module Antares.Common.Models.Dto {
    export class Activity implements IActivity {
        constructor(
            public propertyId: string,
            public activityStatusId: string,
            public contacts?: Contact[]
        ) { }
    }
}