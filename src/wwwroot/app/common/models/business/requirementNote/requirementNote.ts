/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class RequirementNote implements Dto.IRequirementNote {
        id: string = '';
        requirementId: string = '';
        description: string = '';
        createdDate: Date = null;
        user: User = null;
        
        constructor(requirementNote?: Dto.IRequirementNote) {
            if (requirementNote) {
                angular.extend(this, requirementNote);

                this.createdDate = Core.DateTimeUtils.convertDateToUtc(requirementNote.createdDate);
                this.user = new User(requirementNote.user);
            }
        }
    }
}