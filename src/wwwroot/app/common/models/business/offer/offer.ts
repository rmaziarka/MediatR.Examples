/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Offer implements Dto.IOffer {
        id: string = null;
        statusId: string = null;
        activityId: string = null;
        requirementId: string = null;
    }
}