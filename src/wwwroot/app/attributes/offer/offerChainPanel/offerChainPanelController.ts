///<reference path="../../../typings/_all.d.ts"/>

module Antares.Attributes.Offer.OfferChain {
    import Dto = Common.Models.Dto;
    import Enums = Common.Models.Enums;
    import Business = Common.Models.Business;
    import CompanyContactType = Common.Models.Enums.CompanyContactType;

    export class OfferChainPanelController extends Common.Component.BaseSidePanelController {
        // bindings
        inPreviewMode: boolean;
        chainCommand: Common.Models.Commands.IChainTransactionCommand;
        chain: Business.ChainTransaction;
        chainType: Enums.OfferChainsType;
        isLastChain: boolean;

        // properties
        config: any;
        cardPristine: any;
        isBusy: boolean = false;
        panelMode: OfferChainPanelMode;
        offerChainPanelMode: any = OfferChainPanelMode;
        companyContactType: CompanyContactType;
        companyContacts: Business.CompanyContact[] = [];
        initialySelectedCompanyContact: Business.CompanyContactRelation;

        properties: Business.PreviewPropertyWithSelection[] = [];
        initiallySelectedPropertyId: string;

        constructor(
            private eventAggregator: Core.EventAggregator,
            private chainTransationsService: Services.ChainTransationsService,
            private propertyService: Services.PropertyService,
            private dataAccessService: any) {
            super();
        }

        public panelShown = () => {
            if (this.inPreviewMode) {
                this.panelMode = OfferChainPanelMode.Preview;
            } else {
                this.panelMode = OfferChainPanelMode.AddEdit;
            }
        }

        onCompanyContactSelected = (contacts: Business.CompanyContact[]) => {
            if (this.companyContactType === CompanyContactType.ThirdPartyAgent) {
                this.chain.agentCompanyContact = contacts[0];
            }
            else if (this.companyContactType === CompanyContactType.Solicitor) {
                this.chain.solicitorCompanyContact = contacts[0];
            }
            this.panelMode = OfferChainPanelMode.AddEdit;
        }

        onCompanyContactListCancel = () => {
            this.panelMode = OfferChainPanelMode.AddEdit;
        }

        openCompanyContactEditCard = (companyContact: Business.CompanyContactRelation, type: CompanyContactType) => {
            this.initialySelectedCompanyContact = companyContact;
            this.loadCompanyContacts();
            this.panelMode = OfferChainPanelMode.CompanyContactList;
            this.companyContactType = type;
        }

        openPropertyEditCard = (propertyId: string) => {
            this.initiallySelectedPropertyId = propertyId;
            this.loadProperties();
            this.panelMode = OfferChainPanelMode.Property;
        }

        public save = (chain: Business.ChainTransaction) => {
            this.isBusy = true;
            if (chain.id == null) {
                this.chainTransationsService.addChain(chain, this.chainCommand, this.chainType)
                    .then((model: Dto.IActivity | Dto.IRequirement) => {
                        this.onSuccessAddEditChain(chain, model);
                    })
                    .finally(() => { this.isBusy = false; });
            } else {
                this.chainTransationsService.editChain(chain, this.chainCommand, this.chainType)
                    .then((model: Dto.IActivity | Dto.IRequirement) => {
                        this.onSuccessAddEditChain(chain, model);
                    })
                    .finally(() => { this.isBusy = false; });
            }
        }

        public edit = (chain: Business.ChainTransaction) => {
            this.panelMode = OfferChainPanelMode.AddEdit;
        }

        public cancel = () => {
            this.isBusy = false;
            this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
        }

        public reloadConfig(chain: Business.ChainTransaction) {
            this.config = this.defineControlConfig(chain);
        }

        protected onChanges = (changesObj: any) => {
            this.reloadConfig(changesObj.chain.currentValue);
            this.resetState();
        }

        private resetState = () => {
            this.cardPristine = new Object();
        }

        loadCompanyContacts = () => {
            this.isBusy = true;
            this.dataAccessService
                .getCompanyContactResource()
                .query()
                .$promise.then((data: any) => {
                    this.companyContacts = data.map((dataItem: Dto.ICompanyContact) => new Business.CompanyContact(dataItem));

                    if (this.initialySelectedCompanyContact) {
                        this.companyContacts.forEach((current: any) => {
                            var shouldContactBeInitialySelected = current.companyId === this.initialySelectedCompanyContact.company.id && current.contactId === this.initialySelectedCompanyContact.contact.id;
                            current.selected = shouldContactBeInitialySelected;
                        });
                    }
                }).finally(() => {
                    this.isBusy = false;
                });
        }

        loadProperties = () => {
            this.isBusy = true;
            this.propertyService.getProperties()
            .then((data: Dto.IPreviewProperty[]) => {
                this.properties = data.map((dataItem: Dto.IPreviewProperty) => {
                    var property = new Business.PreviewPropertyWithSelection(dataItem);
                    if(property.id == this.initiallySelectedPropertyId){
                        property.selected = true;
                    }
                    return property;
                });
            }).finally(() => {
                this.isBusy = false;
            });
        }

        private defineControlConfig = (chain: Business.ChainTransaction) => {
            return {
                isEnd: { isEnd: { required: false, active: true } },
                property: { propertyId: { required: true, active: true } },
                vendor: { vendor: { required: true, active: true } },
                agentUser: chain != null && chain.agentUser != null ? { agentUserId: { required: true, active: true } } : null,
                agentCompanyContact: chain == null || chain.agentUser != null ? null : { agentContactId: { required: true, active: true }, agentCompanyId: { required: true, active: true } },
                solicitorCompanyContact: { solicitorContactId: { required: false, active: true }, solicitorCompanyId: { required: false, active: true } },
                mortgage: { mortgageId: { required: true, active: true } },
                survey: { surveyId: { required: true, active: true } },
                searches: { searchesId: { required: true, active: true } },
                enquiries: { enquiriesId: { required: true, active: true } },
                contractAgreed: { contractAgreedId: { required: true, active: true } },
                surveyDate: { surveyDate: { required: false, active: true } }
            }
        }

        private onSuccessAddEditChain = (chain: Business.ChainTransaction, model: Dto.IActivity | Dto.IRequirement) => {
            if (this.chainType === Enums.OfferChainsType.Activity) {
                var activity = <Dto.IActivity>model;
                this.eventAggregator.publish(new ActivityUpdatedOfferChainsEvent(activity));
            } else {
                var requirement = <Dto.IRequirement>model;
                this.eventAggregator.publish(new RequirementUpdatedOfferChainsEvent(requirement));
            }
            this.chain = chain;
            this.closeAddEdit();
        }

        private closeAddEdit = () => {
            if (this.inPreviewMode) {
                this.panelMode = OfferChainPanelMode.Preview;
            } else {
                this.cancel();
            }
        }


    }

    angular.module('app').controller('offerChainPanelController', OfferChainPanelController);
}