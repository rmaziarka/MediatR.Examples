/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class RequirementViewController extends Core.WithPanelsBaseController {
        requirement: Business.Requirement;
        viewingAddPanelVisible: boolean = false;
        viewingPreviewPanelVisible: boolean = true;
        offerPreviewPanelVisible: boolean = true;
        loadingActivities: boolean = false;
        saveViewingBusy: boolean = false;
        addEditOfferBusy: boolean = false;
        userData: Dto.IUserData;
        selectedOffer: Dto.IOffer;
        selectedViewing: Dto.IViewing;

        enumTypeRequirementDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.RequirementDocumentType;
        entityType: EntityType = EntityType.Requirement;

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private dataAccessService: Services.DataAccessService,
            private eventAggregator: Core.EventAggregator,
            private requirementService: Requirement.RequirementService) {

            super(componentRegistry, $scope);

            eventAggregator
                .with(this)
                .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                    this.addSavedAttachmentToList(event.attachmentSaved);
                });
        }

        addSavedAttachmentToList = (result: Dto.IAttachment) => {
            var savedAttachment = new Business.Attachment(result);
            this.requirement.attachments.push(savedAttachment);
        }

        saveAttachment = (attachment: Common.Component.Attachment.AttachmentUploadCardModel) => {
            var command = new Requirement.Command.RequirementAttachmentSaveCommand(this.requirement.id, attachment);
            return this.requirementService.createRequirementAttachment(command)
                .then((result: angular.IHttpPromiseCallbackArg<Dto.IAttachment>) => { return result.data; });
        }

        showNotesPanel = () =>{
            this.components.noteAdd().clearNote();
            this.showPanel(this.components.panels.notes);
        }

        showActivitiesPanel = () =>{
            this.loadingActivities = true;
            this.components.activitiesList()
                .loadActivities()
                .finally(() =>{ this.loadingActivities = false; });

            this.components.viewingAdd().clearViewingAdd();
            this.showPanel(this.components.panels.configureViewings);
            this.viewingAddPanelVisible = false;
        }

        showViewingAddPanel = () =>{
            var selectedActivity: Dto.IActivityQueryResult = this.components
                .activitiesList()
                .getSelectedActivity();

            if (selectedActivity === null || selectedActivity === undefined) {
                return;
            }

            this.components.viewingAdd().clearViewingAdd();
            this.components.viewingAdd().setActivity(selectedActivity);

            this.viewingAddPanelVisible = true;

            this.showPanel(this.components.panels.configureViewings);
        }

        showViewingPreviewPanel = () =>{
            this.showPanel(this.components.panels.previewViewings);
        }

        cancelConfigureViewings(){
            this.components.panels.configureViewings().hide();
        }

        cancelViewingAdd(){
            this.viewingAddPanelVisible = false;
        }

        goBackToPreviewViewing(){
            this.viewingPreviewPanelVisible = true;
        }

        cancelViewingPreview(){
            this.hidePanels();
        }

        defineComponentIds(): void{
            this.componentIds = {
                noteAddId : 'requirementView:requirementNoteAddComponent',
                noteListId : 'requirementView:requirementNoteListComponent',
                notesSidePanelId : 'requirementView:notesSidePanelComponent',
                activitiesListId : 'addRequirement:activitiesListComponent',
                viewingAddId : 'addRequirement:viewingAddComponent',
                viewingEditId : 'requirementView:viewingEditComponent',
                viewingPreviewId : 'addRequirement:viewingPreviewComponent',
                configureViewingsSidePanelId : 'addRequirement:configureViewingsSidePanelComponent',
                previewViewingSidePanelId : 'addRequirement:previewViewingSidePanelComponent',
                offerAddId : 'requirementView:addOfferComponent',
                offerEditId : 'requirementView:editOfferComponent',
                offerEditPreviewId : 'requirementView:editOfferPreviewComponent',
                offerSidePanelId : 'requirementView:offerSidePanelComponent',
                offerPreviewId : 'requirementView:offerPreviewComponent',
                offerPreviewSidePanelId : 'requirementView:offerPreviewSidePanelComponent',
                offerEditSidePanelId : 'requirementView:offerEditSidePanelComponent'
            }
        }

        defineComponents(): void{
            this.components = {
                noteAdd : () =>{ return this.componentRegistry.get(this.componentIds.noteAddId); },
                noteList : () =>{ return this.componentRegistry.get(this.componentIds.noteListId); },
                activitiesList : () =>{ return this.componentRegistry.get(this.componentIds.activitiesListId); },
                viewingAdd : () =>{ return this.componentRegistry.get(this.componentIds.viewingAddId); },
                viewingEdit : () =>{ return this.componentRegistry.get(this.componentIds.viewingEditId); },
                viewingPreview : () =>{ return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                offerPreview : () =>{ return this.componentRegistry.get(this.componentIds.offerPreviewId); },
                offerAdd : () =>{ return this.componentRegistry.get(this.componentIds.offerAddId); },
                offerEdit : () =>{ return this.componentRegistry.get(this.componentIds.offerEditId); },
                offerEditPreview : () =>{ return this.componentRegistry.get(this.componentIds.offerEditPreviewId); },
                panels : {
                    notes : () =>{ return this.componentRegistry.get(this.componentIds.notesSidePanelId); },
                    configureViewings : () =>{ return this.componentRegistry.get(this.componentIds.configureViewingsSidePanelId); },
                    previewViewings : () =>{ return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); },
                    offerPreview : () =>{ return this.componentRegistry.get(this.componentIds.offerPreviewSidePanelId) },
                    offerAdd : () =>{ return this.componentRegistry.get(this.componentIds.offerSidePanelId); },
                    offerEdit : () =>{ return this.componentRegistry.get(this.componentIds.offerEditSidePanelId) }
                }
            }
        }

        showViewingPreview = (viewing: Common.Models.Business.Viewing) =>{
            this.selectedViewing = viewing;
            this.showPanel(this.components.panels.previewViewings);
            this.viewingPreviewPanelVisible = true;
        }

        showViewingEdit = () =>{
            this.components.viewingEdit().setViewing(this.selectedViewing);
            this.viewingPreviewPanelVisible = false;
        }

        showOfferPreview = (offer: Common.Models.Business.Offer) =>{
            this.selectedOffer = offer;
            this.showPanel(this.components.panels.offerPreview);
            this.offerPreviewPanelVisible = true;
        }

        saveViewing(){
            this.saveViewingBusy = true;
            this.components.viewingAdd()
                .saveViewing()
                .then((viewing: Common.Models.Dto.IViewing) =>{
                    var viewingModel = new Business.Viewing(viewing);
                    this.requirement.viewings.push(viewingModel);
                    this.requirement.groupViewings(this.requirement.viewings);
                    this.hidePanels();
                }).finally(() =>{
                    this.saveViewingBusy = false;
                });
        }

        saveEditedViewing(){
            this.saveViewingBusy = true;
            this.components.viewingEdit()
                .saveViewing()
                .then((viewing: Common.Models.Dto.IViewing) =>{
                    angular.copy<Dto.IViewing>(new Business.Viewing(viewing), this.selectedViewing);
                    this.requirement.groupViewings(this.requirement.viewings);
                    this.viewingPreviewPanelVisible = true;
                }).finally(() =>{
                    this.saveViewingBusy = false;
                });
        }

        showAddOfferPanel = (viewing: Dto.IViewing) =>{
            if (this.components.panels.offerAdd().visible) {
                return;
            }

            var offerAddComponent = this.components.offerAdd();

            offerAddComponent.activity = viewing.activity;
            offerAddComponent.reset();

            this.showPanel(this.components.panels.offerAdd);
        }

        showEditOfferPreviewPanel = () =>{
            this.components.offerEditPreview().setOffer(this.selectedOffer);
            this.offerPreviewPanelVisible = false;
        }

        showEditOfferPanel = (offer: Dto.IOffer) =>{
            this.components.offerEdit().setOffer(offer);
            this.showPanel(this.components.panels.offerEdit);
        }

        showOfferDetailsView = (offer: Dto.IOffer) =>{
            this.$state.go('app.offer-view', { id: offer.id });
        }

        cancelSaveOffer = () =>{
            this.hidePanels();
        }

        cancelOfferEditPreview(){
            this.offerPreviewPanelVisible = true;
        }

        cancelOfferEdit(){
            this.hidePanels();
        }

        saveOffer = () =>{
            this.addEditOfferBusy = true;
            this.components.offerAdd()
                .saveOffer()
                .then((offerModel: Dto.IOffer) =>{
                    var offer = new Business.Offer(offerModel);
					this.requirement.offers.push(offer);
                    this.hidePanels();
                })
                .finally(() =>{
                    this.addEditOfferBusy = false;
                });
        }

        saveEditOfferCore = (component: any) =>{
            this.addEditOfferBusy = true;
            component
                .saveOffer()
                .then((offer: Common.Models.Dto.IOffer) =>{
                    var originalOffer = component.getOriginalOffer();
                    angular.copy(new Business.Offer(offer), originalOffer);
                    if (component === this.components.offerEditPreview()) {
                        this.offerPreviewPanelVisible = true;
                    }
                    else {
                        this.hidePanels();
                    }
                }).finally(() =>{
                    this.addEditOfferBusy = false;
                });
        }

        saveEditOffer = () =>{
            this.saveEditOfferCore(this.components.offerEdit());
        }

        saveEditPreviewOffer = () =>{
            this.saveEditOfferCore(this.components.offerEditPreview());
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}