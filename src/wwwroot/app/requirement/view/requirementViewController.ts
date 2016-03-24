/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {

    export class RequirementViewController {
        requirement: any;
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}