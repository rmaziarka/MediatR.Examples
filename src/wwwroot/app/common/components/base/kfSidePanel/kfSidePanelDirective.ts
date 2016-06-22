/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export function kfSidePanel(): ng.IDirective {
        return {
            scope: false,
            templateUrl: 'app/common/components/base/kfSidePanel/kfSidePanel.html',
            transclude: {
                'cards': '?sidePanelCards'
            },
            controller: 'kfSidePanelController',
        };
    }

    angular.module('app').directive('kfSidePanel', kfSidePanel);
}