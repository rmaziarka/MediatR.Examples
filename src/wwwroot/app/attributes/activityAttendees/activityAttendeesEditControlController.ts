/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityAttendeesEditControlController {
        // binding
        users: Business.User[];
        contacts: Business.Contact[];
        attendees: Dto.IActivityAttendee[];
        config: IActivityAttendeesEditControlConfig;

        // controller 
        availableAttendees: Models.EditActivityAttendeeModel[];

        constructor() {
            this.attendees = this.attendees ? this.attendees : [];
            this.users = this.users ? this.users : [];
            this.contacts = this.contacts ? this.contacts : [];

            this.availableAttendees = this.getAvailableAttendees();
        }

        $onChanges = (changesObj: any) => {
            this.availableAttendees = this.getAvailableAttendees();
            this.updateSelectedAttendeesFromSelectedList();
        }

        public getAvailableAttendees = (): Models.EditActivityAttendeeModel[] => {
            var availableAttendees: Models.EditActivityAttendeeModel[] = [];

            availableAttendees = this.attendees.map((attendee: Dto.IActivityAttendee) => new Models.EditActivityAttendeeModel(attendee.user, attendee.contact, true));

            var attendeeUsersToAdd = this.users.map((user: Business.User) => new Models.EditActivityAttendeeModel(user, null));
            this.addOnlyNotSelectedAttendeesToList(availableAttendees, attendeeUsersToAdd);

            var attendeeContactsToAdd: Models.EditActivityAttendeeModel[] = this.contacts.map((contact: Business.Contact) => new Models.EditActivityAttendeeModel(null, contact));
            this.addOnlyNotSelectedAttendeesToList(availableAttendees, attendeeContactsToAdd);

            return availableAttendees;
        }

        public onAttendeeSelected = (attendee: Models.EditActivityAttendeeModel) => {
            this.updateSelectedAttendeesFromSelectedList();
        }

        private updateSelectedAttendeesFromSelectedList = () => {
            this.attendees = this.availableAttendees.filter((aa: Models.EditActivityAttendeeModel) => { return aa.isSelected; });
        }

        private addOnlyNotSelectedAttendeesToList = (availableAttendees: Models.EditActivityAttendeeModel[], attendeesToAdd: Models.EditActivityAttendeeModel[]) => {
            attendeesToAdd.forEach((attendeeToAdd: Models.EditActivityAttendeeModel) => {
                if (this.isAttendeeNotAddedToList(availableAttendees, attendeeToAdd)) {
                    availableAttendees.push(attendeeToAdd);
                }
            });
        }

        private isAttendeeNotAddedToList = (availableAttendees: Models.EditActivityAttendeeModel[], attendeeToAdd: Models.EditActivityAttendeeModel): boolean => {
            var attendeesOnList = availableAttendees.filter((addedAttendee: Models.EditActivityAttendeeModel) => {
                return attendeeToAdd.getId() === addedAttendee.getId();
            });

            return attendeesOnList.length === 0;
        }
    }

    angular.module('app').controller('ActivityAttendeesEditControlController', ActivityAttendeesEditControlController);
};