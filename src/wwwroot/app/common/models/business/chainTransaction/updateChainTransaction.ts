/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class UpdateChainTransaction {
        id: string = null;
        activityId: string = null;
        requirementId: string = null;
        propertyId: string = null;
        mortgageId: string = null;
        surveyId: string = null;
        searchesId: string = null;
        enquiriesId: string = null;
        contractAgreedId: string = null;
        parentId: string = null;
        isEnd: boolean = false;
        vendor: string = null;
        agentUserId: string = null;
        agentContactId: string = null;
        agentCompanyId: string = null;
        solicitorContactId: string = null;
        solicitorCompanyId: string = null;
        surveyDate: Date | string = null;

        constructor(chainTransaction?: Business.ChainTransaction) {
            angular.extend(this, chainTransaction);
            if (chainTransaction) {
                if (chainTransaction.agentUser) {
                    this.agentContactId = null;
                    this.agentCompanyId = null;
                    this.agentUserId = chainTransaction.agentUser.id;
                } else if (chainTransaction.agentCompanyContact) {
                    this.agentUserId = null;
                    this.agentContactId = chainTransaction.agentCompanyContact.contact.id;
                    this.agentCompanyId = chainTransaction.agentCompanyContact.company.id;
                }
                else {
                    this.agentUserId = null;
                    this.agentContactId = null;
                    this.agentCompanyId = null;
                }

                if (chainTransaction.solicitorCompanyContact) {
                    this.solicitorContactId = chainTransaction.solicitorCompanyContact.contact.id;
                    this.solicitorCompanyId = chainTransaction.solicitorCompanyContact.company.id;
                } else {
                    this.solicitorContactId = null;
                    this.solicitorCompanyId = null;
                }
                
                if (chainTransaction.surveyDate) {
                    this.surveyDate = new Date(<string>chainTransaction.surveyDate);
                }

            }
        }
    }
}