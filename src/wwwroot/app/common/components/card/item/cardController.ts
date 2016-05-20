/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class CardController {
        public cardTemplateId: string;
        public item: any;
        public showItemDetails: (item: any) => void;
        public displayNewControl: boolean = true;
    }

    angular.module('app').controller('CardController', CardController);
}