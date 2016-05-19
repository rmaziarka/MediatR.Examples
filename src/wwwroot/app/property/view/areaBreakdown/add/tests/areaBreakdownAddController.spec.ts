/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AreaBreakdownAddController = Antares.Property.View.AreaBreakdown.AreaBreakdownAddController;
    import Dto = Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    describe('Given add area breakdown controller', () => {
        var controller: AreaBreakdownAddController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService) => {

            var scope: ng.IScope = $rootScope.$new();

            controller = <AreaBreakdownAddController>$controller('AreaBreakdownAddController', { $scope: scope });
        }));

        it('when is initiated then one empty area should be added as default', () => {
            expect(controller.areas.length).toBe(1);

            var area: Business.CreatePropertyAreaBreakdownResource = controller.areas[0];
            expect(area.size).toBeNull();
            expect(area.name).toBeNull();
        });

        it('when addNewArea is called then one empty area should be added to the areas list', () => {
            // arrange
            var areasCount = controller.areas.length;

            // act
            controller.addNewArea();

            expect(controller.areas.length).toBe(areasCount + 1);
        });

        it('when removeArea is called then one area should be removed from the areas list', () => {
            // arrange
            controller.addNewArea();
            controller.addNewArea();
            var area: Business.CreatePropertyAreaBreakdownResource = controller.areas[1];
            var areasCount = controller.areas.length;

            // act
            controller.removeArea(area);

            // assert
            expect(controller.areas.length).toBe(areasCount - 1);

            var removedAreas: Business.CreatePropertyAreaBreakdownResource[] = controller.areas.filter((a: Business.CreatePropertyAreaBreakdownResource) => {
                return a === area;
            });            
            expect(removedAreas.length).toBe(0);
        });

        it('when cleared then one empty area should be defined', () => {
            // arrange
            controller.addNewArea();
            controller.addNewArea();

            // act
            controller.clearAreas();

            // assert
            expect(controller.areas.length).toBe(1);
            var area: Business.CreatePropertyAreaBreakdownResource = controller.areas[0];
            expect(area.size).toBeNull();
            expect(area.name).toBeNull();
        });

    });
}