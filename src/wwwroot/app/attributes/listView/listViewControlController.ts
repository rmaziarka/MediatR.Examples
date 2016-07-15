/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    export class ListViewControlController {
        // bindings 
        public ngModel: any[];
        public config: any;
        public schema: Attributes.IListViewControlSchema;
        public itemTemplateUrl: string;

        private defaultItemTemplateUrl: string = 'app/attributes/listView/templates/listItemSimpleTemplate.html';

        constructor() {
            this.ngModel = this.ngModel || [];
            this.itemTemplateUrl = this.itemTemplateUrl || this.defaultItemTemplateUrl;
        }
    }

    angular.module('app').controller('ListViewControlController', ListViewControlController);
}