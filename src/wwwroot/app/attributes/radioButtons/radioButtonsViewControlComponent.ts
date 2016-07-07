/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('radioButtonsViewControl', {
        templateUrl: 'app/attributes/radioButtons/radioButtonsViewControl.html',
        controllerAs: 'vm',
        bindings: {
            config: '<',
            ngModel: '<',
            schema: '<'
        }
    });
}