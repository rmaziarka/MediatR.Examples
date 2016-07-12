/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class ChainTransaction implements Dto.IChainTransaction {
        id: string = '';
        propertyId: string = '';
        mortgageId: string = '';
        surveyId: string = '';
        searchesId: string = '';
        enquiriesId: string = '';
        contractAgreedId: string = '';
        activity: Business.Activity = null;
        requirement: Business.Requirement = null;
        parent: ChainTransaction = null;
        isEnd: boolean = false;
        property: Business.PreviewProperty = null;
        vendor: string = '';
        agentUser: Business.User = null;
        agentContact: Business.Contact = null;
        agentCompany: Business.Company = null;
        agentCompanyContact: Business.CompanyContactRelation = null;
        solicitorContact: Business.Contact = null;
        solicitorCompany: Business.Company = null;
        solicitorCompanyContact: Business.CompanyContactRelation = null;
        mortgage: Business.EnumTypeItem = null;
        survey: Business.EnumTypeItem = null;
        searches: Business.EnumTypeItem = null;
        enquiries: Business.EnumTypeItem = null;
        contractAgreed: Business.EnumTypeItem = null;
        surveyDate: Date | string = null;
        isKnightFrankAgent: string = null;

        constructor(chainTransaction?: Dto.IChainTransaction) {
            angular.extend(this, chainTransaction);
            if (chainTransaction) {
                this.property = new Business.PreviewProperty(chainTransaction.property);
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
                if (chainTransaction.agentContact && chainTransaction.agentCompany) {
                    this.agentContact = new Contact(chainTransaction.agentContact);
                    this.agentCompany = new Company(chainTransaction.agentCompany);
                    this.agentCompanyContact = new CompanyContactRelation(this.agentContact, this.agentCompany);
                }
                if (chainTransaction.solicitorContact && chainTransaction.solicitorCompany) {
                    this.solicitorContact = new Contact(chainTransaction.solicitorContact);
                    this.solicitorCompany = new Company(chainTransaction.solicitorCompany);
                    this.solicitorCompanyContact = new CompanyContactRelation(this.solicitorContact, this.solicitorCompany);
                }
                if (chainTransaction.surveyDate) {
                    this.surveyDate = new Date(<string>chainTransaction.surveyDate);
                }
            }
        }
    }
}