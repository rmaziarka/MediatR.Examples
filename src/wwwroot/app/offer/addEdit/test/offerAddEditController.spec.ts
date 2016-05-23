/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferAddEditController = Component.OfferAddEditController;
    import Dto = Common.Models.Dto;

    declare var moment: any;

    describe('Given offer add-edit controller ', () =>{
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: OfferAddEditController;

        var offerMock = TestHelpers.OfferGenerator.generate();
        var activityMock = TestHelpers.ActivityGenerator.generate();
        var requirementMock = TestHelpers.RequirementGenerator.generateDto();

        var offerStatuses = [
            { id : 1, code : 'New' },
            { id : 2, code : 'Closed' }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService,
            enumService: Mock.EnumServiceMock) =>{

            // enums
            enumService.setEnum(Dto.EnumTypeCode.OfferStatus.toString(), offerStatuses);

            $scope = $rootScope.$new();
            $http = $httpBackend;

            controller = <OfferAddEditController>$controller('offerAddEditController', { $scope : $scope });
            controller.requirement = requirementMock;
        }));

        describe('in add mode', () => {
            beforeEach(() =>{
                controller.mode = "add";
            });

            it('when saveOffer() is called then proper data should be sent to API', () =>{
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(true);

                controller.offer = offerMock;
                controller.selectedStatus = offerStatuses[0];

                var requestData: Dto.IOffer = <Dto.IOffer>{};

                $http.expectPOST(/\/api\/offer/, (data: string) =>{
                    requestData = JSON.parse(<string>data);
                    return true;
                }).respond(200, {});

                // act
                var result: any = controller.saveOffer();
                var resultIsPromise = false;
                result.then(() => resultIsPromise = true);

                $http.flush();

                // assert            
                expect(requestData.requirementId).toBe(offerMock.requirementId);
                expect(requestData.activityId).toBe(offerMock.activityId);
                expect(requestData.statusId).toEqual(offerMock.statusId);
                expect(requestData.specialConditions).toEqual(offerMock.specialConditions);
                expect(requestData.price).toEqual(offerMock.price);

                expect(moment(requestData.offerDate).isSame(offerMock.offerDate)).toBeTruthy();
                expect(moment(requestData.exchangeDate).isSame(offerMock.exchangeDate)).toBeTruthy();
                expect(moment(requestData.completionDate).isSame(offerMock.completionDate)).toBeTruthy();

                expect(controller.isDataValid).toHaveBeenCalledTimes(1);
                // ReSharper disable once ExpressionIsAlwaysConst
                expect(resultIsPromise).toBeTruthy();
            });

            it('when reset() is called then the model should be reset', () =>{
                // arrange
                controller.addOfferForm = {
                    $setPristine : jasmine.createSpy('$setPristine'),
                    $setUntouched : jasmine.createSpy('$setUntouched')
                };
                controller.activity = activityMock;
                controller.offer = offerMock;
                controller.statuses = offerStatuses;
                controller.selectedStatus = offerStatuses[1];
                controller.offer.offerDate = new Date(2016, 1, 1);

                // act
                controller.reset();

                // assert
                expect(controller.addOfferForm.$setPristine).toHaveBeenCalledTimes(1);
                expect(controller.addOfferForm.$setUntouched).toHaveBeenCalledTimes(1);
                expect(controller.offer.activityId).toBe(activityMock.id, 'Wrong activity id after reset');
                expect(controller.offer.requirementId).toBe(requirementMock.id, 'Wrong requirement id after reset');
                expect(controller.offer.completionDate).toBeUndefined('Completion date not reset');
                expect(controller.offer.exchangeDate).toBeUndefined('Exchange date not reset');
                expect(moment(controller.offer.offerDate).startOf('day').isSame(moment(new Date()).startOf('day'))).toBeTruthy('Wrong offer date after reset');
                expect(controller.offer.price).toBeUndefined('Price not reset');
                expect(controller.selectedStatus.code).toEqual(controller.defaultStatusCode, 'Wrong status after reset');
                expect(controller.offer.specialConditions).toBeUndefined('Special conditions not reset');
            });
        });
    });
}