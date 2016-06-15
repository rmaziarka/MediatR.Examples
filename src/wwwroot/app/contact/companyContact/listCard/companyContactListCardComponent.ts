/// <reference path="../../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('companyContactListCard', {
        controller: 'companyContactListCardController',
        controllerAs: 'vm',
        templateUrl: 'app/contact/companyContact/listCard/companyContactListCard.html',
        transclude: true,
        bindings: {
            contacts: '=',
            isLoading: '<',
            onCancel: '&',
            onSave: '<',
            allowMultipleSelect: '<'
        }
    });
}