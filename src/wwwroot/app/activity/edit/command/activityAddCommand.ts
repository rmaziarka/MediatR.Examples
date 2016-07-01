/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;
    
    export class ActivityAddCommand extends ActivityBaseCommand implements IActivityAddCommand {
        id: string = null;
        propertyId: string;

        constructor(activity: Activity.ActivityEditModel) {
            super(activity);
            this.propertyId = activity.propertyId;
        }
    }
    
    export interface IActivityAddCommand extends IActivityBaseCommand {
        propertyId: string;
    }
}