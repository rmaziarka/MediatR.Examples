/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    angular.module('app').component('requirementView', {
        templateUrl: 'app/requirement/view/requirementView.html',
        controllerAs: 'vm',
        controller: 'requirementViewController',
        bindings: {
            requirement: '<'
        }
    });
}