/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ViewingGroup {
        constructor(public day: string, public viewings: Viewing[]) {            
        }
    }
}