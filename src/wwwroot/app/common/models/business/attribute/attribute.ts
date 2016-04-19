module Antares.Common.Models.Business {

    export class Attribute implements Dto.IAttribute {
        order: number;
        nameKey: string;
        labelKey: string;
        min: string;
        max: string;
        unit: string;
        regEx: string;
        private static integerRegex = "[0-9]+";

        static mappings = [
            { nameKey : "Area", unit : "UNITS.SQUARE_FEET", regEx : "" },
            { nameKey : "LandArea", unit : "UNITS.SQUARE_FEET", regEx : "" },
            { nameKey : "Bedrooms", unit : "", regEx : Attribute.integerRegex },
            { nameKey : "Receptions", unit : "", regEx : Attribute.integerRegex },
            { nameKey : "Bathrooms", unit : "", regEx : Attribute.integerRegex },
            { nameKey : "GuestRooms", unit : "", regEx : Attribute.integerRegex },
            { nameKey : "FunctionRooms", unit : "", regEx : Attribute.integerRegex },
            { nameKey : "CarParkingSpaces", unit : "", regEx : Attribute.integerRegex },
        ];

        constructor(attribute?: Dto.IAttribute){
            if (attribute) {
                angular.extend(this, attribute);
                this.min = "min" + attribute.nameKey;
                this.max = "max" + attribute.nameKey;

                var mapping = Attribute.mappings.filter((item: any): boolean =>{
                    return item.nameKey === attribute.nameKey;
                });

                if (mapping.length > 0) {
                    this.unit = mapping[0].unit;
                    this.regEx = mapping[0].regEx;
                }
            }
        }
    }
}