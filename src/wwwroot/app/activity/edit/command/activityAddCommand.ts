/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    
    export class ActivityAddCommand extends ActivityBaseCommand implements IActivityAddCommand {
        propertyId: string;

        constructor(activity: Business.Activity, propertyId: string) {
            super(activity);
            this.propertyId = propertyId;
        }
    }
    
    export interface IActivityAddCommand extends IActivityBaseCommand {
        propertyId: string;
    }
}