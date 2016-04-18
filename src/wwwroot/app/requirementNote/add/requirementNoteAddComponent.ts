/// <reference path="../../typings/_all.d.ts" />

module Antares.RequirementNote {
    angular.module('app').component('requirementNoteAdd', {
        templateUrl: 'app/requirementNote/add/requirementNoteAdd.html',
        controllerAs : 'vm',
        controller: 'RequirementNoteAddController',
        bindings: {
            componentId: '<',
            requirement: '='
        }
    });
}