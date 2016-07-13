/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component
{
    angular.module('app').component('search', {
        templateUrl: 'app/common/components/search/search.html',
        controllerAs : 'vm',
        controller: 'SearchController',
        bindings: {
            searchId: '@',
            searchName: '@',
            options: '<',
            onSelectItem: '<',
            onChangeValue: '<',
            onCancel: '<',
            loadItems: '<',
            itemTemplateUrl: '@',
            searchPlaceholder: '@',
            initialValue: '<'
        }
    });
}