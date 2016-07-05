/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('requirementDescriptionEditControl', {
        templateUrl: 'app/attributes/requirement/requirementDescription/requirementDescriptionEditControl.html',
        controllerAs: 'vm',
        bindings: {
            config: '<',
            ngModel: '='
        }
    });
}