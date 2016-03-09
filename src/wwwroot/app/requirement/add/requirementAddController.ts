/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    export class RequirementAddController {
        public requirement: Antares.Common.Models.Dto.IRequirement;

        private requirementResource: Antares.Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IRequirementResource>;

        constructor(private dataAccessService: Antares.Services.DataAccessService) {
            this.requirementResource = dataAccessService.getRequirementResource();
        }

        public save() {
            this.requirementResource.save(this.requirement);
        }
    }

    angular.module('app').controller('requirementAddController', RequirementAddController);
}