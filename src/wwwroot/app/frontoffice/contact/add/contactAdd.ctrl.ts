/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export class ContactAddController {
        public text: string;

        constructor() {
            this.text = 'Hello from contact add controller!';
        }
    }

    angular.module('app.frontoffice').controller('ContactAddController', ContactAddController);
}