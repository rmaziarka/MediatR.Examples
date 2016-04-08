/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('listItem', {
        templateUrl: 'app/common/components/listItem/listItem.html',
        transclude: true
    });
}