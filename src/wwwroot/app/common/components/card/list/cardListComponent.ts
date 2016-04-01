/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('cardList', {
        templateUrl: 'app/common/components/card/list/cardList.html',
        controllerAs: 'clvm',
        controller: 'CardListController',
        transclude: {
            'header': '?cardListHeader',
            'noItems': '?cardListNoItems'
        },
        bindings: {
            cardTemplateUrl: '<',
            itemsOrder: '<',
            items: '<',
            showItemDetails: '<',
            showItemAdd: '<'
        }
    });
}