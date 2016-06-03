declare module Antares.Common.Models.Dto {
    interface IEnumDictionary {
        [id:string]:IEnumItem[],
        entityType: IEnumItem[];
        ownershipType: IEnumItem[];
        activityStatus: IEnumItem[];
        division: IEnumItem[];
    }
}