/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('textViewControl', {
        templateUrl: 'app/attributes/text/textViewControl.html',
        controllerAs: 'vm',
        bindings: {
            text: '<',
            config: '<',
            schema: '<'
        }
    });
}