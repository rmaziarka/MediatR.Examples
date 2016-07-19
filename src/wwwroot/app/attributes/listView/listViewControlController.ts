/// <reference path="../../typings/_all.d.ts" />

module Antares.Attribues {
    export class ListViewControlController {
        // bindings 
        public ngModel: any[];
        public config: any;
        public schema: Attributes.IListViewControlSchema;
        
        private defaultItemTemplateUrl: string = 'app/attributes/listView/templates/listItemSimpleTemplate.html';

        constructor() {
            this.ngModel = this.ngModel || [];
            this.schema.itemTemplateUrl = this.schema.itemTemplateUrl || this.defaultItemTemplateUrl;
        }
    }

    angular.module('app').controller('ListViewControlController', ListViewControlController);
}