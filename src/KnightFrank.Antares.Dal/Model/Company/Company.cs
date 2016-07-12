namespace KnightFrank.Antares.Dal.Model.Company
{
    using System;
    using System.Collections.Generic;

    using Enum;
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string ClientCarePageUrl { get; set; }
        public string Description { get; set; }
        public bool? Valid { get; set; }
        public Guid? CompanyCategoryId { get; set; }
        public CompanyCategory CompanyCategory { get; set; }
        public Guid? CompanyTypeId { get; set; }
        public EnumTypeItem CompanyType { get; set; }
        public virtual ICollection<CompanyContact> CompaniesContacts { get; set; }
        public Guid? ClientCareStatusId { get; set; }
        public EnumTypeItem ClientCareStatus { get; set; }
    }
}