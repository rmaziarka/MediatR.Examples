/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('textareaEditControl', {
        templateUrl: 'app/attributes/text/textAreaEditControl.html',
        controllerAs: 'vm',
        controller: 'TextEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            schema: '<',
            rows: '<'
        }
    });
}