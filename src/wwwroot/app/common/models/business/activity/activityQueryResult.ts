/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ActivityQueryResult {
        id: string = '';
        propertyName: string = '';
        propertyNumber: string = '';
        line2: string = '';

        constructor(activity?: Dto.IActivityQueryResult) {
            if (activity) {
                angular.extend(this, activity);
            }
        }
    }
}