/// <reference path="../../typings/_all.d.ts" />

module Antares.RequirementNote {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class RequirementNoteAddController {
        componentId: string;
        requirement: Business.Requirement;
        description: string;
        requirementNote: Business.RequirementNote;
        requirementNoteResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IRequirementNoteResource>;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope) {

            componentRegistry.register(this, this.componentId);
            this.requirementNoteResource = dataAccessService.getRequirementNoteResource();

            this.requirementNote = new Business.RequirementNote(<Dto.IRequirementNote>{ requirementId: this.requirement.id });
        }

        clearNote = () => {
            this.requirementNote.description = '';
            var form = this.$scope["requirementNoteAddForm"];
            form.$setPristine();
        }

        saveNote = () => {
            this.requirementNoteResource
                .save({ id: this.requirement.id }, new Business.CreateRequirementNoteResource(this.requirementNote))
                .$promise.then((result: Dto.IRequirementNote) => {
                    this.requirement.requirementNotes.splice(0, 0, new Business.RequirementNote(<Dto.IRequirementNote>result));
                    this.clearNote();
                });
        }
    }

    angular.module('app').controller('RequirementNoteAddController', RequirementNoteAddController);
};