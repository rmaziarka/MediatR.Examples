/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Commands.Activity {
    
    export class ActivityAddCommand extends ActivityBaseCommand implements IActivityAddCommand {
        id: string = null;
        propertyId: string;

        constructor(activity: Business.ActivityEditModel) {
            super(activity);
            this.propertyId = activity.propertyId;
        }
    }
    
    export interface IActivityAddCommand extends IActivityBaseCommand {
        propertyId: string;
    }
}