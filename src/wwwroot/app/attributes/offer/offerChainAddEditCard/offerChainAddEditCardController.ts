/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import SearchOptions = Common.Component.SearchOptions;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;
    import CompanyContactType = Common.Models.Enums.CompanyContactType;
    import OpenCompanyContactEditPanelEvent = Attributes.OpenCompanyContactEditPanelEvent;
    import Enums = Common.Models.Enums;

    export class OfferChainAddEditCardController {
        // bindings
        chain: Business.ChainTransaction;
        config: any;
        onEdit: () => void;
        onCancel: () => void;
        onReloadConfig: (chain: Business.ChainTransaction) => void;
        onEditCompanyContact: (obj: { companyContact: Common.Models.Business.CompanyContactRelation, type: CompanyContactType }) => void;
        onSave: (obj: { chain: Business.ChainTransaction }) => void;

        //properties
        offerChainAddEditCardForm: ng.IFormController;
        configAgentType: Dto.IControlConfig = <Dto.IControlConfig>{ required: true, active: true };

        public isThirdPartyAgentInEditMode: boolean = true;
        public thirdPartyAgentSearchOptions: SearchOptions = new SearchOptions();
        public usersSearchMaxCount: number = 100;
        public isKnightFrankAgent: boolean = true;

        isThirdPartyAgentEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        public companyContactType = Common.Models.Enums.CompanyContactType;

        // controls
        controlSchemas: any = {
            isEnd: <any>{
                formName: "isEndControlForm",
                controlId: "offer-chain-edit-is-end",
                translationKey: "OFFER.CHAIN.EDIT.IS_END",
                fieldName: "isEnd"
            },
            property: <any>{
                controlId: "offer-chain-edit-property",
                translationKey: "OFFER.CHAIN.EDIT.PROPERTY"
            },
            vendor: <any>{
                formName: "vendorControlForm",
                controlId: "offer-chain-edit-vendor",
                translationKey: "OFFER.CHAIN.EDIT.VENDOR",
                fieldName: "vendor"
            },
            thirdPartyAgent: <any>{
                formName: "thirdPartyAgentControlForm",
                controlId: "offer-chain-edit-agent-user",
                translationKey: "OFFER.CHAIN.EDIT.AGENT",
                emptyTranslationKey: "OFFER.CHAIN.EDIT.NO_AGENT",
                fieldName: "thirdPartyAgent"
            },
            agentUser: <any>{
                controlId: "offer-chain-edit-agent-user",
                translationKey: "OFFER.CHAIN.EDIT.AGENT"
            },
            solicitorCompanyContact: <Attributes.ICompanyContactViewControlSchema>{
                formName: "solicitorCompanyContactControlForm",
                controlId: "offer-chain-edit-solicitor",
                translationKey: "OFFER.CHAIN.EDIT.SOLICITOR",
                emptyTranslationKey: "OFFER.CHAIN.EDIT.NO_SOLICITOR",
                fieldName: "solicitorCompanyContact"
            },
            mortgage: <Attributes.IEnumItemControlSchema>{
                formName: "mortgageControlForm",
                controlId: "offer-chain-edit-mortgage",
                translationKey: "OFFER.CHAIN.EDIT.MORTGAGE",
                fieldName: "mortgageId",
                enumTypeCode: Dto.EnumTypeCode.ChainMortgageStatus
            },
            survey: <Attributes.IEnumItemControlSchema>{
                formName: "surveyControlForm",
                controlId: "offer-chain-edit-survey",
                translationKey: "OFFER.CHAIN.EDIT.SURVEY",
                fieldName: "surveyId",
                enumTypeCode: Dto.EnumTypeCode.ChainMortgageSurveyStatus
            },
            searches: <Attributes.IEnumItemControlSchema>{
                formName: "searchesControlForm",
                controlId: "offer-chain-edit-searches",
                translationKey: "OFFER.CHAIN.EDIT.SEARCHES",
                fieldName: "searchesId",
                enumTypeCode: Dto.EnumTypeCode.ChainSearchStatus
            },
            enquiries: <Attributes.IEnumItemControlSchema>{
                formName: "enquiriesControlForm",
                controlId: "offer-chain-edit-enquiries",
                translationKey: "OFFER.CHAIN.EDIT.ENQUIRIES",
                fieldName: "enquiriesId",
                enumTypeCode: Dto.EnumTypeCode.ChainEnquiries
            },
            contractAgreed: <Attributes.IEnumItemControlSchema>{
                formName: "contractAgreedeControlForm",
                controlId: "offer-chain-edit-contract-agreed",
                translationKey: "OFFER.CHAIN.EDIT.CONTRACT_AGREED",
                fieldName: "contractAgreedId",
                enumTypeCode: Dto.EnumTypeCode.ChainContractAgreedStatus
            },
            surveyDate: <Attributes.IDateEditControlSchema>{
                formName: "surveyDateControlForm",
                controlId: "offer-chain-edit-survey-date",
                translationKey: "OFFER.CHAIN.EDIT.SURVEY_DATE",
                fieldName: "surveyDate"
            },
            agentType: <Attributes.IRadioButtonsEditControlSchema>{
                formName: "chainEditAgentTypeForm",
                fieldName: "chainEditAgentType",
                translationKey: "OFFER.CHAIN.EDIT.AGENT",
                radioButtons: [
                    { value: true, translationKey: "OFFER.CHAIN.EDIT.KNIGHT_FRANK" },
                    { value: false, translationKey: "OFFER.CHAIN.EDIT.THIRD_PARTY" }]
            }
        }

        constructor(
            private dataAccessService: Services.DataAccessService,
            private eventAggregator: Core.EventAggregator) {
        }

        public editCompanyContact = (companyContact: Common.Models.Business.CompanyContactRelation, type: CompanyContactType) => {
            this.onEditCompanyContact({ companyContact: companyContact, type: type });
        }

        public cancel = () => {
            this.onCancel();
        }

        public editThirdPartyAgent = () => {
            this.isThirdPartyAgentInEditMode = true;
        }

        public changeThirdPartyAgent = (user: Business.User) => {
            this.chain.agentUser = user;
            this.isThirdPartyAgentInEditMode = false;
        }

        public cancelChangeThirdPartyAgent = () => {
            this.isThirdPartyAgentInEditMode = false;
        }

        public save = () => {
            this.onSave({ chain: angular.copy(this.chain) });
        }

        public getUsersQuery = (searchValue: string): DepartmentUserResourceParameters => {
            return { partialName: searchValue, take: this.usersSearchMaxCount, 'excludedIds[]': [] };
        }

        public getUsers = (searchValue: string) => {
            var query = this.getUsersQuery(searchValue);

            //TODO: create and use service
            return this.dataAccessService
                .getDepartmentUserResource()
                .query(query)
                .$promise
                .then((users: any) => {
                    return users.map((user: Common.Models.Dto.IUser) => { return new Common.Models.Business.User(<Common.Models.Dto.IUser>user); });
                });
        }
    }

    angular.module('app').controller('offerChainAddEditCardController', OfferChainAddEditCardController);
}