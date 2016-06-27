///<reference path="../../typings/_all.d.ts"/>

module Antares.Offer {
    export module Component {
        import Dto = Common.Models.Dto;
        import Enums = Antares.Common.Models.Enums;
        import Services = Antares.Services;
        import Business = Antares.Common.Models.Business;
        declare var moment: any;

        export class OfferAddPanelController extends Antares.Common.Component.BaseSidePanelController {
            // bindings
            config: IOfferAddPanelConfig;
            activity: Dto.IActivity;
            requirement: Dto.IRequirement;
            
            // properties
            cardPristine: any;
            isBusy: boolean = false;
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
                this.loadConfig(<Business.CreateOfferCommand>{});
            }

            private loadConfig = (command: Business.CreateOfferCommand) => {
                this.isBusy = true;

                var requirementTypeId = '094ADAA7-1C3A-E611-8299-8CDCD4521601';
                //this.requirementTypeId = this.requirement.typeId;
                var offerTypeId = '094ADAA7-1C3A-E611-8299-8CDCD4521601';

                this.configService
                    .getOffer(this.pageType, requirementTypeId, offerTypeId, command)
                    .then(this.configLoaded)
                    .finally(() => { this.isBusy = false; });
            }

            private configLoaded = (newConfig: IOfferAddPanelConfig) => {
                this.config = newConfig;
            }

            save = (offer: Business.CreateOfferCommand) => {
                this.isBusy = true;
                offer.requirementId = this.requirement.id;
                offer.activityId = this.activity.id;
                this.offerService.createOffer(offer).then((offerDto: Dto.IOffer) =>{
                    this.eventAggregator.publish(new Offer.OfferAddedSidePanelEvent(offerDto));
                    this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
                }).finally(() =>{ this.isBusy = false; });
            }

            cancel = () => {
                this.isBusy = false;
                this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
            }

            reloadConfig = (command: Business.CreateOfferCommand) => {
                this.loadConfig(command);
            }
        }

        angular.module('app').controller('offerAddPanelController', OfferAddPanelController);
    }
}