/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('contactListCard', {
        controller: 'contactListCardController',
        controllerAs: 'vm',
        templateUrl: 'app/contact/listCard/contactListCard.html',
        transclude: true,
        bindings: {
            contacts: '=',
            isLoading: '<',
            onCancel: '&',
            onConfigure: '&?',
            onSave: '<',
            allowMultipleSelect: '<'
        }
    });
}