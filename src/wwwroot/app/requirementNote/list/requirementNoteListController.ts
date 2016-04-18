/// <reference path="../../typings/_all.d.ts" />

module Antares.RequirementNote {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class RequirementNoteListController {

        componentId: string;
        requirement: Business.Requirement;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry) {
            componentRegistry.register(this, this.componentId);
        }
    }

    angular.module('app').controller('RequirementNoteListController', RequirementNoteListController);
};