/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateOfferCommand {
        id: string;
        statusId: string;
        price: number;
        pricePerWeek: number;
        offerDate: Date;
        exchangeDate: Date;
        completionDate: Date;
        specialConditions: string;
        searchStatusId: string;
        mortgageSurveyStatusId: string;
        mortgageStatusId: string;
        additionalSurveyStatusId: string;
        brokerId: string;
        brokerCompanyId: string;
        lenderId: string;
        lenderCompanyId: string;
        surveyorId: string;
        surveyorCompanyId: string;
        additionalSurveyorId: string;
        additionalSurveyorCompanyId: string;
        enquiriesId: string;
        contractApproved: boolean;
        mortgageLoanToValue: number;
        mortgageSurveyDate: Date | string;
        additionalSurveyDate: Date | string;
        progressComment: string;
        vendorSolicitorId: string = null;
        vendorSolicitorCompanyId: string = null;
        applicantSolicitorId: string = null;
        applicantSolicitorCompanyId: string = null;

        constructor(model?: Offer) {
            model = model || <Offer>{};
            this.id = model.id;
            this.statusId = model.statusId;
            this.price = model.price;
            this.pricePerWeek = model.pricePerWeek;
            this.offerDate = Core.DateTimeUtils.createDateAsUtc(model.offerDate);
            this.exchangeDate = Core.DateTimeUtils.createDateAsUtc(model.exchangeDate);
            this.completionDate = Core.DateTimeUtils.createDateAsUtc(model.completionDate);
            this.specialConditions = model.specialConditions;
            this.searchStatusId = model.searchStatusId;
            this.mortgageSurveyStatusId = model.mortgageSurveyStatusId;
            this.mortgageStatusId = model.mortgageStatusId;
            this.additionalSurveyStatusId = model.additionalSurveyStatusId;
            this.brokerId = model.brokerId;
            this.brokerCompanyId = model.brokerCompanyId;
            this.lenderId = model.lenderId;
            this.lenderCompanyId = model.lenderCompanyId;
            this.surveyorId = model.surveyorId;
            this.surveyorCompanyId = model.surveyorCompanyId;
            this.additionalSurveyorId = model.additionalSurveyorId;
            this.additionalSurveyorCompanyId = model.additionalSurveyorCompanyId;
            this.enquiriesId = model.enquiriesId;
            this.contractApproved = model.contractApproved;
            this.mortgageLoanToValue = model.mortgageLoanToValue;
            this.mortgageSurveyDate = Core.DateTimeUtils.createDateAsUtc(model.mortgageSurveyDate);
            this.additionalSurveyDate = Core.DateTimeUtils.createDateAsUtc(model.additionalSurveyDate);
            this.progressComment = model.progressComment;

            this.vendorSolicitorId = model.activity && model.activity.solicitor ? model.activity.solicitor.id : null;
            this.vendorSolicitorCompanyId = model.activity && model.activity.solicitorCompany ? model.activity.solicitorCompany.id : null;
            this.applicantSolicitorId = model.requirement && model.requirement.solicitor ? model.requirement.solicitor.id : null;
            this.applicantSolicitorCompanyId = model.requirement && model.requirement.solicitorCompany ? model.requirement.solicitorCompany.id : null;
        }

        public setBrokerCompanyContact = (companyContact: Business.CompanyContactRelation) => {
            this.brokerId = companyContact && companyContact.contact.id;
            this.brokerCompanyId = companyContact && companyContact.company.id;
        }

        public setLenderCompanyContact = (companyContact: Business.CompanyContactRelation) => {
            this.lenderId = companyContact && companyContact.contact.id;
            this.lenderCompanyId = companyContact && companyContact.company.id;
        }

        public setSurveyorCompanyContact = (companyContact: Business.CompanyContactRelation) => {
            this.surveyorId = companyContact && companyContact.contact.id;
            this.surveyorCompanyId = companyContact && companyContact.company.id;
        }

        public setAdditionalSurveyorCompanyContact = (companyContact: Business.CompanyContactRelation) => {
            this.additionalSurveyorId = companyContact && companyContact.contact.id;
            this.additionalSurveyorCompanyId = companyContact && companyContact.company.id;
        }

        public setVendorSolicitorCompanyContact = (companyContact: Business.CompanyContactRelation) => {
            this.vendorSolicitorId = companyContact && companyContact.contact.id;
            this.vendorSolicitorCompanyId = companyContact && companyContact.company.id;
        }

        public setApplicantSolicitorCompanyContact = (companyContact: Business.CompanyContactRelation) => {
            this.applicantSolicitorId = companyContact && companyContact.contact.id;
            this.applicantSolicitorCompanyId = companyContact && companyContact.company.id;
        }

    }
}