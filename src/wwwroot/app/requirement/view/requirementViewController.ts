/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;

    export class RequirementViewController extends Core.WithPanelsBaseController {
        requirement: Dto.IRequirement;
        viewingAddPanelVisible: boolean = false;
        previewPanelVisible: boolean = true;
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
            
            this.components.viewingAdd().clearViewingAdd();
            this.showPanel(this.components.panels.configureViewingsSidePanel);
            this.viewingAddPanelVisible = false;
        }

        showViewingAddPanel = () =>{
            var selectedActivity : Dto.IActivityQueryResult = this.components
                .activitiesList()
                .getSelectedActivity();
                
            if(selectedActivity === null || selectedActivity === undefined){
                return;
            }
            
            this.components.viewingAdd().clearViewingAdd();
            this.components.viewingAdd().setActivity(selectedActivity);

            this.viewingAddPanelVisible = true;

            this.showPanel(this.components.panels.configureViewingsSidePanel);
        }

        showViewingPreviewPanel = () => {
            this.showPanel(this.components.panels.previewViewingsSidePanel);
        }

        cancelConfigureViewings() {
            this.components.panels.configureViewingsSidePanel().hide();
        }

        cancelViewingAdd() {
            this.viewingAddPanelVisible = false;
        }

        goBackToPreviewViewing(){
            this.previewPanelVisible = true;
        }

        cancelViewingPreview() {
            this.hidePanels();
        }

        defineComponentIds(): void {
            this.componentIds = {
                noteAddId: 'requirementView:requirementNoteAddComponent',
                noteListId: 'requirementView:requirementNoteListComponent',
                notesSidePanelId: 'requirementView:notesSidePanelComponent',
                activitiesListId: 'addRequirement:activitiesListComponent',
                viewingAddId: 'addRequirement:viewingAddComponent',
                viewingPreviewId: 'addRequirement:viewingPreviewComponent',
                viewingEditId: 'requirementView:viewingEditComponent',
                configureViewingsSidePanelId: 'addRequirement:configureViewingsSidePanelComponent',
                previewViewingSidePanelId: 'addRequirement:previewViewingSidePanelComponent'
            }
        }

        defineComponents(): void {
            this.components = {
                noteAdd: () => { return this.componentRegistry.get(this.componentIds.noteAddId); },
                noteList: () => { return this.componentRegistry.get(this.componentIds.noteListId); },
                activitiesList: () => { return this.componentRegistry.get(this.componentIds.activitiesListId); },
                viewingAdd: () => { return this.componentRegistry.get(this.componentIds.viewingAddId); },
                viewingEdit: () => { return this.componentRegistry.get(this.componentIds.viewingEditId); },
                viewingPreview: () => { return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                panels: {
                    notes: () => { return this.componentRegistry.get(this.componentIds.notesSidePanelId); },
                    configureViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.configureViewingsSidePanelId); },
                    previewViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); }
                }
            }
        }

        showViewingPreview = (viewing: Common.Models.Dto.IViewing) => {
            this.components.viewingPreview().clearViewingPreview();
            this.components.viewingPreview().setViewing(viewing);
            this.showPanel(this.components.panels.previewViewingsSidePanel);
            this.previewPanelVisible = true;
        }

        showViewingEdit = () =>{
            var viewing = angular.copy(this.components.viewingPreview().getViewing());
            this.components.viewingEdit().setViewing(viewing);
            this.previewPanelVisible = false;
        }

        saveViewing() {
            this.components.viewingAdd()
            .saveViewing(this.requirement.id)
            .then(() => {
                this.hidePanels();
            });
        }

        saveEditedViewing(){
            this.hidePanels();
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}