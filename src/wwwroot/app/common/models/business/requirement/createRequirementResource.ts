/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class CreateRequirementResource implements Dto.ICreateRequirementResource {
        contactIds: string[] = [];
        address: Dto.IAddress = null;

        minPrice: number = 0;
        maxPrice: number = 0;

        minBedrooms: number = 0;
        maxBedrooms: number = 0;

        minReceptionRooms: number = 0;
        maxReceptionRooms: number = 0;

        minBathrooms: number = 0;
        maxBathrooms: number = 0;

        minParkingSpaces: number = 0;
        maxParkingSpaces: number = 0;

        minArea: number = 0;
        maxArea: number = 0;

        minLandArea: number = 0;
        maxLandArea: number = 0;

        description: string = null;

        constructor(requirement?: Dto.IRequirement) {
            if (requirement) {
                this.contactIds = requirement.contacts.map((contact: Dto.IContact) => contact.id);
                this.address = requirement.address;

                this.minPrice = requirement.minPrice;
                this.maxPrice = requirement.maxPrice;

                this.minBedrooms = requirement.minBedrooms;
                this.maxBedrooms = requirement.maxBedrooms;

                this.minReceptionRooms = requirement.minReceptionRooms;
                this.maxReceptionRooms = requirement.maxReceptionRooms;

                this.minBathrooms = requirement.minBathrooms;
                this.maxBathrooms = requirement.maxBathrooms;

                this.minParkingSpaces = requirement.minParkingSpaces;
                this.maxParkingSpaces = requirement.maxParkingSpaces;

                this.minArea = requirement.minArea;
                this.maxArea = requirement.maxArea;

                this.minLandArea = requirement.minLandArea;
                this.maxLandArea = requirement.maxLandArea;

                this.description = requirement.description;
            }
        }
    }
}