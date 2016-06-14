/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class CardController {
        public cardTemplateId: string;
        public item: any;
        public showItemDetails: (item: any) => void;
        public cardSelected: (item: any, selected: boolean) => void;
        public displayNewControl: boolean = true;
        public allowSelection: boolean = false;
        public selected: boolean = false;

        clicked = () => {
            if (this.allowSelection) {
                this.selected = !this.selected;
                if (this.cardSelected != undefined) this.cardSelected(this.item, this.selected);
            }
        }
    }

    angular.module('app').controller('CardController', CardController);
}