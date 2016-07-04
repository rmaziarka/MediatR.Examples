/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    export class ActivityAppraisalMeetingDateEditControlController {
        // bindings 
        appraisalMeetingStart: Date;
        appraisalMeetingEnd: Date;
        config: IActivityAppraisalMeetingDateControlConfig;

        // controllers
        meetingDateOpen: boolean = false;

        today: Date = new Date();
        meetingDate: Date;
        meetingStartTime: moment.Moment;
        meetingEndTime: moment.Moment;

        constructor() {
            this.meetingDate = this.appraisalMeetingStart ? moment(this.appraisalMeetingStart).toDate() : moment(this.today).toDate();
            this.meetingStartTime = this.appraisalMeetingStart ? moment(this.appraisalMeetingStart) : moment(this.today);
            this.meetingEndTime = this.appraisalMeetingEnd ? moment(this.appraisalMeetingEnd) : moment(this.today).add(1, 'hours');
        }

        isRequired() {
            return this.config.appraisalMeetingStart.required || this.config.appraisalMeetingEnd.required;
        }

        onDatesChanged() {
            this.appraisalMeetingStart = this.combineDateWithTime(this.meetingDate, moment(this.meetingStartTime).toDate());
            this.appraisalMeetingEnd = this.combineDateWithTime(this.meetingDate, moment(this.meetingEndTime).toDate());
        }

        combineDateWithTime(date: Date, time: Date): Date {
            return new Date(
                (<Date>date).getFullYear(),
                (<Date>date).getMonth(),
                (<Date>date).getDate(),
                time.getHours(),
                time.getMinutes(),
                time.getSeconds(),
                time.getMilliseconds()
            );
        }

        openMeetingDate() {
            this.meetingDateOpen = true;
        }
    }

    angular.module('app').controller('ActivityAppraisalMeetingDateEditControlController', ActivityAppraisalMeetingDateEditControlController);
};