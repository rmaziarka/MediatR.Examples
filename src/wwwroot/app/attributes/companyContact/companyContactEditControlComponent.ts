/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('companyContactEditControl', {
        templateUrl:'app/attributes/companyContact/companyContactEditControl.html',
            controllerAs: 'vm',
            controller:'CompanyContactEditControlController',
            bindings: {
                companyContact: '=',
                config:'<',
                schema: '<',
                type: '<',
                isPanelVisible:'<'
            }
    });
}