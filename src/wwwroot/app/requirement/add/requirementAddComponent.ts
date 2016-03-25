/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement {
    angular.module('app').component('requirementAdd', {
        templateUrl: 'app/requirement/add/requirementAdd.html',
        controllerAs: 'vm',
        controller: 'requirementAddController',
        bindings: {
            userData: '<'
        }
    });
}