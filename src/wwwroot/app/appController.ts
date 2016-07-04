/// <reference path="typings/_all.d.ts" />

module Antares {
    export class AppController {

        constructor(public userData: Antares.Common.Models.Dto.ICurrentUser) {
        }

    }
    angular.module('app').controller('appController', AppController);
}