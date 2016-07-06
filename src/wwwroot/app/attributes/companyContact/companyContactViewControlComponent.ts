/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('companyContactViewControl', {
        templateUrl:'app/attributes/companyContact/companyContactViewControl.html',
            controllerAs: 'vm',
            bindings: {
                companyContact: '<',
                config:'<',
                schema: '<'
            }
    });
}