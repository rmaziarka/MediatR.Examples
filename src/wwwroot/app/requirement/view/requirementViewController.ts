/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;

    export class RequirementViewController {
        requirement: Dto.IRequirement;
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}