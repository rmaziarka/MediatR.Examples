/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    export class SearchController {
        // component attributes (API)
        public loadItems: ($viewValue: string) => void;
        public onSelectItem: <T>($item: T) => void;
        public onChangeValue: <T>($item: T) => void;
        public onCancel: () => void;
        public initialValue: string;
        public itemTemplateUrl: string;
        public searchPlaceholder: string;

        // component data
        public searchId: string;
        public searchName: string;
        public options: SearchOptions;
        public selectedItem: any;

        public $onInit = () =>{
            if (this.initialValue) {
                this.selectedItem = this.initialValue;
            }
        }

        public select = <T>($item: T) => {
            if (this.onSelectItem)
                this.onSelectItem($item);

            if (this.options.nullOnSelect)
                this.selectedItem = null;
        }

        public change = () => {
            if(this.onChangeValue)
                this.onChangeValue(this.selectedItem);
        }

        public cancel = () => {
            this.onCancel();

            this.selectedItem = null;
        }
    }

    angular.module('app').controller('SearchController', SearchController);
};