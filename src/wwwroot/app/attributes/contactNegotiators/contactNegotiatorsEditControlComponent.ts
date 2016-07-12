/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('contactNegotiatorsEditControl', {
        templateUrl: 'app/attributes/contactNegotiators/contactNegotiatorsEditControl.html',
        controllerAs: 'vm',
        controller: 'ContactNegotiatorsEditControlController',
        bindings: {
            contactId : '<',
            leadNegotiator : '=',
            secondaryNegotiators : '='
        }
    });
}