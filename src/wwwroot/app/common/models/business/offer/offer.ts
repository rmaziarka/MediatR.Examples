/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    declare var moment: any;

    export class Offer implements Dto.IOffer {
        [index: string]: any;
        id: string = null;
        statusId: string = null;
        activityId: string = null;
        requirementId: string = null;
        offerTypeId: string = null;
        price: number;
        pricePerWeek: number;
        exchangeDate: Date;
        completionDate: Date;
        specialConditions: string;
        negotiatorId: string;
        negotiator: Business.User;
        activity: Business.Activity;
        requirement: Business.Requirement;
        status: Business.EnumTypeItem;
        offerDate: Date = null;
        createdDate: Date = null;
        mortgageStatus: Business.EnumTypeItem = null;
        mortgageStatusId: string = null;
        mortgageSurveyStatus: Business.EnumTypeItem = null;
        mortgageSurveyStatusId: string = null;
        searchStatus: Business.EnumTypeItem = null;
        searchStatusId: string = null;
        enquiries: Business.EnumTypeItem = null;
        enquiriesId: string = null;
        contractApproved: boolean = false;
        mortgageLoanToValue: number = null;
        broker: Business.Contact = null;
        brokerId: string = null;
        brokerCompany: Business.Company = null;
        brokerCompanyId: string = null;
		lender: Business.Contact = null;
        lenderId: string = null;
        lenderCompany: Business.Company = null;
        lenderCompanyId: string = null;
        mortgageSurveyDate: Date | string = null;
        surveyor: Business.Contact = null;
        surveyorId: string = null;
        surveyorCompany: Business.Company = null;
        surveyorCompanyId: string = null;
		additionalSurveyor: Business.Contact = null;
        additionalSurveyorId: string = null;
        additionalSurveyorCompany: Business.Company = null;
        additionalSurveyorCompanyId: string = null;
		additionalSurveyStatus: Business.EnumTypeItem = null;
        additionalSurveyStatusId: string = null;
        additionalSurveyDate: Date | string = null;
        progressComment: string =  null;

        constructor(offer?: Dto.IOffer) {
            angular.extend(this, offer);
            if (offer) {
                this.activity = new Activity(offer.activity);
                this.requirement = new Requirement(offer.requirement);
                this.negotiator = new User(offer.negotiator);
                this.offerDate = moment(offer.offerDate).toDate();
                this.createdDate = moment(offer.createdDate).toDate();

                if (offer.completionDate) {
                    this.completionDate = moment(offer.completionDate).toDate();
                }

                if (offer.exchangeDate) {
                    this.exchangeDate = moment(offer.exchangeDate).toDate();
                }

                if (offer.broker) {
                    this.broker = new Contact(offer.broker, offer.brokerCompany);
                }

                if (offer.lender) {
                    this.lender = new Contact(offer.lender, offer.lenderCompany);
                }
                
                if (offer.surveyor) {
                    this.surveyor = new Contact(offer.surveyor, offer.surveyorCompany);
                }

                if (offer.additionalSurveyor) {
                    this.additionalSurveyor = new Contact(offer.additionalSurveyor, offer.additionalSurveyorCompany);
                }

                if (offer.brokerCompany) {
                    this.brokerCompany = new Company(offer.brokerCompany);
                }

                if (offer.lenderCompany) {
                    this.lenderCompany = new Company(offer.lenderCompany);
                }

                if (offer.surveyorCompany) {
                    this.surveyorCompany = new Company(offer.surveyorCompany);
                }

                if (offer.additionalSurveyorCompany) {
                    this.additionalSurveyorCompany = new Company(offer.additionalSurveyorCompany);
                }

                if (offer.mortgageSurveyDate) {
                    this.mortgageSurveyDate = new Date(<string>offer.mortgageSurveyDate);
                }

                if (offer.additionalSurveyDate) {
                    this.additionalSurveyDate = new Date(<string>offer.additionalSurveyDate);
                }
            }
        }

        clearProgressData() {
            this.mortgageStatusId = null;
            this.mortgageSurveyStatusId = null;
            this.additionalSurveyStatusId = null;
            this.searchStatusId = null;
            this.enquiriesId = null;
            this.contractApproved = null;
            this.contractApproved = null;
            this.mortgageLoanToValue = null;
            this.brokerId = null;
            this.brokerCompanyId = null;
            this.lenderId = null;
            this.lenderCompanyId = null;
            this.mortgageSurveyDate = null;
            this.surveyorId = null;
            this.surveyorCompanyId = null;
            this.additionalSurveyDate = null;
            this.additionalSurveyorId = null;
            this.additionalSurveyorCompanyId = null;
            this.progressComment = null;
        }
    }
}