/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;

    export class ActivityAttendeesEditControlController {
        // binding
        users: Business.User[];
        contacts: Business.Contact[];
        attendees: Dto.IActivityAttendee[];
        config: any;

        // controller 
        availableAttendees: Business.UpdateActivityAttendee[];

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

        public getAvailableAttendees = (): Business.UpdateActivityAttendee[] => {
            var availableAttendees: Business.UpdateActivityAttendee[] = [];

            availableAttendees = this.attendees.map((attendee: Dto.IActivityAttendee) => new Business.UpdateActivityAttendee(attendee.user, attendee.contact, true));

            var attendeeUsersToAdd = this.users.map((user: Business.User) => new Business.UpdateActivityAttendee(user, null));
            this.addOnlyNotSelectedAttendeesToList(availableAttendees, attendeeUsersToAdd);

            var attendeeContactsToAdd: Business.UpdateActivityAttendee[] = this.contacts.map((contact: Business.Contact) => new Business.UpdateActivityAttendee(null, contact));
            this.addOnlyNotSelectedAttendeesToList(availableAttendees, attendeeContactsToAdd);

            return availableAttendees;
        }

        public onAttendeeSelected = (attendee: Business.UpdateActivityAttendee) => {
            this.updateSelectedAttendeesFromSelectedList();
        }

        private updateSelectedAttendeesFromSelectedList = () => {
            this.attendees = this.availableAttendees.filter((aa: Business.UpdateActivityAttendee) => { return aa.isSelected; });
        }

        private addOnlyNotSelectedAttendeesToList = (availableAttendees: Business.UpdateActivityAttendee[], attendeesToAdd: Business.UpdateActivityAttendee[]) => {
            attendeesToAdd.forEach((attendeeToAdd: Business.UpdateActivityAttendee) => {
                if (this.isAttendeeNotAddedToList(availableAttendees, attendeeToAdd)) {
                    availableAttendees.push(attendeeToAdd);
                }
            });
        }

        private isAttendeeNotAddedToList = (availableAttendees: Business.UpdateActivityAttendee[], attendeeToAdd: Business.UpdateActivityAttendee): boolean => {
            var attendeesOnList = availableAttendees.filter((addedAttendee: Business.UpdateActivityAttendee) => {
                return attendeeToAdd.getId() === addedAttendee.getId();
            });

            return attendeesOnList.length === 0;
        }
    }

    angular.module('app').controller('ActivityAttendeesEditControlController', ActivityAttendeesEditControlController);
};