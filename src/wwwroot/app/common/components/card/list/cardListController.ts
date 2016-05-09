/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class CardListController {
        public showItemAdd: () => void;
        public showItemAddDisabled: boolean = false;
    }

    angular.module('app').controller('CardListController', CardListController);
}