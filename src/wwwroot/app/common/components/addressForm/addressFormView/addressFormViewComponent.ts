/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('addressFormView', {
        controller: 'addressFormViewController',
        controllerAs: 'vm',
        templateUrl:'app/common/components/addressForm/addressFormView/addressFormView.html',
        bindings: {
            address: '<',
            templateUrl: '<',
            addressType: '@'
        }
    });
}