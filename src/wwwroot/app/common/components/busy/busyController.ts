/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Component {
            export class BusyController {
                isBusy: boolean;
                label: string;
            }

            angular.module('app').controller('busyController', BusyController);
        }
    }
}