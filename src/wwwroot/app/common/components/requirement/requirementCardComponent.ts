/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('requirementCard', {
        templateUrl : 'app/common/components/requirement/requirementCard.html',
        controllerAs : 'vm',
        controller : 'requirementCardController',
        bindings : {
            requirement: '<'
        }
    });
}