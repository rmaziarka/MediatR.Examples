declare module Antares.Common.Models.Dto {
    interface IEnumDictionary {
        [id: string]: IEnumItem[],
        activityDepartmentType: IEnumItem[];
        activityDocumentType: IEnumItem[];
        activityStatus: IEnumItem[];
        activityUserType: IEnumItem[];
        division: IEnumItem[];
        entityType: IEnumItem[];
        offerStatus: IEnumItem[];
        ownershipType: IEnumItem[];
        salutationFormat: IEnumItem[];
    }
}