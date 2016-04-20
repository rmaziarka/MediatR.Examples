/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('attributeList', {
        controller: 'attributeListController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/attribute/list/attributeList.html',
        bindings: {
            userData: '<',
            componentId: '<',
            property: '=',
            mode: '@'
        }
    });
}