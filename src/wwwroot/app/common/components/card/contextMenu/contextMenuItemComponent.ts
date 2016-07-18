/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('contextMenuItem', {
        templateUrl: 'app/common/components/card/contextMenu/contextMenuItem.html',
        controllerAs: 'vm',
        controller: 'ContextMenuItemController',
        bindings: {
            type: '@',
            action: '<',
            item: '<',
            external: '<'
        }
    });
}