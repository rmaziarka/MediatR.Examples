/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('cardWithHeader', {
        templateUrl: 'app/common/components/card/itemWithHeader/cardWithHeader.html',
        controllerAs: 'vm',
        controller: 'CardController',
        transclude: {
            'contextMenu': '?cardContextMenu',
            'headerContent': '?cardHeaderContent',
            'template': '?cardTemplate'
        },
        bindings: {
            item: '<',
            cardTemplateUrl: '<',
            showItemDetails: '<',
            showContextMenu: '<',
            allowSelection: '<',
            cardSelected: '<',
            selected: '<'
        }
    });
}