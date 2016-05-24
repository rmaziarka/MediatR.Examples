/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import PropertyViewController = Property.View.PropertyViewController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('Given property view controller', () => {
        var controller: PropertyViewController,
            $http: ng.IHttpBackendService,
            scope: ng.IScope;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            var propertyMock = TestHelpers.PropertyGenerator.generatePropertyView();

            scope = $rootScope.$new();

            var bindings = { property: propertyMock };
            controller = <PropertyViewController>$controller('propertyViewController', { $scope: scope }, bindings);
        }));

        describe('when saveArea is executed', () => {
            var $q: ng.IQService;
            var saveAreaDefferedMock: ng.IDeferred<Business.PropertyAreaBreakdown[]>;

            beforeEach(inject((
                _$q_: ng.IQService) => {

                $q = _$q_;

                saveAreaDefferedMock = $q.defer();

                spyOn(controller.components, 'areaAdd').and.returnValue({
                    saveAreas: (propertyId: string) => {
                        return saveAreaDefferedMock.promise;
                    }
                });
                spyOn(controller, 'cancelAddArea').and.callFake(() => { });
            }));

            it('then new areas should be added to the areas list', () => {
                // arrange
                var newAreasCount: number = 3;
                var areas: Business.PropertyAreaBreakdown[] = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generateMany(newAreasCount);

                // act 
                controller.saveArea();
                saveAreaDefferedMock.resolve(areas);
                scope.$apply();

                // assert
                expect(controller.property.propertyAreaBreakdowns.length).toBe(newAreasCount);
                expect(controller.property.propertyAreaBreakdowns).toContain(areas[0]);
                expect(controller.property.propertyAreaBreakdowns).toContain(areas[1]);
                expect(controller.property.propertyAreaBreakdowns).toContain(areas[2]);
            });

            it('then side panel for adding areas should be hiden', () => {
                // act
                controller.saveArea();
                saveAreaDefferedMock.resolve();
                scope.$apply();

                // assert
                expect(controller.cancelAddArea).toHaveBeenCalled();
            });
        });

        describe('when area is dragged and dropped', () => {
            var $http: ng.IHttpBackendService,
                event: Common.Models.IDndEvent,
                areaToDrag: Business.PropertyAreaBreakdown,
                newIndexOfDraggedArea: number = 1;

            beforeEach(inject((
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;
                areaToDrag = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();

                controller.property.propertyAreaBreakdowns.push(areaToDrag);

                var eventSource: Common.Models.IDndEventSource = <Common.Models.IDndEventSource>{
                    itemScope: {
                        modelValue: areaToDrag
                    }
                }

                var eventDest: Common.Models.IDndEventDest = <Common.Models.IDndEventDest>{
                    index: newIndexOfDraggedArea
                }

                event = <Common.Models.IDndEvent>{
                    source: eventSource,
                    dest: eventDest
                };

            }));

            it('then new position should be saved', () => {
                // arrange
                var requestData: Dto.IUpdatePropertyAreaBreakdownOrderResource;

                $http.expectPUT(/\/api\/properties\/[0-9a-zA-Z]*\/areabreakdown\/order/, (data: string) => {
                    requestData = JSON.parse(data);
                    return true;
                }).respond(201, {});

                // act
                controller.onAreaDraggedAndDropped()(event);
                scope.$apply();

                // assert
                expect(requestData).not.toBe(null);
                expect(requestData.areaId).toBe(areaToDrag.id);
                expect(requestData.order).toBe(newIndexOfDraggedArea);
                expect(requestData.propertyId).toBe(controller.property.id);
            });
        });
    });
}