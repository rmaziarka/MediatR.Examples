declare module Antares.Common.Models.Dto {
    interface ICompany {
        id: string;
        name: string;        
        contacts: IContact[];        
    }
}