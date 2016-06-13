module Antares.Common.Component {
    angular.module('app').component('kfSidePanelCard', {
        controller: 'kfSidePanelCardController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/kfSidePanelCard/kfSidePanelCard.html',
        transclude: {
            'content': '?sidePanelContent',
            'header': '?sidePanelHeader',
            'footer': '?sidePanelFooter'
        },
        bindings: {
            componentId: '<',
            isBusy: '<'
        }
    });
}