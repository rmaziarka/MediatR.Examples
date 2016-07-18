/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    angular.module('app').component('listViewControl', {
        templateUrl: 'app/attributes/listView/listViewControl.html',
        controllerAs: 'vm',
        controller: 'ListViewControlController',
        bindings: {
            config: '<',
            list: '<',
            schema: '<'
        }
    });
}