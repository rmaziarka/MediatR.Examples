/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityAppraisalMeetingDateEditControl', {
        templateUrl: 'app/attributes/activityAppraisalMeetingDate/activityAppraisalMeetingDateEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityAppraisalMeetingDateEditControlController',
        bindings: {
            appraisalMeetingStart: '=',
            appraisalMeetingEnd: '=',
            config: '<'
        }
    });
}