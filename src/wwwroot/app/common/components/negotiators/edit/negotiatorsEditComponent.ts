/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('negotiatorsEdit', {
        templateUrl: 'app/common/components/negotiators/edit/negotiatorsEdit.html',
        controllerAs: 'nevm',
        controller: 'NegotiatorsController',
        bindings: {
            activityId: '<',
            propertyDivisionId: '@',
            departments: '=',
            leadNegotiator: '=' ,
            secondaryNegotiators: '='
        }
    });
}