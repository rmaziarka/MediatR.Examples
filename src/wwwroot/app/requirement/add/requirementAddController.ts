/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    import Business = Common.Models.Business;
    
    export class RequirementAddController extends Core.WithPanelsBaseController {
        requirementResource: any;
        requirement: Business.Requirement = new Business.Requirement();
        loadingContacts: boolean = false;
        entityTypeCode: string = 'Requirement';
        isSaving: boolean = false;

        constructor(
            private dataAccessService: Services.DataAccessService,
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {
            
            super(componentRegistry, $scope);
            this.requirementResource = dataAccessService.getRequirementResource();
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
                contactSidePanelId: 'addRequirement:contactSidePanelComponent'
            }
        }

        defineComponents() {
            this.components = {
                contactList: () => { return this.componentRegistry.get(this.componentIds.contactListId); },
                panels: {
                    contactSidePanel: () => { return this.componentRegistry.get(this.componentIds.contactSidePanelId); }
                }
            };
        }
    }
    angular.module('app').controller('requirementAddController', RequirementAddController);
}