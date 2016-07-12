/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    angular.module('app').component('editSalutations', {
        templateUrl: 'app/contact/editSalutations/editSalutations.html',
        controllerAs: 'vm',
        controller: 'EditSalutationsController',
        transclude: true,
        bindings: {
            contact: '=',
            firstName: '<',
            lastName: '<',
            title: '<',
            defaultSalutationFormatId: '<'
        }
    });
}
