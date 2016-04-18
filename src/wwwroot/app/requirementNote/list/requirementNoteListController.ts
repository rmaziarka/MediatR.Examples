/// <reference path="../../typings/_all.d.ts" />

module Antares.RequirementNote {
    import Business = Common.Models.Business;
    import ListOrder = Common.Component.ListOrder;

    export class RequirementNoteListController {

        componentId: string;
        requirement: Business.Requirement;

        noteListOrder: ListOrder = new ListOrder('createdDate', true);

        constructor(
            componentRegistry: Core.Service.ComponentRegistry) {
            componentRegistry.register(this, this.componentId);
        }
    }

    angular.module('app').controller('RequirementNoteListController', RequirementNoteListController);
};