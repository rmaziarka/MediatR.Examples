/// <reference path="../../typings/_all.d.ts" />

module Antares.Tenancy {
    angular.module('app').component('tenancyAdd', {
        templateUrl: 'app/tenancy/add/tenancyAdd.html',
        controllerAs: 'tavm',
        controller: 'TenancyAddController',
        bindings: {
            activity: '<',
            requirement: '<'
        }
    });
}