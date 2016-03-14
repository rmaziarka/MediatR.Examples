namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;

    public class CreatePropertyPage : ProjectPageBase
    {
        public CreatePropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public AddressTemplatePage AddressTemplate => new AddressTemplatePage(this.DriverContext);
    }
}
