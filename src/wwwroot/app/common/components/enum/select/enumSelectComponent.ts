/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('enumSelect', {
        templateUrl: 'app/common/components/enum/select/enumSelect.html',
        controllerAs: 'vm',
        controller: 'EnumSelectController',
        bindings: {
            id: '@',
            name: '@',
            required: '<',
            ngModel: '=',
            enumTypeCode: '@',
            hideEmptyValue: '<',
            sort: '<'
        }
    });
}