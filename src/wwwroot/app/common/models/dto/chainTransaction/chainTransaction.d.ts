declare module Antares.Common.Models.Dto {
    interface  IChainTransaction {
        id: string;
        activityId?: string;
        activity?: Dto.IActivity;
        requirementId?: string;
        requirement?: Dto.IRequirement;
        parentId?: string;
        parent?: IChainTransaction;
        isEnd: boolean;
        propertyId: string;
        property: Dto.IPreviewProperty;
        vendor: string;
        agentUserId?: string;
        agentUser?: Dto.IUser;
        agentContactId?: string;
        agentContact?: Dto.IContact;
        agentCompanyId?: string;
        agentCompany?: Dto.ICompany;
        solicitorContactId?: string;
        solicitorContact?: Dto.IContact;
        solicitorCompanyId?: string;
        solicitorCompany?: Dto.ICompany;
        mortgageId: string;
        mortgage: Dto.IEnumTypeItem;
        surveyId: string;
        survey: Dto.IEnumTypeItem;
        searchesId: string;
        searches: Dto.IEnumTypeItem;
        enquiriesId: string;
        enquiries: Dto.IEnumTypeItem;
        contractAgreedId: string;
        contractAgreed: Dto.IEnumTypeItem;
        surveyDate?: Date | string;
        isKnightFrankAgent: string;
    }
}