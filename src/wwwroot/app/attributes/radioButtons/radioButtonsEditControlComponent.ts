/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('radioButtonsEditControl', {
        templateUrl: 'app/attributes/radioButtons/radioButtonsEditControl.html',
        controllerAs: 'vm',
        controller: 'RadioButtonsEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            schema: '<',
            onSelectedItemChanged: '&?'
        }
    });
}