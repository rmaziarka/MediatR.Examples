/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import Attachment = Common.Component.Attachment;

    export class ActivityViewController {
        // bindings
        activity: Business.Activity;

        //fields
        isAttachmentsUploadPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isAttachmentsPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isPropertyPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isViewingPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isOfferPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;        

        attachmentManagerData: Attachment.IAttachmentsManagerData;

        activityAttachmentResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityAttachmentSaveCommand>;

        selectedOffer: Dto.IOffer;
        selectedViewing: Dto.IViewing;        

        //controls
        controlSchemas: any = {
            marketAppraisalPrice: {
                controlId: "market-appraisal-price",
                translationKey: "ACTIVITY.VIEW.PRICES.MARKET_APPRAISAL_PRICE"
            },
            recommendedPrice: {
                controlId: "recommended-price",
                translationKey: "ACTIVITY.VIEW.PRICES.RECOMMENDED_PRICE"
            },
            vendorEstimatedPrice: {
                controlId: "vendor-estimated-price",
                translationKey: "ACTIVITY.VIEW.PRICES.VENDOR_ESTIMATED_PRICE"
            },
            askingPrice: {
                controlId: "asking-price",
                translationKey: "ACTIVITY.VIEW.PRICES.ASKING_PRICE"
            },
            shortLetPricePerWeek: {
                controlId: "short-let-price-per-week",
                translationKey: "ACTIVITY.VIEW.PRICES.SHORT_LET_PRICE_PER_WEEK"
            }
        };

        constructor(            
            private $state: ng.ui.IStateService,
            private dataAccessService: Services.DataAccessService,
            private latestViewsProvider: LatestViewsProvider,
            private eventAggregator: Core.EventAggregator) {           

            this.activityAttachmentResource = dataAccessService.getAttachmentResource();

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () => {
                this.hidePanels();
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenPropertyPrewiewPanelEvent, (event: Antares.Attributes.OpenPropertyPrewiewPanelEvent) => {
                this.hidePanels();
                this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Opened;
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenViewingPreviewPanelEvent, (event: Antares.Attributes.OpenViewingPreviewPanelEvent) => {
                this.hidePanels();
                this.isViewingPreviewPanelVisible = Enums.SidePanelState.Opened;
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenOfferPreviewPanelEvent, (event: Antares.Attributes.OpenOfferPreviewPanelEvent) => {
                this.hidePanels();
                this.isOfferPreviewPanelVisible = Enums.SidePanelState.Opened;
            });
            
            this.eventAggregator.with(this).subscribe(Attachment.OpenAttachmentPreviewPanelEvent, this.openAttachmentPreviewPanel);
            this.eventAggregator.with(this).subscribe(Attachment.OpenAttachmentUploadPanelEvent, this.openAttachmentUploadPanel);

            eventAggregator
                .with(this)
                .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                    this.addSavedAttachmentToList(event.attachmentSaved);
                    this.recreateAttachmentsData();
                });

            this.recreateAttachmentsData();
        }

        hidePanels = () => {
            this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Closed;
            this.isAttachmentsUploadPanelVisible = Enums.SidePanelState.Closed;
            this.isAttachmentsPreviewPanelVisible = Enums.SidePanelState.Closed;
            this.isViewingPreviewPanelVisible = Enums.SidePanelState.Closed;
            this.isOfferPreviewPanelVisible = Enums.SidePanelState.Closed;

            this.recreateAttachmentsData();
        }

        openAttachmentPreviewPanel = () => {
            this.hidePanels();

            this.isAttachmentsPreviewPanelVisible = Enums.SidePanelState.Opened;
            this.recreateAttachmentsData();
        }

        openAttachmentUploadPanel = () => {
            this.hidePanels();

            this.isAttachmentsUploadPanelVisible = Enums.SidePanelState.Opened;
            this.recreateAttachmentsData();
        }

        openOfferPreviewPanel = () => {
            this.hidePanels();

            this.isOfferPreviewPanelVisible = Enums.SidePanelState.Opened;
            this.recreateAttachmentsData();
        }

        recreateAttachmentsData = () => {
            this.attachmentManagerData = {
                entityId: this.activity.id,
                enumDocumentType: Dto.EnumTypeCode.ActivityDocumentType,
                entityType: Enums.EntityTypeEnum.Activity,
                attachments: this.activity.attachments,
                isPreviewPanelVisible: this.isAttachmentsPreviewPanelVisible,
                isUploadPanelVisible: this.isAttachmentsUploadPanelVisible
            }
        }

        

        addSavedAttachmentToList = (result: Dto.IAttachment) => {
            var savedAttachment = new Business.Attachment(result);
            this.activity.attachments.push(savedAttachment);
        }

        saveAttachment = (attachment: Antares.Common.Component.Attachment.AttachmentUploadCardModel) => {
            return this.activityAttachmentResource.save({ id: this.activity.id }, new Antares.Activity.Command.ActivityAttachmentSaveCommand(this.activity.id, attachment))
                .$promise;
        }

        cancelViewingPreview() {
            this.hidePanels();
        }

        goToEdit = () => {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        };
    }

    angular.module('app').controller('activityViewController', ActivityViewController);
}
