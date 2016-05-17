/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class CardController {
        public cardTemplateId: string;
        public item: any;
        public showItemDetails: (item: any) => void;
        public showContextMenu: boolean = false;
        public displayNewControl: boolean = true;
        public contextMenuOpened: boolean = false;

        constructor(
            private $timeout: ng.ITimeoutService) {
        }

        closeContextMenu = () => {
            //TODO try to find better solution to close context menu on blur (why document.activeElement is not set properly???)
            // - without timeout clicking on context menu items doesnt work (click action is not called - blur wins :) )
            this.$timeout(() => {
                if (!$(document.activeElement).hasClass('contextMenuItem')) {
                    this.contextMenuOpened = false;
                }
            }, 10);
        }
    }

    angular.module('app').controller('CardController', CardController);
}