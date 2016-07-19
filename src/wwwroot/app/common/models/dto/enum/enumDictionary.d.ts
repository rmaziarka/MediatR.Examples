declare module Antares.Common.Models.Dto {
    interface IEnumDictionary {
        [id: string]: IEnumItem[],
        activityDepartmentType: IEnumItem[];
        activityDocumentType: IEnumItem[];
        activityStatus: IEnumItem[];
        userType: IEnumItem[];
        division: IEnumItem[];
        entityType: IEnumItem[];
        offerStatus: IEnumItem[];
        ownershipType: IEnumItem[];
        salutationFormat: IEnumItem[];
        rentPaymentPeriod: IEnumItem[];
        salesBoardStatus: IEnumItem[];
        salesBoardType: IEnumItem[];
    }
}