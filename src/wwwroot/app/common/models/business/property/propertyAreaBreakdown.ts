module Antares.Common.Models.Business {

    export class PropertyAreaBreakdown implements Dto.IPropertyAreaBreakdown {
        id: string = null;
        name: string = null;
        size: number = null;
        order: number = null;

        constructor(propertyArea?: Dto.IPropertyAreaBreakdown)
        {
            if (propertyArea) {
                angular.extend(this, propertyArea);
            }
        }

    }
}