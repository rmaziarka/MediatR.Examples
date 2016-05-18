/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class SearchController {
        // component attributes (API)
        public loadItems: ($viewValue: string) => void;
        public onSelectItem: <T>($item: T) => void;
        public onCancel: () => void;
        public itemTemplateUrl: string;
        public placeholder: string;

        // component data
        public options: SearchOptions = new SearchOptions();
        public selectedItem: any;



        public onSelect = <T>($item: T) => {
            this.onSelectItem($item);

            this.selectedItem = null;
        }

        public cancel = () => {
            this.onCancel();

            this.selectedItem = null;
        }
    }

    angular.module('app').controller('SearchController', SearchController);
};