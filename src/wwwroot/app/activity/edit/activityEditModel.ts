/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity  {
    import Dto = Antares.Common.Models.Dto;
    
    export class ActivityEditModel extends Antares.Activity.ActivityBaseModel {
        constructor(activity?: Dto.IActivity) {
            super(activity);
        }
    }
}