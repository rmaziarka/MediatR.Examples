/// <reference path="../../typings/_all.d.ts" />

module Antares.TestHelpers {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    export class ActivityAttendeeGenerator {
        public static generateAttendeeContactDto(): Dto.IActivityAttendee {
            var contact = TestHelpers.ContactGenerator.generateDto();
            var activityAttendee: Dto.IActivityAttendee = {
                contact: contact,
                contactId: contact.id,
                user: null,
                userId: null
            };

            return activityAttendee;
        }

        public static generateAttendeeUserDto(): Dto.IActivityAttendee {
            var user = TestHelpers.UserGenerator.generateDto();
            var activityAttendee: Dto.IActivityAttendee = {
                contact: null,
                contactId: null,
                user: user,
                userId: user.id
            };

            return activityAttendee;
        }

        public static generateManyAttendeeContactDtos(n: number): Dto.IActivityAttendee[] {
            return _.times(n, () => { return ActivityAttendeeGenerator.generateAttendeeContactDto() });
        }

        public static generateManyAttendeeUserDtos(n: number): Dto.IActivityAttendee[] {
            return _.times(n, () => { return ActivityAttendeeGenerator.generateAttendeeUserDto() });
        }

        public static generateManyAttendeeContact(n: number): Attributes.Models.EditActivityAttendeeModel[] {
            return _.map<Dto.IActivityAttendee, Attributes.Models.EditActivityAttendeeModel>(ActivityAttendeeGenerator.generateManyAttendeeContactDtos(n), (aa: Dto.IActivityAttendee) => { return new Attributes.Models.EditActivityAttendeeModel(null, aa.contact); });
        }

        public static generateManyAttendeeUser(n: number): Attributes.Models.EditActivityAttendeeModel[] {
            return _.map<Dto.IActivityAttendee, Attributes.Models.EditActivityAttendeeModel>(ActivityAttendeeGenerator.generateManyAttendeeUserDtos(n), (aa: Dto.IActivityAttendee) => { return new Attributes.Models.EditActivityAttendeeModel(aa.user, null); });
        }

    }
}