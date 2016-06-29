declare module Antares.Common.Models.Dto {
    interface IUserData {
        id: string;
        firstName: string;
        lastName: string;
        name: string;
        email: string;
        country: string;
        division: IEnumTypeItem;
        roles: string[];
        department: IDepartment;
    }
}