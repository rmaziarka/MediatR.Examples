/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('activityAddCard', {
        templateUrl: 'app/activity/addCard/activityAddCard.html',
        controllerAs: 'vm',
        controller: 'ActivityAddCardController',
        bindings: {
            propertyTypeId: '<',
            ownerships: '<',
            config:'<',
            onSave: '&',
            onCancel: '&',
            isPristine: '<'
        }
    });
}