/// <reference path="../../../typings/_all.d.ts" />

module Antares.Attributes.Offer.OfferChain {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import CompanyContactType = Common.Models.Enums.CompanyContactType;

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
        controlSchemas: OfferChainAddEditSchema = OfferChainAddEditSchema;

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