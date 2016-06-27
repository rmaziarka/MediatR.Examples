/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('requirementTypeControl', {
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