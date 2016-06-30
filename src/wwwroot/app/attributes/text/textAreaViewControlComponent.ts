/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('textareaViewControl', {
        templateUrl: 'app/attributes/text/textAreaViewControl.html',
        controllerAs: 'vm',
        bindings: {
            ngModel: '<',
            config: '<',
            schema: '<'
        }
    });
}