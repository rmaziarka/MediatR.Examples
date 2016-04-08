/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('list', {
        templateUrl: 'app/common/components/list/list.html',
        transclude: {
            'header': '?listHeader',
            'actions': '?listActions',
            'noItems': '?listNoItems'
        }
    });
}