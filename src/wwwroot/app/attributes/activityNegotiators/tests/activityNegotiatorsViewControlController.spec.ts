/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityNegotiatorsViewControlController = Antares.Common.Component.ActivityNegotiatorsViewControlController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given view negotiators controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ActivityNegotiatorsViewControlController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            $scope = $rootScope.$new();
            $http = $httpBackend;

            var scopeMock = {
                negotiatorForm: {
                    callDate: {}
                }
            }

            var bindings = { activityId: 'testId', departments: <Business.ActivityDepartment[]>[] };
            controller = <ActivityNegotiatorsViewControlController>$controller('ActivityNegotiatorsViewControlController', { $scope: scopeMock }, bindings)

        }));

        describe('when updateNegotiatorCallDate is executed for lead negotiator', () => {
            var leadNegotiator: Business.ActivityUser;

            beforeEach(() => {
                leadNegotiator = Antares.TestHelpers.ActivityUserGenerator.generate(Common.Models.Enums.NegotiatorTypeEnum.LeadNegotiator);
                controller.leadNegotiator = leadNegotiator;
            });

            it('then correct data should be sent to API', () => {
                // arrange
                var newCallDate: Date = moment().add(10, 'days').toDate();
                var requestData: Dto.IUpdateSingleActivityUserResource;

                $http.expectPUT(/\/api\/activities\/[0-9a-zA-Z]*\/negotiators/, (data: string) => {
                    requestData = JSON.parse(data);
                    requestData.callDate = moment(requestData.callDate).toDate();
                    return true;
                }).respond(201, {});

                // act
                controller.updateNegotiatorCallDate(controller.leadNegotiator)(newCallDate);
                $http.flush();

                // assert
                expect(requestData.id).toBe(leadNegotiator.id);
                expect(requestData.callDate).toEqual(Core.DateTimeUtils.createDateAsUtc(newCallDate));
            });

            it('then call date should be set for correct negotiator', () => {
                // arrange
                var newCallDate: Date = moment().add(10, 'days').toDate();

                $http.expectPUT(/\/api\/activities\/[0-9a-zA-Z]*\/negotiators/, (data: string) => {
                    return true;
                }).respond(201, {});

                // act
                controller.updateNegotiatorCallDate(controller.leadNegotiator)(newCallDate);
                $http.flush();

                // assert
                expect(controller.leadNegotiator.callDate).toEqual(newCallDate);
            });

        })
    });
}