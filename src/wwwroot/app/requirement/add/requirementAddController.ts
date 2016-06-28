/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.Add {
    import Business = Common.Models.Business;
    import RequirementAddConfig = Antares.Requirement.IRequirementAddConfig;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;

    export class RequirementAddController extends Core.WithPanelsBaseController {
        requirementResource: any;
        requirement: Business.Requirement = new Business.Requirement();
        loadingContacts: boolean = false;
        entityTypeCode: string = 'Requirement';
        isSaving: boolean = false;
        configService: Services.ConfigService;
        public config: RequirementAddConfig;

        constructor(
            private dataAccessService: Services.DataAccessService,
            componentRegistry: Core.Service.ComponentRegistry,
            configService: Services.ConfigService,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService) {

            super(componentRegistry, $scope);
            this.requirementResource = dataAccessService.getRequirementResource();
            this.configService = configService;
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

        requirementTypeChanged = (requirementTypeId: string) => {
            this.requirement.requirementTypeId = requirementTypeId;
            this.configService.getRequirement(PageTypeEnum.Create,
                this.requirement.requirementTypeId,
                this.requirement)
                .then((config: any) => {
                    this.config = config;
                });
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