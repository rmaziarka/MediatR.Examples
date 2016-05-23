/// <reference path="../../../../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    describe('Given propertyView', () => {
        it('when no areas breakdown then total area should be 0', () => {
            var property = new Business.PropertyView();

            // arrange
            expect(property.totalAreaBreakdown).toBe(0);
        });

        it('when area breakdown is added then total area should be recalculated', () => {
            var property = Antares.TestHelpers.PropertyGenerator.generatePropertyView();
            property.propertyAreaBreakdowns = [];

            var area1 = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate({ size: 10.4 });
            var area2 = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate({ size: 10.51 });

            // act & arrange
            property.propertyAreaBreakdowns.push(area1);
            expect(property.totalAreaBreakdown).toBe(10.4);

            property.propertyAreaBreakdowns.push(area2);
            expect(property.totalAreaBreakdown).toBe(20.91);
        });
    });
}