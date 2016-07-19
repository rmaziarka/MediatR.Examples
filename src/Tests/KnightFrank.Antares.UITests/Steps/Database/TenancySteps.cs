namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Tenancy;

    using TechTalk.SpecFlow;

    using TenancyType = KnightFrank.Antares.Domain.Common.Enums.TenancyType;

    [Binding]
    public class TenancySteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public TenancySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Tenancy is created in database")]
        public void CreateTenancy()
        {
            var activity = this.scenarioContext.Get<Activity>("Activity");
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");

            var tenancy = new Tenancy
            {
                ActivityId = activity.Id,
                RequirementId = requirement.Id,
                TenancyTypeId =
                    this.dataContext.TenancyTypes.Single(t => t.EnumCode.Equals(nameof(TenancyType.ResidentialLetting))).Id,
                Contacts = new List<TenancyContact>(),
                Terms = new List<TenancyTerm>
                {
                    new TenancyTerm
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddYears(1),
                        AgreedRent = 1000000
                    }
                }
            };

            this.dataContext.Tenancies.Add(tenancy);
            this.dataContext.SaveChanges();
            
            this.scenarioContext.Set(tenancy, "Tenancy");

            requirement.TenancyId = tenancy.Id;
            this.dataContext.SaveChanges();
        }
    }
}
