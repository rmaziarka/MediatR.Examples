/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class ExternalLinkController {

        public url: string;                // set via binding
        public showText: boolean;          // set via binding

        formatUrlWithProtocol = (): string => {
            //regular expression for url with a protocol (case insensitive)
            var r = new RegExp('^(?:[a-z]+:)?//', 'i');
            if (r.test(this.url)) {return this.url;}
            else {return 'http://' + this.url;}
        }
    }

    angular.module('app').controller('ExternalLinkController', ExternalLinkController);
}