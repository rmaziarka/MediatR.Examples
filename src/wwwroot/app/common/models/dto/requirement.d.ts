declare module Antares.Common.Models.Dto {
    interface IRequirement {
        id: string;
        createDate: Date;
        minPrice: number;
        maxPrice: number;
        minBedrooms: number;
        maxBedrooms: number;
        minReceptionRooms: number;
        maxReceptionRooms: number;
        minBathrooms: number;
        maxBathrooms: number;
        minParkingSpaces: number;
        maxParkingSpaces: number;
        minArea: number;
        maxArea: number;
        minLandArea: number;
        maxLandArea: number;
        note: string;
    }
}