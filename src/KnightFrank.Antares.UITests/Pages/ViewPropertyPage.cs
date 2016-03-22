namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewPropertyPage : ProjectPageBase
    {
        private readonly ElementLocator country = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyNumber = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyName = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator addressLine2 = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator postCode = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator city = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator county = new ElementLocator(Locator.Id, string.Empty);

        public ViewPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string GetCountry()
        {
            return this.Driver.GetElement(this.country).Text;
        }

        public string GetPropertyNumber()
        {
            return this.Driver.GetElement(this.propertyNumber).Text;
        }

        public string GetPropertyName()
        {
            return this.Driver.GetElement(this.propertyName).Text;
        }

        public string GetAddressLine2()
        {
            return this.Driver.GetElement(this.addressLine2).Text;
        }

        public string GetPostCode()
        {
            return this.Driver.GetElement(this.postCode).Text;
        }

        public string GetCity()
        {
            return this.Driver.GetElement(this.city).Text;
        }

        public string GetCounty()
        {
            return this.Driver.GetElement(this.county).Text;
        }
    }
}
