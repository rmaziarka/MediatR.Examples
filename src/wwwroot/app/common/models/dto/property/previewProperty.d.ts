declare module Antares.Common.Models.Dto {
    interface IPreviewProperty {
        id: string;
        propertyTypeId: string;
        address: Dto.IAddress;
        divisionId: string
    }
}