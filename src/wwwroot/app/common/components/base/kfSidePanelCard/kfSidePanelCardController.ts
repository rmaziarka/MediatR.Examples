/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class KfSidePanelCardController {

        isFooterVisible: boolean = false;
        isHeaderVisible: boolean = false;

        visible: boolean = false;
        stateChanged: boolean = false;

        constructor($transclude: any, private eventAggregator: Core.EventAggregator) {
            this.isFooterVisible = $transclude.isSlotFilled('footer');
            this.isHeaderVisible = $transclude.isSlotFilled('header');
        }

        close = () => {
            this.eventAggregator.publish(new CloseSidePanelEvent());
        }
    }

    angular.module('app').controller('kfSidePanelCardController', KfSidePanelCardController);
}