/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('activityCard', {
        templateUrl: 'app/common/components/activity/activityCard.html',
        controllerAs : 'vm',
        controller: 'activityCardController',
        bindings : {
            activity: '<'
        }
    });
}