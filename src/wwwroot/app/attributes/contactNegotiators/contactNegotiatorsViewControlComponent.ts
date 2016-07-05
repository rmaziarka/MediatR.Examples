/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('contactNegotiatorsViewControl', {
        templateUrl: 'app/attributes/contactNegotiators/contactNegotiatorsViewControl.html',
        controllerAs: 'vm',
        controller: 'ContactNegotiatorsViewControlController',
        bindings: {
		    leadNegotiator : '<',
		    secondaryNegotiators : '<',
	    }
    });
}