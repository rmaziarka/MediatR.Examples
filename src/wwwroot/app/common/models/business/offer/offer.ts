/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    declare var moment: any;

    export class Offer implements Dto.IOffer {
        id: string = null;
        statusId: string = null;
        activityId: string = null;
        requirementId: string = null;
        price: number;
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
        mortgageLoanToValue: number;
        broker: Business.Contact = null;
        brokerId: string = null;
        lender: Business.Contact = null;
        lenderId: string = null;
        mortgageSurveyDate: Date = null;
        surveyor: Business.Contact = null;
        surveyorId: string = null;
        additionalSurveyor: Business.Contact = null;
        additionalSurveyorId: string = null;
        additionalSurveyStatus: Business.EnumTypeItem = null;
        additionalSurveyStatusId: string = null;
        additionalSurveyDate: Date = null;
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

                if (this.broker) {
                    this.broker = new Contact(offer.broker);
                }

                if (this.lender) {
                    this.lender = new Contact(offer.lender);
                }
                
                if (this.surveyor) {
                    this.surveyor = new Contact(offer.surveyor);
                }

                if (this.additionalSurveyor) {
                    this.additionalSurveyor = new Contact(offer.additionalSurveyor);
                }
            }
        }
    }
}