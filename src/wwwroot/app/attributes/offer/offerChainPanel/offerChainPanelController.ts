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
        cardPristine: any;
        config: any;
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
            this.cardPristine = new Object();
            if (this.inPreviewMode) {
                this.panelMode = OfferChainPanelMode.Preview;
            } else {
                this.panelMode = OfferChainPanelMode.AddEdit;
            }
        }

        onCompanyContactSelected = (contacts: Business.CompanyContact[]) => {
            switch (this.companyContactType) {
                case CompanyContactType.ChainAgent:
                    this.chain.agentCompanyContact = contacts[0];
                    break;
                case CompanyContactType.ChainSolicitor:
                    this.chain.solicitorCompanyContact = contacts[0];
                    break;
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

            chain.propertyId = '0D58971F-0049-E611-8115-00155D038C01';

            var chainCommand = this.createChainCommand(chain, this.chainCommand.chainTransactions);

            if (chainCommand.id == null) {
                this.chainTransationsService.addChain(chainCommand, this.chainCommand, this.chainType)
                    .then((model: Dto.IActivity | Dto.IRequirement) => {
                        this.onSuccessAddEditChain(chain, model);
                    })
                    .finally(() => { this.isBusy = false; });
            } else {
                this.chainTransationsService.editChain(chainCommand, this.chainCommand, this.chainType)
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
            if (changesObj.chain && changesObj.chain.currentValue) {
                this.reloadConfig(changesObj.chain.currentValue);
            }
            this.resetState();
        }

        private resetState = () => {
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

        private createChainCommand = (chain: Business.ChainTransaction, chainTransactions: Dto.IChainTransaction[]): Business.ChainTransaction => {

            // TODO: chang to use dedicated ChainTransactionCommand in IChainTransactionCommand
            var command = angular.copy(chain);
            command.activity = null;
            command.requirement = null;
            command.parent = null;
            command.property = null;
            command.agentUser = null;
            command.agentContact = null;
            command.agentCompany = null;
            command.agentCompanyContact = null;
            command.solicitorContact = null;
            command.solicitorCompany = null;
            command.solicitorCompanyContact = null;
            command.mortgage = null;
            command.survey = null;
            command.searches = null;
            command.enquiries = null;
            command.contractAgreed = null;

            if (command.id == null && chainTransactions != null && chainTransactions.length > 0) {
                command.parentId = chainTransactions[chainTransactions.length - 1].id;
            }

            return command;
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