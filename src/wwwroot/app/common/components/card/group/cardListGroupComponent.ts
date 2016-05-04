/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('cardListGroup', {
        templateUrl: 'app/common/components/card/group/cardListGroup.html',       
        transclude: {
            'header': '?cardListGroupHeader',
            'item': '?cardListGroupItem'
        }
    });
}