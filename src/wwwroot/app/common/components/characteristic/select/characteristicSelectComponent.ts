/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('characteristicSelect', {
        templateUrl: 'app/common/components/characteristic/select/characteristicSelect.html',
        controllerAs: 'vm',
        controller: 'CharacteristicSelectController',
        bindings: {
            characteristic: '<'
        }
    });
}