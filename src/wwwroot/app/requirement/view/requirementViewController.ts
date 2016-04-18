/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;

    export class RequirementViewController extends Core.WithPanelsBaseController {
        requirement: Dto.IRequirement;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope) {

            super(componentRegistry, $scope);
        }

        showNotesPanel = () => {
            this.showPanel(this.components.panels.notes);
        }

        defineComponentIds(): void {
            this.componentIds = {
                notesSidePanelId: 'requirementView:notesSidePanelComponent',
                noteListId: 'requirementView:requirementNoteListComponent'
            }
        }

        defineComponents(): void {
            this.components = {
                noteList: () => { return this.componentRegistry.get(this.componentIds.noteListId); },
                panels: {
                    notes: () => { return this.componentRegistry.get(this.componentIds.notesSidePanelId); }
                }
            }
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}