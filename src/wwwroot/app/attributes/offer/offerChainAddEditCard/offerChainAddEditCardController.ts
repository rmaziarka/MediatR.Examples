﻿/// <reference path="../../../typings/_all.d.ts" />

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
        configAgentType: Dto.IControlConfig = <Dto.IControlConfig>{ chainEditAgentType: { required: true, active: true } };
        companyContactType = Common.Models.Enums.CompanyContactType;

        // controls
        controlSchemas: any = {
            searchUser: <ISearchUserControlSchema>{
                formName: "searchUserControlForm",
                emptyTranslationKey: "OFFER.CHAIN.EDIT.NO_AGENT",
                controlId: "offer-chain-search-user",
                searchPlaceholderTranslationKey: "OFFER.CHAIN.EDIT.FIND_NEGOTIATOR",
                fieldName: "agentUserId",
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
                fieldName: "vendor",
                placeholder: "OFFER.CHAIN.EDIT.NAME_AND_SURNAME",
                maxLength: 128
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
                translationKey: "",
                emptyTranslationKey: "OFFER.CHAIN.EDIT.NO_AGENT",
                fieldName: "agentContactId"
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
            private enumProvider: Providers.EnumProvider,
            private eventAggregator: Core.EventAggregator) {
        }

        $onChanges = (obj: any) => {
            if (obj.pristineFlag && obj.pristineFlag.currentValue !== obj.pristineFlag.previousValue) {
                if (this.offerChainAddEditCardForm) {
                    this.offerChainAddEditCardForm.$setPristine();
                }
            }

            if (obj.chain && obj.chain.currentValue && !obj.chain.currentValue.id) {
                this.setDefaultStatuses(this.chain);
            }
        }

        public agentUserChanged = (user: Business.User) => {
            this.chain.agentCompanyContact = null;
        }

        public onAgentUserTypeChange = (value: boolean) => {
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

        private setDefaultStatuses(chain: Business.ChainTransaction) {
            chain.mortgageId = this.getDefaultStatus(
                Common.Models.Enums.ChainMortgageStatus[Common.Models.Enums.ChainMortgageStatus.Unknown],
                Dto.EnumTypeCode.ChainMortgageStatus);

            chain.surveyId = this.getDefaultStatus(
                Common.Models.Enums.ChainMortgageSurveyStatus[Common.Models.Enums.ChainMortgageSurveyStatus.Unknown],
                Dto.EnumTypeCode.ChainMortgageSurveyStatus);

            chain.searchesId = this.getDefaultStatus(
                Common.Models.Enums.ChainSearchStatus[Common.Models.Enums.ChainSearchStatus.Outstanding],
                Dto.EnumTypeCode.ChainSearchStatus);

            chain.enquiriesId = this.getDefaultStatus(
                Common.Models.Enums.ChainEnquiries[Common.Models.Enums.ChainEnquiries.Outstanding],
                Dto.EnumTypeCode.ChainEnquiries);

            chain.contractAgreedId = this.getDefaultStatus(
                Common.Models.Enums.ChainContractAgreedStatus[Common.Models.Enums.ChainContractAgreedStatus.Outstanding],
                Dto.EnumTypeCode.ChainContractAgreedStatus);
        }

        private getDefaultStatus = (defaultStatusCode: string, enumDictionary: any): string => {
            var statuses = this.enumProvider.enums[enumDictionary];
            var defaultStatus: Dto.IEnumItem = _.find(statuses, { 'code': defaultStatusCode });
            if (defaultStatus) {
                return defaultStatus.id;
            }
            return null;
        }
    }

    angular.module('app').controller('offerChainAddEditCardController', OfferChainAddEditCardController);
}