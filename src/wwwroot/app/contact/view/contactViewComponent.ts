/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    angular.module('app').component('contactView', {
        templateUrl: 'app/contact/view/contactView.html',
        controllerAs: 'vm',
        controller: 'ContactViewController',
        bindings: {
            contact: '<'
        }
    });
}