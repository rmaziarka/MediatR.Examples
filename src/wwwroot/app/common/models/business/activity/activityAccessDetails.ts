/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityAccessDetails implements Dto.IActivityAccessDetails {
        keyNumber: string = '';
        accessArrangements: string = '';

        constructor(accessDetails?: Dto.IActivityAccessDetails) {
            if (accessDetails) {
                this.keyNumber = accessDetails.keyNumber;
                this.accessArrangements = accessDetails.accessArrangements;
            }
        }
    }
}