namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    public enum ErrorMessage
    {
        /// <summary>
        /// Key value : Call date is required.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityUser_CallDate_Is_Required,
        /// <summary>
        /// Key value : Negotiator is assigned to other activity.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityUser_Is_Assigned_To_Other_Activity,
        /// <summary>
        /// Key value : One or more negotiators are duplicated.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_Negotiators_Not_Unique,
        /// <summary>
        /// Key value : Activity negotiator call date cannot be set in the past.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_Negotiator_CallDate_InPast,
        /// <summary>
        /// Key value : One or more {0}(s) with given ids does not exist in the database.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Entity_List_Item_Not_Exists,
        /// <summary>
        /// Key value : {0} with given id '{1}' does not exist in the database.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Entity_Not_Exists,
        /// <summary>
        /// Key value : Enum type {0} with given item id '{1}' does not exist in the database.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        EnumType_Item_Not_Exists,
        /// <summary>
        /// Key value : Inconsistent address country with address form definition.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Inconsistent_Address_Country_Id,
        /// <summary>
        /// Key value : {0} configuration is inconsistent.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Inconsistent_Dynamic_Configuration,
        /// <summary>
        /// Key value : One or more contacts do not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Activity_Contacts_Id,
        /// <summary>
        /// Key value : One or more contacts do not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Company_Contacts_Id,
        /// <summary>
        /// Key value : One or more attendees are not on the applicant list.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Requirement_Applicants_Id,
        /// <summary>
        /// Key value : Offer Date must be less than or equal to {0}.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        OfferDateLessOrEqualToCreateDate,
        /// <summary>
        /// Key value : Completion Date must be greater than or equal to {0}.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        CompletionDateGreaterOrEqualToCreateDate,
        /// <summary>
        /// Key value : Exchange Date must be greater than or equal to {0}.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ExchangeDateGreaterOrEqualToCreateDate,
        /// <summary>
        /// Key value : Mortgage Survey Date must be greater than or equal to {0}.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MortgageSurveyDateGreaterOrEqualToCreateDate,
        /// <summary>
        /// Key value : Additional Survey Date must be greater than or equal to {0}.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        AdditionalSurveyDateGreaterOrEqualToCreateDate,
        /// <summary>
        /// Key value : Enum value of the field {0} not passed
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Enum_Value_Not_Passed,
        /// <summary>
        /// Key value : One or more attendees are not on the applicant list.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Requirement_Attendees_Id,
        /// <summary>
        /// Key value : Only commercial property should have area breakdown.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Only_Commercial_Property_Should_Have_AreaBreakdown,
        /// <summary>
        /// Key value : The ownership dates overlap.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Ownership_Dates_Overlap,
        /// <summary>
        /// Key value : Area breakdown  is assigned to other property
        /// </summary>
        // ReSharper disable once InconsistentNaming
        PropertyAreaBreakdown_Is_Assigned_To_Other_Property,
        /// <summary>
        /// Key value : Specified order is out of available range.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        PropertyAreaBreakdown_OrderOutOfRange,
        /// <summary>
        /// Key value : The '{0}' property value format is inappropriate.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Property_Format_Is_Invalid,
        /// <summary>
        /// Key value : The '{0}' property value should be empty.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Property_Should_Be_Empty,
        /// <summary>
        /// Key value : The '{0}' property value is required.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Property_Should_Not_Be_Empty,
        /// <summary>
        /// Key value : Value of ActivityDepartment is invalid.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityDepartment_Invalid_Value,
        /// <summary>
        /// Key value : One or more departments are duplicated.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_Departments_Not_Unique,
        /// <summary>
        /// Key value : One or more attendees are duplicated.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_AppraisalMeetingAttendees_Not_Unique,
        /// <summary>
        /// Key value : One or more contacts are duplicated.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_Contacts_Not_Unique,
        /// <summary>
        /// Key value : Activity lead negotiator is required.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_LeadNegotiator_Is_Required,
        /// <summary>
        /// Key value : Activity should have exactly one managing department.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_Should_Have_Exactly_One_Managing_Department,
        /// <summary>
        /// Key value : One or more appraisal meeting attendees do not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Activity_Attendees_Id,
        /// <summary>
        /// Key value : One or more departments do not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Activity_Departments_Id,
        /// <summary>
        /// Key value : One or more chain transactions do not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Activity_ChainTransaction_Id,
        /// <summary>
        /// Key value : One or more negotiators do not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Activity_Negotiators_Id,
        /// <summary>
        /// Key value : One or more negotiators are duplicated.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Negotiators_Not_Unique,
        /// <summary>
        /// Key value : More then one chain transaction is marked as last chain transaction.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Activity_ChainTransactions_IsEndCountMoreThenOne,
        /// <summary>
        /// Key value : Only last chain transaction can be marked as end transaction.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionEnd_InvalidValue,
        /// <summary>
        /// Key value : Vendor value is longer than 128 characters.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityVendor_ValueToLong,
        /// <summary>
        /// Key value : 3rd party Agent has been selected, cannot use Knight Frank Agent.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionAgentContact_InvalidValue,
        /// <summary>
        /// Key value : Knight Frank Agent has been selected, cannot use 3rd party Agent.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionAgentUser_InvalidValue,
        /// <summary>
        /// Key value : You cannot add more than one transaction chain.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionAdd_MoreThanOne,
        /// <summary>
        /// Key value : Parent of added transaction chain must be last transaction chain.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionAdd_InvalidParentId,
        /// <summary>
        /// Key value : You cannot remove more than one transaction chain.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionRemove_MoreThanOne,
        /// <summary>
        /// Key value : Removed transaction chain must be last transaction chain.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ActivityChainTransactionRemove_InvalidId
    }
}

