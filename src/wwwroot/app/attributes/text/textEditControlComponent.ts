/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('textEditControl', {
        templateUrl: 'app/attributes/text/textEditControl.html',
        controllerAs: 'vm',
        controller: 'TextEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            schema: '<'
        }
    });
}