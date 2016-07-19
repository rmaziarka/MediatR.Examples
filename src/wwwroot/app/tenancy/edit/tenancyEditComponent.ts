/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    angular.module('app').component('tenancyEdit', {
        templateUrl: 'app/tenancy/edit/tenancyEdit.html',
        controllerAs: 'vm',
        controller: 'TenancyEditController',
        bindings: {
            tenancy: '<',
            config: '<'
        }
    });
}