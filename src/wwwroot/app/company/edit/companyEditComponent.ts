/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {
    angular.module('app').component('companyEdit', {
        templateUrl: 'app/company/edit/companyEdit.html',
        controllerAs: 'vm',
        controller: 'CompanyEditController',
        bindings: {
            company: '<'
        }   
    });
}