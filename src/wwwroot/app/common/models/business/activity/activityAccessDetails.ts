/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityAccessDetails {
        keyNumber: string = '';
        accessArrangements: string = '';

        constructor(keyNumber: string, accessArrangements: string) {
            this.keyNumber = keyNumber;
            this.accessArrangements = accessArrangements;
        }
    }
}