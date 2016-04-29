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
            this.components.noteAdd().clearNote();
            this.showPanel(this.components.panels.notes);
        }

        defineComponentIds(): void {
            this.componentIds = {
                noteAddId: 'requirementView:requirementNoteAddComponent',
                noteListId: 'requirementView:requirementNoteListComponent',
                notesSidePanelId: 'requirementView:notesSidePanelComponent'
            }
        }

        defineComponents(): void {
            this.components = {
                noteAdd: () => { return this.componentRegistry.get(this.componentIds.noteAddId); },
                noteList: () => { return this.componentRegistry.get(this.componentIds.noteListId); },
                panels: {
                    notes: () => { return this.componentRegistry.get(this.componentIds.notesSidePanelId); }
                }
            }
        }

        showViewingView = (viewing: any) => {
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}