/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {

    // !!! this component is not feature ready for data shaping - adjust as needed

    angular.module('app').component('selectUserEditControl', {
        templateUrl: 'app/attributes/selectUser/selectUserEditControl.html',
        controllerAs: 'vm',
        controller: 'SelectUserEditControlController',
        bindings: {
            config: '<',
            schema: '<',
            user: '='
        }
    });
}