/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class ContextMenuItemController {
        public type: string;
        public item: any;
        public action: (item: any) => void;
        public external: boolean;
    }

    angular.module('app').controller('ContextMenuItemController', ContextMenuItemController);
}