/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    import Business = Common.Models.Business;
    
    export class RequirementAddController extends Core.WithPanelsBaseController {
        requirementResource: any;
        requirement: Business.Requirement = new Business.Requirement();
        loadingContacts: boolean = false;
        loadingActivities: boolean = false;
        entityTypeCode: string = 'Requirement';
        isSaving: boolean = false;
        viewingDetailsPanelVisible: boolean = false;

        constructor(
            private dataAccessService: Services.DataAccessService,
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {
            
            super(componentRegistry, $scope);
            this.requirementResource = dataAccessService.getRequirementResource();

            $scope.$on('$destroy', () => {
                this.componentRegistry.deregister(this.componentIds.contactListId);
                this.componentRegistry.deregister(this.componentIds.contactSidePanelId);
                this.componentRegistry.deregister(this.componentIds.activitiesListId);
                this.componentRegistry.deregister(this.componentIds.viewingDetailsId);
                this.componentRegistry.deregister(this.componentIds.configureViewingsPanelId);
            });
        }

        updateContacts() {
            this.requirement.contacts = this.components.contactList().getSelected();
            this.components.panels.contactSidePanel().hide();
        }

        cancelUpdateContacts() {
            this.components.panels.contactSidePanel().hide();
        }

        showContactList = () => {
            this.loadingContacts = true;
            this.components.contactList()
                .loadContacts()
                .then(() => {
                    var selectedIds: string[] = [];
                    if (this.requirement.contacts !== undefined && this.requirement.contacts !== null) {
                        selectedIds = <string[]>this.requirement.contacts.map((c: any) => c.id);
                    }
                    this.components.contactList().setSelected(selectedIds);
                })
                .finally(() => { this.loadingContacts = false; });

            this.showPanel(this.components.panels.contactSidePanel);
        }

        showActivitiesPanel = () => {
            this.loadingActivities = true;
            this.components.activitiesList()
                .loadActivities()
                .finally(() => { this.loadingActivities = false; });

            this.showPanel(this.components.panels.configureViewingsSidePanel);
        }

        cancelConfigureViewings() {
            this.components.panels.configureViewingsSidePanel().hide();
        }

        save() {
            this.isSaving = true;
            this.requirementResource
                .save(new Business.CreateRequirementResource(this.requirement))
                .$promise
                .then(
                (requirement: any) => {
                    this.$state.go('app.requirement-view', requirement);
                })
                .finally(() => {
                    this.isSaving = false;
                });
        }

        defineComponentIds() {
            this.componentIds = {
                contactListId: 'addRequirement:contactListComponent',
                contactSidePanelId: 'addRequirement:contactSidePanelComponent',
                activitiesListId: 'addRequirement:activitiesListComponent',
                viewingDetailsId: 'addRequirement:viewingDetailsComponent',
                configureViewingsPanelId: 'addRequirement:configureViewingsSidePanelComponent',
            }
        }

        defineComponents() {
            this.components = {
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
                activitiesList: () => { return this.componentRegistry.get(this.componentIds.activitiesListId); },
                viewingDetails: () => { return this.componentRegistry.get(this.componentIds.viewingDetailsId); },
                panels: {
                    contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); },
                    configureViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.configureViewingsSidePanelId); }
                }
            };
        }
    }
    angular.module('app').controller('requirementAddController', RequirementAddController);
}