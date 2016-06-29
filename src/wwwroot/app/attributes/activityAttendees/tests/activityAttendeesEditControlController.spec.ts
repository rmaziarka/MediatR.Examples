/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes {
    import ActivityAttendeesEditControlController = Antares.Attributes.ActivityAttendeesEditControlController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given edit activity attendees controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: ActivityAttendeesEditControlController;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            $httpBackend: ng.IHttpBackendService) => {

            // init
            $scope = $rootScope.$new();
            $http = $httpBackend;

            var bindings = { contacts: TestHelpers.ContactGenerator.generateMany(2), users: TestHelpers.UserGenerator.generateMany(2) };
            controller = <ActivityAttendeesEditControlController>$controller('ActivityAttendeesEditControlController', { $scope: {} }, bindings);
        }));

        describe('when getting all available attendees', () => {

            it('with no selected attendees then all users and contacts should be displayed as not selected', () => {
                controller.attendees = [];

                var availableAttendees: Models.EditActivityAttendeeModel[] = controller.getAvailableAttendees();

                expect(availableAttendees.length).toBe(4);
                expectAttendeeOnList(availableAttendees, controller.contacts[0].id, false);
                expectAttendeeOnList(availableAttendees, controller.contacts[1].id, false);
                expectAttendeeOnList(availableAttendees, controller.users[0].id, false);
                expectAttendeeOnList(availableAttendees, controller.users[1].id, false);
            });

            it('with selected attendee is not in users and contacts list then all should be returned', () => {
                var contact: Dto.IContact = TestHelpers.ContactGenerator.generateDto();

                var attendee: Dto.IActivityAttendee = {
                    contact: contact,
                    contactId: contact.id,
                    user: null,
                    userId: null
                }

                controller.attendees.push(attendee);

                // act
                var availableAttendees: Models.EditActivityAttendeeModel[] = controller.getAvailableAttendees();

                // assert
                expect(availableAttendees.length).toBe(5);
                expectAttendeeOnList(availableAttendees, controller.contacts[0].id, false);
                expectAttendeeOnList(availableAttendees, controller.contacts[1].id, false);
                expectAttendeeOnList(availableAttendees, controller.users[0].id, false);
                expectAttendeeOnList(availableAttendees, controller.users[1].id, false);

                expectAttendeeOnList(availableAttendees, contact.id, true);
            });

            it('with selected attendee that is on users list then no duplicates should be returned', () => {
                var user: Dto.IUser = controller.users[0];

                var attendee: Dto.IActivityAttendee = {
                    contact: null,
                    contactId: null,
                    user: user,
                    userId: user.id
                }

                controller.attendees.push(attendee);

                // act
                var availableAttendees: Models.EditActivityAttendeeModel[] = controller.getAvailableAttendees();

                // assert
                expect(availableAttendees.length).toBe(4);
                expectAttendeeOnList(availableAttendees, controller.users[0].id, true);

                expectAttendeeOnList(availableAttendees, controller.users[1].id, false);
                expectAttendeeOnList(availableAttendees, controller.contacts[0].id, false);
                expectAttendeeOnList(availableAttendees, controller.contacts[1].id, false);
            });


            it('with selected attendee that is on contacts list then no duplicates should be returned', () => {
                var contact: Dto.IContact = controller.contacts[0];

                var attendee: Dto.IActivityAttendee = {
                    contact: contact,
                    contactId: contact.id,
                    user: null,
                    userId: null
                }

                controller.attendees.push(attendee);

                // act
                var availableAttendees: Models.EditActivityAttendeeModel[] = controller.getAvailableAttendees();

                // assert
                expect(availableAttendees.length).toBe(4);
                expectAttendeeOnList(availableAttendees, controller.contacts[0].id, true);

                expectAttendeeOnList(availableAttendees, controller.users[0].id, false);
                expectAttendeeOnList(availableAttendees, controller.users[1].id, false);
                expectAttendeeOnList(availableAttendees, controller.contacts[1].id, false);
            });
        });

        describe('when attendee selection has changed', () => {
            it('to selected then should be added to attendees list', () => {

                controller.availableAttendees[0].isSelected = true;
                controller.attendees = [controller.availableAttendees[0]];

                controller.availableAttendees[1].isSelected = true;
                controller.onAttendeeSelected(controller.availableAttendees[1]);

                expect(controller.attendees.length).toBe(2);
            });

            it('to deselected then should be removed from attendees list', () => {
                controller.availableAttendees[0].isSelected = true;
                controller.availableAttendees[1].isSelected = true;

                controller.attendees = [controller.availableAttendees[0], controller.availableAttendees[1]];

                controller.availableAttendees[0].isSelected = false;
                controller.onAttendeeSelected(controller.availableAttendees[0]);

                expect(controller.attendees.length).toBe(1);
            });
        });

        var expectAttendeeOnList = (list: Models.EditActivityAttendeeModel[], attendeeId: string, isSelectedExpected: boolean): void => {
            var attendee = isAttendeeOnList(list, attendeeId);
            expect(attendee).toBeTruthy();
            expect(attendee.isSelected).toBe(isSelectedExpected);
        }

        var isAttendeeOnList = (list: Models.EditActivityAttendeeModel[], attendeeId: string): Models.EditActivityAttendeeModel => {
            var attendeesOnList = list.filter((activityAttendee: Models.EditActivityAttendeeModel) => {
                return activityAttendee.getId() === attendeeId;
            });

            return attendeesOnList.length === 1 ? attendeesOnList[0] : null;
        }
    });
};