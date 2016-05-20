/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class ExternalLinkController {

        public url: string;                // set via binding
        public showText: boolean;          // set via binding

        formatUrlWithProtocol = (): string => {
            // TODO check if http is set if not - add it
            return 'http://' + this.url;
        }
    }

    angular.module('app').controller('ExternalLinkController', ExternalLinkController);
}