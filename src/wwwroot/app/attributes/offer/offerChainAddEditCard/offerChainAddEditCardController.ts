/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import SearchOptions = Common.Component.SearchOptions;
    import DepartmentUserResourceParameters = Common.Models.Resources.IDepartmentUserResourceParameters;
    import CompanyContactType = Common.Models.Enums.CompanyContactType;
    import Enums = Common.Models.Enums;
    import ISearchUserControlSchema = Attributes.ISearchUserControlSchema;

    export class OfferChainAddEditCardController {
        // bindings
        chain: Business.ChainTransaction;
        isAgentUserType: boolean;
        config: any;
        onCancel: () => void;
        onSave: (obj: { chain: Business.ChainTransaction }) => void;
        onReloadConfig: (obj: { isAgentUserType: boolean }) => void;
        onEditCompanyContact: (obj: { companyContact: Common.Models.Business.CompanyContactRelation, type: CompanyContactType }) => void;
        onAddEditProperty: (obj: { property: Business.PreviewProperty }) => void;
        pristineFlag: any;

        //properties
        offerChainAddEditCardForm: ng.IFormController;
        configAgentType: Dto.IControlConfig = <Dto.IControlConfig>{ required: true, active: true };


        isThirdPartyAgentEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        public companyContactType = Common.Models.Enums.CompanyContactType;

        // controls
        controlSchemas: any = {
            searchUser: <ISearchUserControlSchema>{
                formName: "searchUserControlForm",
                controlId: "offer-chain-search-user",
                searchPlaceholderTranslationKey: "OFFER.CHAIN.EDIT.FIND_AGENT",
                fieldName: "searchUser",
                itemTemplateUrl: "app/attributes/activityNegotiators/userSearchTemplate.html",
                usersSearchMaxCount: 100
            },
            isEnd: <any>{
                formName: "isEndControlForm",
                controlId: "offer-chain-edit-is-end",
                translationKey: "OFFER.CHAIN.EDIT.IS_END",
                fieldName: "isEnd"
            },
            vendor: <any>{
                formName: "vendorControlForm",
                controlId: "offer-chain-edit-vendor",
                translationKey: "OFFER.CHAIN.EDIT.VENDOR",
                fieldName: "vendor"
            },
            agentUser: <any>{
                formName: "agentUserForm",
                controlId: "offer-chain-edit-agent-user",
                translationKey: "OFFER.CHAIN.EDIT.AGENT",
                fieldName: "agentUserId"
            },
            agentCompanyContact: <any>{
                formName: "agentCompanyContactForm",
                controlId: "offer-chain-edit-agent-company-contact",
                translationKey: "OFFER.CHAIN.EDIT.AGENT",
                emptyTranslationKey: "OFFER.CHAIN.EDIT.NO_AGENT",
                fieldName: "agentCompanyContact"
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

        $onChanges = (obj: any) => {
            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.pristineFlag.previousValue) {
                if (this.offerChainAddEditCardForm) {
                    this.offerChainAddEditCardForm.$setPristine();
                }
            }
        }

        public onAgentUserTypeChange =  (value: boolean) => { 
            this.onReloadConfig({ isAgentUserType: value }); 
        }

        public editCompanyContact = (companyContact: Common.Models.Business.CompanyContactRelation, type: CompanyContactType) => {
            this.onEditCompanyContact({ companyContact: companyContact, type: type });
        }

        public addEditProperty = (property: Business.PreviewProperty) => {
            this.onAddEditProperty({ property: property });
        }

        public cancel = () => {
            this.onCancel();
        }

        public save = () => {
            this.onSave({ chain: angular.copy(this.chain) });
        }
    }

    angular.module('app').controller('offerChainAddEditCardController', OfferChainAddEditCardController);
}