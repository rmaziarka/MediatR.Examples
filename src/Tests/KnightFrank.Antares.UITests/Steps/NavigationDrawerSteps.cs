namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class NavigationDrawerSteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private NavigationDrawerPage page;

        public NavigationDrawerSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new NavigationDrawerPage(this.driverContext);
            }
        }

        [When(@"User opens navigation drawer menu")]
        public void OpenNavigationDrawerMenu()
        {
            this.page = new NavigationDrawerPage(this.driverContext).OpenNavigationDrawer();
        }

        [When(@"User selects (.*) menu item")]
        public void SelectMenuItem(string drawerMenuItem)
        {
            this.page.ClickDrawerMenuItem(drawerMenuItem);
        }

        [When(@"User clicks create button in drawer submenu")]
        public void ClickCreateButton()
        {
            this.page.ClickCreateButton();
        }

        [When(@"User closes navigation drawer menu")]
        public void CloseNavigationDrawerMenu()
        {
            this.page.CloseNavigationDrawer();
        }

        [Then(@"Drawer menu should be displayed")]
        public void CheckIfDrawerMenuPresent()
        {
            Assert.True(this.page.IsOpenedDrawerMenuPresent());
        }

        [Then(@"Drawer (.*) submenu should be displayed")]
        public void CheckIfDrawerSubMenuPresent(string drawerMenu)
        {
            Assert.True(this.page.IsSubMenuVisible(drawerMenu));
        }
    }
}
