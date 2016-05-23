/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component
{
    angular.module('app').component('search', {
        templateUrl: 'app/common/components/search/search.html',
        controllerAs : 'vm',
        controller: 'SearchController',
        bindings: {
            options: '<',
            onSelectItem: '<',
            onCancel: '<',
            loadItems: '<',
            itemTemplateUrl: '@',
            searchPlaceholder: '@'
        }
    });
}