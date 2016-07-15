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

        // properties
        isAgentUserType: boolean;
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

        onPropertySelected = (properties: Business.PreviewProperty[]) => {
            this.chain.property = properties[0];
            this.chain.propertyId = properties[0] && properties[0].id;
            this.panelMode = OfferChainPanelMode.AddEdit;
        }

        onListCancel = () => {
            this.panelMode = OfferChainPanelMode.AddEdit;
        }

        openCompanyContactEditCard = (companyContact: Business.CompanyContactRelation, type: CompanyContactType) => {
            this.initialySelectedCompanyContact = companyContact;
            this.loadCompanyContacts();
            this.panelMode = OfferChainPanelMode.CompanyContactList;
            this.companyContactType = type;
        }

        openPropertyAddEditCard = (property: Business.PreviewProperty) => {
            this.initiallySelectedPropertyId = property && property.id;
            this.loadProperties();
            this.panelMode = OfferChainPanelMode.Property;
        }

        public save = (chain: Business.ChainTransaction) => {
            this.isBusy = true;

            var serverChain = this.createChainForServer(chain, this.chainCommand.chainTransactions);
            var chainCommand = angular.copy(this.chainCommand);

            if (serverChain.id == null) {
                this.chainTransationsService.addChain(serverChain, chainCommand, this.chainType)
                    .then((model: Dto.IActivity | Dto.IRequirement) => {
                        this.onSuccessAddEditChain(chain, model);
                    })
                    .finally(() => { this.isBusy = false; });
            } else {
                this.chainTransationsService.editChain(serverChain, chainCommand, this.chainType)
                    .then((model: Dto.IActivity | Dto.IRequirement) => {
                        this.onSuccessAddEditChain(chain, model);
                    })
                    .finally(() => { this.isBusy = false; });
            }
        }

        public edit = (chain: Business.ChainTransaction) => {
            this.panelMode = OfferChainPanelMode.AddEdit;
        }

        public reloadConfig(isAgentUserType: boolean) {
            this.config = this.defineControlConfig(isAgentUserType);
        }

        protected onChanges = (changesObj: IOfferChainPanelChange) => {
            if (changesObj.chain && changesObj.chain.currentValue) {
                var agentUserDefined = !!changesObj.chain.currentValue.agentUser;
                var agentContactDefined = !! changesObj.chain.currentValue.agentContact;

                this.isAgentUserType = agentUserDefined || !agentContactDefined;           
                this.reloadConfig(this.isAgentUserType);
                this.cardPristine = new Object();
            }
            if (changesObj.inPreviewMode) {
                this.panelMode = this.inPreviewMode 
                    ? OfferChainPanelMode.Preview 
                    : OfferChainPanelMode.AddEdit;
            }

            this.resetState();
        }

        private resetState = () => {
            if (this.inPreviewMode) {
                this.panelMode = OfferChainPanelMode.Preview;
            } else {
                this.panelMode = OfferChainPanelMode.AddEdit;
            }
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

        private defineControlConfig = (isAgentUserType: boolean) => {
            return {
                isEnd: { isEnd: { required: false, active: this.chain.lastElementInChainLink } },
                property: { propertyId: { required: true, active: true } },
                vendor: { vendor: { required: true, active: true } },
                agentUser: isAgentUserType ? { agentUserId: { required: true, active: true } } : null,
                agentCompanyContact: isAgentUserType ? null : { agentContactId: { required: true, active: true }, agentCompanyId: { required: true, active: true } },
                solicitorCompanyContact: { solicitorContactId: { required: false, active: true }, solicitorCompanyId: { required: false, active: true } },
                mortgage: { mortgageId: { required: true, active: true } },
                survey: { surveyId: { required: true, active: true } },
                searches: { searchesId: { required: true, active: true } },
                enquiries: { enquiriesId: { required: true, active: true } },
                contractAgreed: { contractAgreedId: { required: true, active: true } },
                surveyDate: { surveyDate: { required: false, active: true } }
            }
        }

        private createChainForServer = (chain: Business.ChainTransaction, chainTransactions: Dto.IChainTransaction[]): Business.ChainTransaction => {

            // TODO: chang to use dedicated ChainTransactionCommand in IChainTransactionCommand
            var command = angular.copy(chain);

            if(this.isAgentUserType && command.agentUser){
                command.agentContactId = null;
                command.agentCompanyId = null;
                command.agentUserId = command.agentUser.id;
            } else if(command.agentCompanyContact){
                command.agentUserId = null;
                command.agentContactId = command.agentCompanyContact.contact.id;
                command.agentCompanyId = command.agentCompanyContact.company.id;
            }
            else{
                command.agentContactId = null;
                command.agentCompanyId = null;
                command.agentUserId = null;
            }

            if(command.solicitorCompanyContact){
                command.solicitorContactId = command.solicitorCompanyContact.contact.id;
                command.solicitorCompanyId = command.solicitorCompanyContact.company.id;
            } else{
                command.solicitorContactId = null;
                command.solicitorCompanyId = null;
            }

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
                this.isBusy = false;
                this.eventAggregator.publish(new Common.Component.CloseSidePanelEvent());
            }
        }


    }

    interface IOfferChainPanelChange {
        chain: { currentValue: Business.ChainTransaction, previousValue: Business.ChainTransaction }
        inPreviewMode: { currentValue: boolean, previousValue: boolean }
    }

    angular.module('app').controller('offerChainPanelController', OfferChainPanelController);
}