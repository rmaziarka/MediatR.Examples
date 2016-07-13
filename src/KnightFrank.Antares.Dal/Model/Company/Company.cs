namespace KnightFrank.Antares.Dal.Model.Company
{
    using System;
    using System.Collections.Generic;

    using Enum;

    using KnightFrank.Antares.Dal.Model.User;

    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string ClientCarePageUrl { get; set; }
        public string Description { get; set; }
        public bool? Valid { get; set; }
        public Guid? CompanyCategoryId { get; set; }
        public EnumTypeItem CompanyCategory { get; set; }
        public Guid? CompanyTypeId { get; set; }
        public EnumTypeItem CompanyType { get; set; }
        public virtual ICollection<CompanyContact> CompaniesContacts { get; set; }
        public Guid? ClientCareStatusId { get; set; }
        public EnumTypeItem ClientCareStatus { get; set; }
        public Guid? RelationshipMangerId { get; set; }
        public virtual User RelationshipManger { get; set; }
    }
}