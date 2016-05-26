/// <reference path="../../../../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    import runDescribe = TestHelpers.runDescribe;
    type TestCase = [string, boolean];

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

        runDescribe('when division code ')
            .data<TestCase>([
                ['Commercial', true],
                ['Residential', false]])
            .dataIt((data: TestCase) =>
                `is set to "${data[0]}" then isCommercial flag should be ${data[1] ? 'true' : 'false'}`)
            .run((data: TestCase) => {
                // arrange / act / assert
                var property = Antares.TestHelpers.PropertyGenerator.generatePropertyView();
                property.division.code = data[0];

                expect(property.isCommercial()).toBe(data[1]);
            });
    });
}