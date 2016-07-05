/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityAppraisalMeetingViewControl', {
        templateUrl: 'app/attributes/activityAppraisalMeeting/activityAppraisalMeetingViewControl.html',
        controllerAs: 'vm',
        controller: 'ActivityAppraisalMeetingViewControlController',
        bindings: {
            appraisalMeeting: '<',
            attendees: '<',
            config: '<'
        }
    });
}