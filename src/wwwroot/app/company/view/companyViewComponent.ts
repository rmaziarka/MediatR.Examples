/// <reference path="../../typings/_all.d.ts" />

module Antares.Company {
    angular.module('app').component('companyView', {
        templateUrl: 'app/company/view/companyView.html',
        controllerAs: 'vm',
        controller: 'CompanyViewController',
        bindings: {
            company: '<'
        }
    });
}