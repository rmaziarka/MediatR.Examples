/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('editableDate', {
        templateUrl: 'app/common/components/editableDate/editableDate.html',
        controllerAs: 'vm',
        controller: 'EditableDateController',
        bindings: {
            selectedDate: '=',
            isRequired: '<',
            onSave: '&'
        }
    });
}