/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;

    export class RequirementViewController extends Core.WithPanelsBaseController {
        requirement: Dto.IRequirement;
        viewingAddPanelVisible: boolean = false;
        previewPanelVisible: boolean = true;
        loadingActivities: boolean = false;
        saveViewingBusy: boolean = false;

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

        showViewingAddPanel = () => {
            var selectedActivity: Dto.IActivityQueryResult = this.components
                .activitiesList()
                .getSelectedActivity();

            if (selectedActivity === null || selectedActivity === undefined) {
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

        goBackToPreviewViewing() {
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
                viewingEditId: 'requirementView:viewingEditComponent',
                viewingPreviewId: 'addRequirement:viewingPreviewComponent',
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

        showViewingPreview = (viewing: Common.Models.Business.Viewing) => {
            this.components.viewingPreview().clearViewingPreview();
            this.components.viewingPreview().setViewing(viewing);
            this.showPanel(this.components.panels.previewViewingsSidePanel);
            this.previewPanelVisible = true;
        }

        showViewingEdit = () => {
            var viewing = this.components.viewingPreview().getViewing();
            this.components.viewingEdit().setViewing(viewing);
            this.previewPanelVisible = false;
        }

        saveViewing() {
            this.saveViewingBusy = true;
            this.components.viewingAdd()
                .saveViewing()
                .then(() => {
                    this.hidePanels();
                }).finally(() => {
                    this.saveViewingBusy = false;
                });
        }

        saveEditedViewing() {
            this.saveViewingBusy = true;
            this.components.viewingEdit()
                .saveViewing()
                .then(() => {
                    this.hidePanels();
                }).finally(() => {
                    this.saveViewingBusy = false;
                });
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}