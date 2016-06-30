namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class CompanySteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public CompanySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Company is created in database")]
        [When(@"Company is created in database")]
        public void CreateCompanyInDb(Table table)
        {
            var company = table.CreateInstance<Company>();
            List<Guid> contactsIds = this.scenarioContext.Get<List<Contact>>("ContactsList").Select(c => c.Id).ToList();

            company.CompaniesContacts = contactsIds.Select(id => new CompanyContact { ContactId = id }).ToList();

            this.dataContext.Companies.Add(company);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(company, "Company");
        }
    }
}
