/// <reference path="../../../typings/_all.d.ts" />

module Antares.Activity.Commands {
    import Business = Common.Models.Business;

    export class ActivityEditCommand extends ActivityBaseCommand implements IActivityEditCommand {
        id: string;

        constructor(activity: Business.Activity) {
            super(activity);
            this.id = activity.id;
        }
    }
    
    export interface IActivityEditCommand extends IActivityBaseCommand {
        id: string;
    }
}