namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    public enum ErrorMessage
    {
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
        /// Key value : One or more vendors are not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Activity_Vendors_Id,
        /// <summary>
        /// Key value : One or more contacts are not exist.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Company_Contacts_Id,
        /// <summary>
        /// Key value : One or more attendees are not on the applicant list.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Requirement_Applicants_Id,
        /// <summary>
        /// Key value : One or more attendees are not on the applicant list.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Requirement_Attendees_Id,
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
        Property_Should_Not_Be_Empty
    }
}

