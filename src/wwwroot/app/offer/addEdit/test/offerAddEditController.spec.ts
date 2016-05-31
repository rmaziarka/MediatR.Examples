/// <reference path="../../../typings/_all.d.ts" />

// ReSharper disable InconsistentNaming
declare module jasmine {
    interface Matchers {
        // ReSharper restore InconsistentNaming
        toBeSameAsOffer(expected: any): void;
    }
}

module Antares {
    import OfferAddEditController = Component.OfferAddEditController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    var offerCustomMatchers: jasmine.CustomMatcherFactories = {
        toBeSameAsOffer : (util: jasmine.MatchersUtil): jasmine.CustomMatcher => ({
            compare : (actual: Business.Offer, expected: Business.Offer): jasmine.CustomMatcherResult =>{
                var result: jasmine.CustomMatcherResult = {
                    pass : false,
                    message : ""
                };

                result.pass = util.equals(actual.requirementId, expected.requirementId)
                    && util.equals(actual.id, expected.id)
                    && util.equals(actual.activityId, expected.activityId)
                    && util.equals(actual.statusId, expected.statusId)
                    && util.equals(actual.specialConditions, expected.specialConditions)
                    && util.equals(actual.price, expected.price)
                    && moment(actual.offerDate).isSame(expected.offerDate)
                    && moment(actual.exchangeDate).isSame(expected.exchangeDate)
                    && moment(actual.completionDate).isSame(expected.completionDate);

                if (!result.pass) {
                    result.message = "Expected actual offer: " + JSON.stringify(actual, null, 4) + " to be same as expected offer: " + JSON.stringify(expected, null, 4);
                }
                else {
                    // if used with .not
                    result.message = "Expected actual offer to be different than expected offer but they are the same.";
                }

                return result;
            }
        })
    }

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

        beforeEach(() =>{
            jasmine.addMatchers(offerCustomMatchers);
        });

        describe('in add mode', () =>{
            beforeEach(() =>{
                controller.mode = "add";
            });

            it('when saveOffer() is called then proper data should be sent to API in POST request', () =>{
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
                expect(requestData).toBeSameAsOffer(offerMock);
                expect(controller.isDataValid).toHaveBeenCalledTimes(1);
                // ReSharper disable once ExpressionIsAlwaysConst
                expect(resultIsPromise).toBeTruthy();
            });
        });
        describe('in edit mode', () =>{
            beforeEach(() =>{
                controller.mode = "edit";
            });

            it('when saveOffer() is called then proper data should be sent to API in PUT request', () =>{
                // arrange
                spyOn(controller, 'isDataValid').and.returnValue(true);

                controller.offer = offerMock;
                controller.selectedStatus = offerStatuses[0];

                var requestData: Dto.IOffer = <Dto.IOffer>{};

                $http.expectPUT(/\/api\/offer/, (data: string) =>{
                    requestData = JSON.parse(<string>data);
                    return true;
                }).respond(200, {});

                // act
                var result: any = controller.saveOffer();
                var resultIsPromise = false;
                result.then(() => resultIsPromise = true);

                $http.flush();

                // assert
                expect(requestData).toBeSameAsOffer(offerMock);
                expect(controller.isDataValid).toHaveBeenCalledTimes(1);
                // ReSharper disable once ExpressionIsAlwaysConst
                expect(resultIsPromise).toBeTruthy();
            });
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
}