/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('negotiators', {
        templateUrl: 'app/common/components/negotiators/negotiators.html',
        controllerAs: 'nvm',
        controller: 'NegotiatorsController',
        bindings: {
            leadNegotiator: '=' ,
            secondaryNegotiators: '='
        }
    });
}