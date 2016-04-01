/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class CardListController {
        public cardTemplateId: string;
        public itemsOrder: CartListOrder;
        public items: any[];
        public showItemDetails: (item: any) => void;
        public showItemAdd: () => void;
    }

    angular.module('app').controller('CardListController', CardListController);
}