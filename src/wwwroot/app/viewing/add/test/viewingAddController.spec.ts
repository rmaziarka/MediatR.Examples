/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ViewingAddController = Antares.Component.ViewingAddController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    declare var moment: any;

    describe('Given viewing add controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ViewingAddController;

        var requirementMock = TestHelpers.RequirementGenerator.generate();
        var activityMock = TestHelpers.ActivityQueryResultGenerator.generate();
        var attendeesMock = TestHelpers.ContactGenerator.generateMany(3);
        var viewingMock = TestHelpers.ViewingGenerator.generate({ requirement: requirementMock, attendees: attendeesMock });
        var selectedAttendeesIds = attendeesMock.map((i: Business.Contact) => i.id).slice(2);

        describe('when save is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { requirement: requirementMock, attendees: attendeesMock };
                controller = <ViewingAddController>$controller('viewingAddController', { $scope: $scope }, bindings);
            }));

            describe('in add mode', () => {
                it('then proper data should be send to API', () => {
                    // arrange
                    controller.setViewing(viewingMock);
                    controller.setActivity(activityMock);
                    controller.setSelectedAttendees(selectedAttendeesIds);
                    controller.mode = "add";
                    spyOn(controller, 'isDataValid').and.returnValue(true);
                    var requestData: Dto.IViewing = new Business.Viewing();

                    $http.expectPOST(/\/api\/viewings/, (data: string) => {
                        requestData = JSON.parse(data);

                        return true;
                    }).respond(200, {});

                    // act
                    controller.saveViewing();
                    $http.flush();

                    // assert            
                    expect(requestData.requirementId).toBe(requirementMock.id);
                    expect(requestData.activityId).toBe(activityMock.id);
                    expect(requestData.attendeesIds).toEqual(selectedAttendeesIds);
                    expect(requestData.invitationText).toBe(viewingMock.invitationText);
                    expect(moment(requestData.startDate).isSame(viewingMock.startDate)).toBeTruthy();
                    expect(moment(requestData.endDate).isSame(viewingMock.endDate)).toBeTruthy();
                });
            });

            describe('in edit mode', () => {
                it('then proper data should be send to API', () => {
                    // arrange
                    controller.setViewing(viewingMock);
                    controller.setActivity(activityMock);
                    controller.setSelectedAttendees(selectedAttendeesIds);
                    controller.mode = "edit";
                    spyOn(controller, 'isDataValid').and.returnValue(true);
                    var requestData: Dto.IViewing = new Business.Viewing();

                    $http.expectPUT(/\/api\/viewings/, (data: string) => {
                        requestData = JSON.parse(data);

                        return true;
                    }).respond(200, {});
                    var expectedRequest = angular.copy(viewingMock);

                    // act
                    controller.saveViewing();
                    $http.flush();

                    // assert            
                    expect(requestData.requirementId).toBe(expectedRequest.requirementId);
                    expect(requestData.activityId).toBe(expectedRequest.activityId);
                    expect(requestData.attendeesIds).toEqual(selectedAttendeesIds);
                    expect(requestData.invitationText).toBe(expectedRequest.invitationText);
                    expect(moment(requestData.startDate).isSame(expectedRequest.startDate)).toBeTruthy();
                    expect(moment(requestData.endDate).isSame(expectedRequest.endDate)).toBeTruthy();
                });
            });
        });
    });
}