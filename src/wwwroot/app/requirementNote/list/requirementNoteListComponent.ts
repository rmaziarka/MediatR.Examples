/// <reference path="../../typings/_all.d.ts" />

module Antares.RequirementNote {
    angular.module('app').component('requirementNoteList', {
        templateUrl: 'app/requirementNote/list/requirementNoteList.html',
        controllerAs : 'rnlvm',
        controller: 'RequirementNoteListController',
        bindings: {
            componentId: '<',
            requirement: '<'
        }
    });
}