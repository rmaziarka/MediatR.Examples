/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class ContextMenuItemController {
        public type: string;
        public item: any;
        public action: (item: any) => void;
        public parent: any;

        callAction = () => {
            this.action(this.item);
            this.parent.contextMenuOpened = false;
        }
    }

    angular.module('app').controller('ContextMenuItemController', ContextMenuItemController);
}