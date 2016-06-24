declare module Antares.Common.Models.Dto {
    interface IRequirement {
        [index: string]: any;

        id: string;

        contacts: IContact[];
        address: IAddress;
        createDate: Date;
        requirementNotes: IRequirementNote[];

        minPrice?: number;
        maxPrice?: number;

        minBedrooms?: number;
        maxBedrooms?: number;

        minReceptionRooms?: number;
        maxReceptionRooms?: number;

        minBathrooms?: number;
        maxBathrooms?: number;

        minParkingSpaces?: number;
        maxParkingSpaces?: number;

        minArea?: number;
        maxArea?: number;

        minLandArea?: number;
        maxLandArea?: number;

        description?: string;

        viewings?: IViewing[];
        offers?: IOffer[];

        attachments: IAttachment[];
    }
}