/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('sidePanel', {
        controller: 'sidePanelController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/sidePanel/sidepanel.html',
        transclude: {
            'content': '?sidePanelContent',
            'header': '?sidePanelHeader',
            'footer': '?sidePanelFooter'
        },
        bindings: {
            componentId: '<'
        }
    });
}