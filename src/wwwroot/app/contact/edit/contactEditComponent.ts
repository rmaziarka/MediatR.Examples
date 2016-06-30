/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    angular.module('app').component('contactEdit', {
        templateUrl: 'app/contact/edit/contactEdit.html',
        controllerAs: 'vm',
        controller: 'ContactEditController',
        bindings: {
            contact: '<',  
            userData: '<'
        }
    });
}