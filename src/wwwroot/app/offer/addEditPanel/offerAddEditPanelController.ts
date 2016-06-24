///<reference path="../../typings/_all.d.ts"/>

module Antares.Offer {
    export module Component {
        import Dto = Common.Models.Dto;
        import Business = Common.Models.Business;
        import Enums = Antares.Common.Models.Enums;
        import Services = Antares.Services;
        import Commands = Antares.Services.Commands;
        declare var moment: any;

        export class OfferAddEditPanelController extends Antares.Common.Component.BaseSidePanelController {
            // bindings
            config: IOfferAddEditPanelConfig;
            activity: Dto.IActivity;
            requirement: Dto.IRequirement;
            mode: string;

            // properties
            cardPristine: any;
            isBusy: boolean = false;
            pageType: Enums.PageTypeEnum;
            offerTypeId: string;
            requirementTypeId: string;

            constructor(
                private configService: Services.ConfigService,
                private eventAggregator: Antares.Core.EventAggregator,
                private latestViewsProvider: Providers.LatestViewsProvider,
                private offerService: Services.OfferService) {
                super();
                this.pageType = this.mode === 'edit' ? Enums.PageTypeEnum.Update : Enums.PageTypeEnum.Create;
            }

            panelShown = () => {
                this.cardPristine = new Object();
                this.config = null;
                this.requirementTypeId = '094ADAA7-1C3A-E611-8299-8CDCD4521601';
                //this.requirementTypeId = this.requirement.typeId;
                this.offerTypeId = this.requirementTypeId;
                this.loadConfig(<Commands.OfferAddPanelCommand>{});
            }

            loadConfig = (command: Commands.OfferAddPanelCommand) => {
                this.isBusy = true;

                this.configService
                    .getOffer(this.pageType, this.requirementTypeId, this.offerTypeId, command)
                    .then(this.configLoaded)
                    .finally(() => { this.isBusy = false; });
            }

            configLoaded = (newConfig: IOfferAddEditPanelConfig) => {
                this.config = newConfig;
            }

            save = (offer: Commands.OfferAddPanelCommand) => {
                this.isBusy = true;

                this.offerService.addOffer(offer).then((offerDto: Dto.IOffer) =>{
                    this.eventAggregator.publish(new Offer.OfferAddedSidePanelEvent(offerDto));
                    this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());

                    this.latestViewsProvider.addView({
                        entityId: offerDto.id,
                        entityType : Enums.EntityTypeEnum.Offer
                    });
                }).finally(() =>{ this.isBusy = false; });

            }

            cancel = () => {
                this.isBusy = false;
                this.eventAggregator.publish(new Antares.Common.Component.CloseSidePanelEvent());
            }

            reloadConfig = (offer: Dto.IOffer) => {
                var command = new Commands.OfferAddPanelCommand(offer);
                this.loadConfig(command);
            }
        }

        angular.module('app').controller('offerAddEditPanelController', OfferAddEditPanelController);
    }
}