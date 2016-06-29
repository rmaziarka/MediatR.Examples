///<reference path="../../typings/_all.d.ts"/>

module Antares.Offer {
    export module Component {
        import Dto = Common.Models.Dto;
        import Enums = Antares.Common.Models.Enums;
        import Services = Antares.Services;
        import Business = Antares.Common.Models.Business;

        export enum OfferPanelMode {
            Edit,
            Preview
        }

        export class OfferEditPreviewPanelController extends Antares.Common.Component.BaseSidePanelController {
            // bindings
            config: IOfferConfig;
            offer: Dto.IOffer;
            mode: OfferPanelMode;

            // properties
            cardPristine: any;
            isBusy: boolean = false;
            busyLabelKey: string;
            isPreviewCardVisible: boolean = false;
            isEditCardVisible: boolean = false;
            backToPreview: boolean = false;

            constructor(
                private configService: Services.ConfigService,
                private eventAggregator: Antares.Core.EventAggregator,
                private offerService: Services.OfferService) {
                super();
            }

            private resetState = () => {
                this.cardPristine = new Object();
                this.config = null;
                this.offer.requirement = null;
                this.busyLabelKey = null;

                if (this.mode === OfferPanelMode.Preview) {
                    this.isPreviewCardVisible = true;
                    this.isEditCardVisible = false;
                    this.loadConfig(Enums.PageTypeEnum.Preview, this.offer);
                } else if (this.mode === OfferPanelMode.Edit) {
                    this.isPreviewCardVisible = false;
                    this.isEditCardVisible = true;
                    this.loadConfig(Enums.PageTypeEnum.Update, this.offer);
                }
            }

            panelShown = () => {
                this.backToPreview = this.mode === OfferPanelMode.Preview;
                this.resetState();
            }

            private loadConfig = (pageType: Enums.PageTypeEnum, offer: Dto.IOffer) => {
                this.isBusy = true;

                // TODO: update after merge with letting requirement
                var requirementTypeId = '128E59E5-013D-E611-8299-8CDCD4521601';
                //this.requirementTypeId = this.requirement.typeId;

                this.configService
                    .getOffer(pageType, requirementTypeId, offer.offerTypeId, offer)
                    .then(this.configLoaded)
                    .finally(() => { this.isBusy = false; });
            }

            private configLoaded = (newConfig: IOfferConfig) => {
                this.config = newConfig;
            }

            save = (offer: Dto.IOffer) => {
                this.isBusy = true;
                this.busyLabelKey = 'OFFER.ADD.SAVING_OFFER_IN_PROGRESS';

                offer.offerDate = Core.DateTimeUtils.createDateAsUtc(offer.offerDate);
                offer.exchangeDate = Core.DateTimeUtils.createDateAsUtc(offer.exchangeDate);
                offer.completionDate = Core.DateTimeUtils.createDateAsUtc(offer.completionDate);

                this.offerService.updateOffer(offer).then((offerDto: Dto.IOffer) => {
                    this.eventAggregator.publish(new Offer.OfferUpdatedSidePanelEvent(offerDto));
                    this.offer = new Business.Offer(offerDto);
                    this.closeEdit();
                }).finally(() => { this.isBusy = false; });
            }

            cancel = () => {
                this.closeEdit();
            }

            private closeEdit = () => {
                this.isBusy = false;
                if (this.backToPreview) {
                    this.preview();
                }
                else {
                    this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
                }
            }

            preview = () => {
                this.mode = OfferPanelMode.Preview;
                this.resetState();
            }

            edit = () => {
                this.mode = OfferPanelMode.Edit;
                this.resetState();
            }
        }

        angular.module('app').controller('offerEditPreviewPanelController', OfferEditPreviewPanelController);
    }
}