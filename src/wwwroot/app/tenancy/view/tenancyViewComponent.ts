/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    angular.module('app').component('tenancyView', {
        templateUrl: 'app/tenancy/view/tenancyView.html',
        controllerAs: 'vm',
        controller: 'TenancyViewController',
        bindings: {
            tenancy: '<'
        }
    });
}