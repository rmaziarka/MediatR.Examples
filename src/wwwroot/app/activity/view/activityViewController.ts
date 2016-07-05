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
        activity: Business.ActivityViewModel;
        public selectedTabIndex: number = 0;

        //fields
        isAttachmentsUploadPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isAttachmentsPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isPropertyPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isViewingPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isOfferPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        attachmentManagerData: Attachment.IAttachmentsManagerData;

        activityAttachmentResource: Common.Models.Resources.
        IBaseResourceClass<Common.Models.Resources.IActivityAttachmentSaveCommand>;

        selectedOffer: Dto.IOffer;
        selectedViewing: Dto.IViewing;

        activitySourceSchema: Antares.Attributes.IEnumItemControlSchema = {
            controlId: 'sourceId',
            translationKey: 'ACTIVITY.COMMON.SOURCE'
        }

        activityStatusSchema: Antares.Attributes.IEnumItemControlSchema = {
            controlId: 'activityStatusId',
            translationKey: 'ACTIVITY.COMMON.STATUS'
        }

        activitySellingReasonSchema: Antares.Attributes.IEnumItemControlSchema = {
            controlId: 'sellingReasonId',
            translationKey: 'ACTIVITY.COMMON.SELLING_REASON'
        }

        activitySourceDescriptionSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'sourceDescriptionId',
            translationKey: null
        }

        activityPitchingThreatsSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'pitchingThreatsId',
            translationKey: 'ACTIVITY.COMMON.PITCHING_THREATS'
        }

        activityKeyNumberSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'keyNumberId',
            translationKey: 'ACTIVITY.EDIT.KEY_NUMBER'
        }

        activityAccessArrangementsSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'accessArrangementsId',
            translationKey: 'ACTIVITY.EDIT.ACCESS_ARRANGEMENTS'
        }

        activityDisposalTypeSchema: Antares.Attributes.IEnumItemControlSchema = {
            controlId: 'disposalTypeId',
            translationKey: 'ACTIVITY.COMMON.DISPOSAL_TYPE'            
        }

        shortKfValuationPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'shortKfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            suffix: ''
        }

        shortVendorValuationPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'shortVendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            suffix: ''
        }

        shortAgreedInitialMarketingPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'shortAgreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.SHORT_LET',
            suffix: ''
        }

        agreedInitialMarketingPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'agreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.AGREED_INITIAL_MARKETING_PRICE',
            suffix: ''
        }

        longKfValuationPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'longKfValuationPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            suffix: ''
        }

        kfValuationPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'kfValuationPrice',
            translationKey: 'ACTIVITY.VIEW.PRICES.KF_ESTIMATED_PRICE',
            suffix: ''
        }

        vendorValuationPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'vendorValuationPrice',
            translationKey: 'ACTIVITY.VIEW.PRICES.VENDOR_ESTIMATED_PRICE',
            suffix: ''
        }

        longVendorValuationPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'longVendorValuationPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            suffix: ''
        }

        longAgreedInitialMarketingPriceSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'longAgreedInitialMarketingPrice',
            translationKey: 'ACTIVITY.COMMON.LONG_LET',
            suffix: ''
        }

        serviceChargeAmountSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'serviceChargeAmount',
            translationKey: 'ACTIVITY.COMMON.SERVICE_CHARGE'
        }

        serviceChargeNoteSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'serviceChargeNote',
            translationKey: ''
        }

        groundRentAmountSchema: Antares.Attributes.IPriceControlSchema = {
            controlId: 'groundRentAmount',
            translationKey: 'ACTIVITY.COMMON.GROUND_RENT'
        }

        groundRentNoteSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'groundRentNote',
            translationKey: ''
        }

        otherConditionSchema: Antares.Attributes.ITextControlSchema = {
            controlId: 'otherCondition',
            translationKey: 'ACTIVITY.COMMON.OTHER_CONDITIONS'
        }

        activityDecorationSchema: Antares.Attributes.IEnumItemControlSchema = {
            controlId: 'decorationId',
            translationKey: 'ACTIVITY.COMMON.DECORATION',            
        }

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
                .subscribe(Common.Component.Attachment.AttachmentSavedEvent,
                (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                    this.addSavedAttachmentToList(event.attachmentSaved);
                    this.recreateAttachmentsData();
                });

            this.recreateAttachmentsData();
        }

        public setActiveTabIndex = (tabIndex: number) => {
            this.selectedTabIndex = tabIndex;
            this.setInitialState();
        }

        private setInitialState = () => {
            this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Untouched;
            this.isAttachmentsUploadPanelVisible = Enums.SidePanelState.Untouched;
            this.isAttachmentsPreviewPanelVisible = Enums.SidePanelState.Untouched;
            this.isViewingPreviewPanelVisible = Enums.SidePanelState.Untouched;
            this.isOfferPreviewPanelVisible = Enums.SidePanelState.Untouched;

            this.recreateAttachmentsData();
        }

        public isOverviewTabSelected = () => {
            return this.selectedTabIndex === 0;
        }

        public isDetailsTabSelected = () => {
            return this.selectedTabIndex === 1;
        }

        private hidePanels = () => {
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
            return this.activityAttachmentResource.save({ id: this.activity.id },
                new Antares.Activity.Command.ActivityAttachmentSaveCommand(this.activity.id, attachment))
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
