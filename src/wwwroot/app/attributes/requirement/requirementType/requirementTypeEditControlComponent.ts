/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('requirementTypeEditControl', {
        templateUrl: 'app/attributes/requirement/requirementType/requirementTypeEditControl.html',
        controllerAs: 'vm',
        controller: 'RequirementTypeEditControlController',
        bindings: {
            config: '<',
            ngModel: '=',
            onRequirementTypeChanged: '&'
        }
    });
}