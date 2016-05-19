/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {

    export class ExternalLinkController {

        public url: string;                // set via binding
        public showText: boolean;          // set via binding
    }

    angular.module('app').controller('ExternalLinkController', ExternalLinkController);
}