/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('card', {
        templateUrl: 'app/common/components/card/item/card.html',
        controllerAs: 'cvm',
        controller: 'CardController',
        transclude: {
            'contextMenu': '?cardContextMenu',
            'template': '?cardTemplate'
        },
        bindings: {
            item: '<',
            cardTemplateUrl: '<',
            showItemDetails: '<',
            showContextMenu: '<',
            displayNewControl: '<',
            allowSelection: '<',
            cardSelected: '<',
            selected: '<'
        }
    });
}