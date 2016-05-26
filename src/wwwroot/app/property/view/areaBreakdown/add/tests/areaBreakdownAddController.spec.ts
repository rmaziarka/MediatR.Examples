/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AreaBreakdownAddController = Antares.Property.View.AreaBreakdown.AreaBreakdownAddController;
    import Dto = Common.Models.Dto;
    import Business = Antares.Common.Models.Business;
    import Resources = Common.Models.Resources;

    describe('Given add area breakdown controller', () =>{
        var controller: AreaBreakdownAddController,
            $scope: ng.IScope,
            $http: ng.IHttpBackendService;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService,
            $httpBackend: ng.IHttpBackendService) => {

            var scope: ng.IScope = $rootScope.$new();
            $http = $httpBackend;

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

        it('when saveAreas then proper data should be send to API', () => {
            // arrange
            spyOn(controller, 'isValid').and.returnValue(true);

            var propertyIdMock = '123456',
                url = '/api/properties/' + propertyIdMock + '/areabreakdown';

            controller.addNewArea();
            controller.areas[0].name = 'TestArea1';
            controller.areas[1].name = 'TestArea2';

            //act
            var requestData: Resources.ICreatePropertyAreaBreakdownResourceClassData;
            $http.expectPOST(new RegExp(url), (data: string) => {
                requestData = JSON.parse(data);

                return true;
            }).respond(200, []);

            controller.saveAreas(propertyIdMock);
            $http.flush();

            // assert
            expect(requestData.areas.map((area) => new Business.CreatePropertyAreaBreakdownResource(area))).toEqual(controller.areas);
        });

        it('when saveAreas then proper data should returned in promise form API', () => {
            // arrange
            spyOn(controller, 'isValid').and.returnValue(true);

            var propertyIdMock = '123456',
                url = '/api/properties/' + propertyIdMock + '/areabreakdown';

            var mockResponsedata: Dto.IPropertyAreaBreakdown[] = [{ id: 'id1', name: 'TestArea1', size: 1, order: 1 }, { id: 'id2', name: 'TestArea2', size: 500, order: 2 }];

            $http.expectPOST(new RegExp(url)).respond(200, mockResponsedata);

            controller.addNewArea();
            controller.areas[0].name = 'TestArea1';
            controller.areas[1].name = 'TestArea2';

            //act
            var returnedData: Business.PropertyAreaBreakdown[];
            controller.saveAreas(propertyIdMock).then((data) =>{
                returnedData = data;
            });

            $http.flush();

            // assert
            expect(returnedData).toEqual(mockResponsedata.map((area) => new Business.PropertyAreaBreakdown(area)));
        });
    });
}