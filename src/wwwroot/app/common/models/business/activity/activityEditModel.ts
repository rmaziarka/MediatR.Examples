/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business  {
    import Dto = Antares.Common.Models.Dto;
    
    export class ActivityEditModel extends ActivityBaseModel {
        constructor(activity?: Dto.IActivity) {
            super(activity);
        }
    }
}