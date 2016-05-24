/// <reference path="../../../../../typings/_all.d.ts" />

module Antares {
    import AreaBreakdownEditController = Antares.Property.View.AreaBreakdown.AreaBreakdownEditController;
    import Dto = Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    describe('Given area breakdown edit controller', () =>{
        var controller: AreaBreakdownEditController,
            $http: ng.IHttpBackendService;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: ng.IControllerService,
            $httpBackend: ng.IHttpBackendService) => {

            var scope: ng.IScope = $rootScope.$new();
            $http = $httpBackend;

            controller = <AreaBreakdownEditController>$controller('AreaBreakdownEditController', { $scope: scope });
        }));

        it('when editPropertyAreaBreakdown is called then proper area should be set', () => {
            // arrange
            var areaToUpdate = TestHelpers.PropertyAreaBreakdownGenerator.generate();

            // act
            controller.editPropertyAreaBreakdown(areaToUpdate);

            // assert
            expect(controller.propertyAreaBreakdown.id).toEqual(areaToUpdate.id);
            expect(controller.propertyAreaBreakdown.name).toEqual(areaToUpdate.name);
            expect(controller.propertyAreaBreakdown.size).toEqual(areaToUpdate.size);
        });

        it('when updatePropertyAreaBreakdown then proper data should be send to API', () => {
            // arrange
            spyOn(controller, 'isDataValid').and.returnValue(true);

            var areaToUpdate = TestHelpers.PropertyAreaBreakdownGenerator.generate();
            var propertyIdMock = '123456',
                url = '/api/properties/' + propertyIdMock + '/areabreakdown';

            controller.editPropertyAreaBreakdown(areaToUpdate);

            //act
            var requestData: Dto.IUpdatePropertyAreaBreakdownResource;
            $http.expectPUT(new RegExp(url), (data: string) => {
                requestData = JSON.parse(data);

                return true;
            }).respond(200, {});

            controller.updatePropertyAreaBreakdown(propertyIdMock);
            $http.flush();

            // assert
            expect(requestData.id).toEqual(controller.propertyAreaBreakdown.id);
            expect(requestData.name).toEqual(controller.propertyAreaBreakdown.name);
            expect(requestData.size).toEqual(controller.propertyAreaBreakdown.size);
        });

        it('when updatePropertyAreaBreakdown then proper data should be returned in promise form API', () => {
            // arrange
            spyOn(controller, 'isDataValid').and.returnValue(true);

            var areaToUpdate = Antares.TestHelpers.PropertyAreaBreakdownGenerator.generate();
            var propertyIdMock = '123456',
                url = '/api/properties/' + propertyIdMock + '/areabreakdown';

            controller.editPropertyAreaBreakdown(areaToUpdate);
            var mockResponsedata: Dto.IPropertyAreaBreakdown = { id: 'id1', name: 'TestArea1', size: 1, order: 1 };

            //act
            var returnedData: Business.PropertyAreaBreakdown;
            $http.expectPUT(new RegExp(url)).respond(200, mockResponsedata);

            controller.updatePropertyAreaBreakdown(propertyIdMock).then((data) => {
                returnedData = data;
            });
            $http.flush();

            // assert
            expect(returnedData.id).toEqual(mockResponsedata.id);
            expect(returnedData.name).toEqual(mockResponsedata.name);
            expect(returnedData.size).toEqual(mockResponsedata.size);
        });
    });
}