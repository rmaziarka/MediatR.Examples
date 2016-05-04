/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;

    export class RequirementViewController extends Core.WithPanelsBaseController {
        requirement: Dto.IRequirement;
        viewingDetailsPanelVisible: boolean = false;
        loadingActivities: boolean = false;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope) {

            super(componentRegistry, $scope);
        }

        showNotesPanel = () => {
            this.components.noteAdd().clearNote();
            this.showPanel(this.components.panels.notes);
        }
        
        showActivitiesPanel = () => {
            this.loadingActivities = true;
            this.components.activitiesList()
                .loadActivities()
                .finally(() => { this.loadingActivities = false; });
            
            this.components.viewingDetails().clearViewingDetails();
            this.showPanel(this.components.panels.configureViewingsSidePanel);
            this.viewingDetailsPanelVisible = false;
        }

        showViewingDetailsPanel = () =>{
            var selectedActivity : Dto.IActivityQueryResult = this.components.activitiesList().getSelectedActivity();
            this.viewingDetailsPanelVisible = true;
            this.components.viewingDetails()
                .setActivity(selectedActivity);
            this.showPanel(this.components.panels.configureViewingsSidePanel);
        }

        cancelConfigureViewings() {
            this.components.panels.configureViewingsSidePanel().hide();
        }

        cancelViewingDetails(){
            this.viewingDetailsPanelVisible = false;
        }

        defineComponentIds(): void {
            this.componentIds = {
                noteAddId: 'requirementView:requirementNoteAddComponent',
                noteListId: 'requirementView:requirementNoteListComponent',
                notesSidePanelId: 'requirementView:notesSidePanelComponent',
                activitiesListId: 'addRequirement:activitiesListComponent',
                viewingDetailsId: 'addRequirement:viewingDetailsComponent',
                configureViewingsPanelId: 'addRequirement:configureViewingsSidePanelComponent'
            }
        }

        defineComponents(): void {
            this.components = {
                noteAdd: () => { return this.componentRegistry.get(this.componentIds.noteAddId); },
                noteList: () => { return this.componentRegistry.get(this.componentIds.noteListId); },
                activitiesList: () => { return this.componentRegistry.get(this.componentIds.activitiesListId); },
                viewingDetails: () => { return this.componentRegistry.get(this.componentIds.viewingDetailsId); },
                panels: {
                    notes: () => { return this.componentRegistry.get(this.componentIds.notesSidePanelId); },
                    configureViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.configureViewingsSidePanelId); }
                }
            }
        }

        showViewingView = (viewing: any) => {
        }

        saveViewing() {
            this.components.viewingDetails()
            .saveViewing(this.requirement.id)
            .then(() => {
                this.cancelViewingDetails();
            });
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}