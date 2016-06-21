/// <reference path="../../typings/_all.d.ts" />

module Antares.Contact {
    angular.module('app').component('contactAdd', {
        templateUrl: 'app/contact/add/contactAdd.html',
        controllerAs: 'vm',
        controller: 'ContactAddController',
        bindings: {
            contact: '<',  
            userData: '<'
        }
    });
}