///<reference path="../../typings/_all.d.ts"/>

module Antares.Offer {
    export module Component {
        import Dto = Common.Models.Dto;
        import Enums = Antares.Common.Models.Enums;
        import Services = Antares.Services;
        import Business = Antares.Common.Models.Business;

        export class OfferAddPanelController extends Antares.Common.Component.BaseSidePanelController {
            // bindings
            config: IOfferAddPanelConfig;
            activity: Dto.IActivity;
            requirement: any;
            isVisible: Enums.SidePanelState;

            // properties
            cardPristine: any;
            isBusy: boolean = false;
            offerTypes: Dto.IOfferType[];
            offerTypeId: string;
            pageType: Enums.PageTypeEnum = Enums.PageTypeEnum.Create;

            constructor(
                private configService: Services.ConfigService,
                private eventAggregator: Antares.Core.EventAggregator,
                private offerService: Services.OfferService) {
                super();
            }

            panelShown = () => {
                this.cardPristine = new Object();
                this.config = null;

                this.isBusy = true;
                this.loadOfferTypes()
                    .then(() => { this.loadConfig(<Business.CreateOfferCommand>{}); })
                    .finally(() => { this.isBusy = false; });
            }

            private loadOfferTypes = () => {
                return this.offerService.getOfferTypes()
                    .then((result: Array<Dto.IOfferType>) => {
                        this.offerTypes = result;
                        this.offerTypeId = _.find(this.offerTypes, { enumCode: this.requirement.requirementType.enumCode }).id;
                        return result;
                    });
            }

            private loadConfig = (command: Business.CreateOfferCommand) => {
                var requirementTypeId = this.requirement.requirementTypeId;
                return this.configService
                    .getOffer(this.pageType, requirementTypeId, this.offerTypeId, command)
                    .then((newConfig: IOfferAddPanelConfig) => {
                        this.config = newConfig;
                        return newConfig;
                    });
            }

            save = (offer: Business.CreateOfferCommand) => {
                this.isBusy = true;

                offer.requirementId = this.requirement.id;
                offer.activityId = this.activity.id;
                offer.offerTypeId = this.offerTypeId;
                offer.offerDate = Core.DateTimeUtils.createDateAsUtc(offer.offerDate);
                offer.exchangeDate = Core.DateTimeUtils.createDateAsUtc(offer.exchangeDate);
                offer.completionDate = Core.DateTimeUtils.createDateAsUtc(offer.completionDate);

                this.offerService.createOffer(offer).then((offerDto: Dto.IOffer) => {
                    this.eventAggregator.publish(new Offer.OfferAddedSidePanelEvent(offerDto));
                    this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
                }).finally(() => { this.isBusy = false; });
            }

            cancel = () => {
                this.isBusy = false;
                this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
            }

            reloadConfig = (command: Business.CreateOfferCommand) => {
                this.isBusy = true;
                this.loadConfig(command).finally(() => { this.isBusy = false; });
            }
        }

        angular.module('app').controller('offerAddPanelController', OfferAddPanelController);
    }
}