/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module("app").component("offerView", {
        controller: 'offerViewController',
        controllerAs: 'vm',
        templateUrl: 'app/offer/view/offerView.html',
        bindings: {
            offer: '<',
            config: '<'
        }
    });
}