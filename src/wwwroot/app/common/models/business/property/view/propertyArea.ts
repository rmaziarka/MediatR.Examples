module Antares.Common.Models.Business {

    export class PropertyArea {
        id: string = null;
        name: string = null;
        size: number = null;

        constructor(propertyArea?: Dto.IPropertyArea)
        {
            if (propertyArea) {
                angular.extend(this, propertyArea);
            }
        }

    }
}