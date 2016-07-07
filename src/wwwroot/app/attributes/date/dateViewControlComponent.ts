/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('dateViewControl', {
        templateUrl:'app/attributes/date/dateViewControl.html',
            controllerAs: 'vm',
            bindings: {
                date: '<',
                config:'<',
                schema: '<'
            }
    });
}