namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    public enum ErrorMessage
    {
        /// <summary>
        /// Key value : {0} with given id '{1}' does not exist in the database
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Entity_Not_Exists,
        /// <summary>
        /// Key value : One or more attendees are not on the applicant list
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Missing_Applicant_Id
    }
}

