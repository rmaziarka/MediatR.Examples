module Antares.Common.Models.Business {

    export class Attribute implements Dto.IAttribute {
        order: number;
        nameKey: string;
        labelKey: string;
        min: string;
        max: string;
        unit: string;

        unitMappings = [
            { nameKey: "Area", unit: "sq. ft" },
            { nameKey: "LandArea", unit: "sq. ft" }];

        constructor(attribute?: Dto.IAttribute) {
            if (attribute) {
                angular.extend(this, attribute);
                this.min = "min" + attribute.nameKey;
                this.max = "max" + attribute.nameKey;

                var mapping = this.unitMappings.filter((item: any): boolean => {
                    return item.nameKey === attribute.nameKey;
                });

                if (mapping.length > 0) {
                    this.unit = mapping[0].unit;
                }
            }
        }
    }
}