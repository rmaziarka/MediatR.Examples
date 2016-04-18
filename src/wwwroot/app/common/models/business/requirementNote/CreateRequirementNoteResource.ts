/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateRequirementNoteResource implements Dto.ICreateRequirementNoteResource {
        requirementId: string = '';
        description: string = '';

        constructor(requirementNote?: Dto.IRequirementNote) {
            if (requirementNote) {
                this.requirementId = requirementNote.requirementId;
                this.description = requirementNote.description;
            }
        }
    }
}