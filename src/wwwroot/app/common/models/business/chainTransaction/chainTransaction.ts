/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ChainTransaction {
        id: string = '';
        activity: Business.Activity = null;
        requirement: Business.Requirement = null;
        parent: ChainTransaction = null;
        isEnd: boolean = false;
        property: Business.Property = null;
        vendor: string = '';
        agentUser: Business.User = null;
        agentContact: Business.Contact = null;
        agentCompany: Business.Company = null;
        solicitorContact: Business.Contact = null;
        solicitorCompany: Business.Company = null;
        mortgage: Business.EnumTypeItem = null;
        survey: Business.EnumTypeItem = null;
        searches: Business.EnumTypeItem = null;
        enquiries: Business.EnumTypeItem = null;
        contractAgreed: Business.EnumTypeItem = null;
        surveyDate: Date | string = null;

        constructor(chainTransaction?: Dto.IChainTransaction) {
            angular.extend(this, chainTransaction);
            if (chainTransaction) {
                this.property = new Business.Property(chainTransaction.property);
                if (chainTransaction.activity) {
                    this.activity = new Business.Activity(chainTransaction.activity);
                }
                if (chainTransaction.requirement) {
                    this.requirement = new Business.Requirement(chainTransaction.requirement);
                }
                if (chainTransaction.parent) {
                    this.parent = new ChainTransaction(chainTransaction.parent);
                }
                if (chainTransaction.agentUser) {
                    this.agentUser = new User(chainTransaction.agentUser);
                }
                if (chainTransaction.agentContact) {
                    this.agentContact = new Contact(chainTransaction.agentContact);
                }
                if (chainTransaction.agentCompany) {
                    this.agentCompany = new Company(chainTransaction.agentCompany);
                }
                if (chainTransaction.solicitorContact) {
                    this.solicitorContact = new Contact(chainTransaction.solicitorContact);
                }
                if (chainTransaction.solicitorCompany) {
                    this.solicitorCompany = new Company(chainTransaction.solicitorCompany);
                }
                if (chainTransaction.surveyDate) {
                    this.surveyDate = new Date(<string>chainTransaction.surveyDate);
                }
            }
        }
    }
}