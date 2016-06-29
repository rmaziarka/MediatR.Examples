/// <reference path="../../typings/_all.d.ts" />

module Antares.Requirement.View {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import RequirementViewConfig = Antares.Requirement.IRequirementViewConfig;
    import Enums = Common.Models.Enums;
    import OfferPanelMode = Antares.Offer.Component.OfferPanelMode;

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
        public config: RequirementViewConfig;

        enumTypeRequirementDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.RequirementDocumentType;
        entityType: EntityType = EntityType.Requirement;

        isOfferAddPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isOfferEditPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        offerPanelMode: OfferPanelMode = OfferPanelMode.Preview;

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

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () => {
                this.isOfferAddPanelVisible = Enums.SidePanelState.Closed;
                this.isOfferEditPreviewPanelVisible = Enums.SidePanelState.Closed;
            });

            this.eventAggregator.with(this).subscribe(Offer.OfferAddedSidePanelEvent, (msg: Offer.OfferAddedSidePanelEvent) =>{
                this.onSaveNewOffer(msg.addedOffer);
            });

            this.eventAggregator.with(this).subscribe(Offer.OfferUpdatedSidePanelEvent, (msg: Offer.OfferUpdatedSidePanelEvent) => {
                this.onUpdateOffer(msg.updatedOffer);
            });
        }

        onPanelsHidden = () =>{
            this.isOfferAddPanelVisible = Enums.SidePanelState.Closed;
            this.isOfferEditPreviewPanelVisible = Enums.SidePanelState.Closed;
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
                panels : {
                    notes : () =>{ return this.componentRegistry.get(this.componentIds.notesSidePanelId); },
                    configureViewings : () =>{ return this.componentRegistry.get(this.componentIds.configureViewingsSidePanelId); },
                    previewViewings : () =>{ return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); },
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

        showOfferDetailsView = (offer: Dto.IOffer) =>{
            this.$state.go('app.offer-view', { id: offer.id });
        }

        showAddOfferPanel = (viewing: Dto.IViewing) => {
            this.hidePanels();
            this.selectedViewing = viewing;
            this.isOfferAddPanelVisible = Enums.SidePanelState.Opened;
        }

        showOfferPreviewPanel = (offer: Dto.IOffer) =>{
            if (!this.isOfferEditPreviewPanelVisible) {
                this.hidePanels();
            }
            this.selectedOffer = offer;
            this.isOfferEditPreviewPanelVisible = Enums.SidePanelState.Opened;
            this.offerPanelMode = OfferPanelMode.Preview;
        }

        showOfferEditPanel = (offer: Dto.IOffer) =>{
            if (!this.isOfferEditPreviewPanelVisible) {
                this.hidePanels();
            }
            this.selectedOffer = offer;
            this.isOfferEditPreviewPanelVisible = Enums.SidePanelState.Opened;
            this.offerPanelMode = OfferPanelMode.Edit;
        }

        onSaveNewOffer = (offerModel: Dto.IOffer) =>{
            var offer = new Business.Offer(offerModel);
            this.requirement.offers.push(offer);
        }

        onUpdateOffer = (offerModel: Dto.IOffer) =>{
            var offer = new Business.Offer(offerModel);
            var existingOfferIndex = _.findIndex(this.requirement.offers, { id : offer.id });
            if (existingOfferIndex > -1) {
                this.requirement.offers[existingOfferIndex] = offer;
            }
        }
    }

    angular.module('app').controller('requirementViewController', RequirementViewController);
}