/// <reference path="../../typings/_all.d.ts" />

module Antares.Activity.View {
    import Business = Common.Models.Business;
    import CartListOrder = Common.Component.ListOrder;
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ActivityViewController extends Core.WithPanelsBaseController {
        // bindings
        activity: Business.Activity;

        enumTypeActivityDocumentType: Dto.EnumTypeCode = Dto.EnumTypeCode.ActivityDocumentType;
        entityType: EntityType = EntityType.Activity;

        activityAttachmentResource: Common.Models.Resources.IBaseResourceClass<Common.Models.Resources.IActivityAttachmentSaveCommand>;

        selectedOffer: Dto.IOffer;
        selectedViewing: Dto.IViewing;

        isPropertyPreviewPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;        

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
            componentRegistry: Core.Service.ComponentRegistry,
            private $scope: ng.IScope,
            private $state: ng.ui.IStateService,
            private dataAccessService: Services.DataAccessService,
            private latestViewsProvider: LatestViewsProvider,
            private eventAggregator: Core.EventAggregator) {

            super(componentRegistry, $scope);

            this.activityAttachmentResource = dataAccessService.getAttachmentResource();

            this.eventAggregator.with(this).subscribe(Common.Component.CloseSidePanelEvent, () => {
                this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Closed;
            });

            this.eventAggregator.with(this).subscribe(Attributes.OpenPropertyPrewiewPanelEvent, (event: Antares.Attributes.OpenPropertyPrewiewPanelEvent) => {
                this.hidePanels();
                this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Opened;
            });

            eventAggregator
                .with(this)
                .subscribe(Common.Component.Attachment.AttachmentSavedEvent, (event: Common.Component.Attachment.AttachmentSavedEvent) => {
                    this.addSavedAttachmentToList(event.attachmentSaved);
                });
        }       

        onPanelsHidden = () => {
            this.isPropertyPreviewPanelVisible = Enums.SidePanelState.Closed;
        };

        showPropertyPreview = (property: Business.PreviewProperty) => {
            this.components.propertyPreview().setProperty(property);
            this.showPanel(this.components.panels.propertyPreview);

            this.latestViewsProvider.addView({
                entityId: property.id,
                entityType: EntityType.Property
            });
        }

        addSavedAttachmentToList = (result: Dto.IAttachment) => {
            var savedAttachment = new Business.Attachment(result);
            this.activity.attachments.push(savedAttachment);
        }

        saveAttachment = (attachment: Antares.Common.Component.Attachment.AttachmentUploadCardModel) => {
            return this.activityAttachmentResource.save({ id: this.activity.id }, new Antares.Activity.Command.ActivityAttachmentSaveCommand(this.activity.id, attachment))
                .$promise;
        }

        showViewingPreview = (viewing: Common.Models.Dto.IViewing) => {
            this.selectedViewing = viewing;
            this.showPanel(this.components.panels.previewViewingsSidePanel);
        };
        showOfferPreview = (offer: Common.Models.Dto.IOffer) => {
            this.selectedOffer = offer;
            this.showPanel(this.components.panels.offerPreview);
        };

        cancelViewingPreview() {
            this.hidePanels();
        }

        goToEdit = () => {
            this.$state.go('app.activity-edit', { id: this.$state.params['id'] });
        };
        navigateToOfferView = (offer: Common.Models.Dto.IOffer) => {
            this.$state.go('app.offer-view', { id: offer.id });
        };

        defineComponentIds() {
            this.componentIds = {
                propertyPreviewId: 'viewActivity:propertyPreviewComponent',
                propertyPreviewSidePanelId: 'viewActivity:propertyPreviewSidePanelComponent',
                previewViewingSidePanelId: 'viewActivity:previewViewingSidePanelComponent',
                viewingPreviewId: 'viewActivity:viewingPreviewComponent',
                offerPreviewId: 'viewActivity:offerPreviewComponent',
                offerPreviewSidePanelId: 'viewActivity:offerPreviewSidePanelComponent'
            };
        }

        defineComponents() {
            this.components = {
                viewingPreview: () => { return this.componentRegistry.get(this.componentIds.viewingPreviewId); },
                offerPreview: () => { return this.componentRegistry.get(this.componentIds.offerPreviewId); },
                panels: {
                    previewViewingsSidePanel: () => { return this.componentRegistry.get(this.componentIds.previewViewingSidePanelId); },
                    offerPreview: () => { return this.componentRegistry.get(this.componentIds.offerPreviewSidePanelId); }
                }
            };
        }
    }

    angular.module('app').controller('activityViewController', ActivityViewController);
}
