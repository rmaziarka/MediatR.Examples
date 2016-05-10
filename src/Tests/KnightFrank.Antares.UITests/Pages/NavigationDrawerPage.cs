namespace KnightFrank.Antares.UITests.Pages
{
    using System;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class NavigationDrawerPage : ProjectPageBase
    {
        private readonly ElementLocator createButton = new ElementLocator(Locator.CssSelector, "div.panel-open a.btn");
        private readonly ElementLocator hamburgerBox = new ElementLocator(Locator.CssSelector, "span.hamburger-box");
        private readonly ElementLocator menuItem = new ElementLocator(Locator.XPath, "//nav[@class='drawer']{0}//span[normalize-space(text()) = '{1}']");
        private readonly ElementLocator openedDrawer = new ElementLocator(Locator.CssSelector, "div.drawer-open");
        private readonly ElementLocator subMenuItem = new ElementLocator(Locator.CssSelector, "div.panel-open");

        public NavigationDrawerPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NavigationDrawerPage OpenNavigationDrawer()
        {
            if (!this.Driver.IsElementPresent(this.openedDrawer, BaseConfiguration.ShortTimeout))
            {
                this.Driver.GetElement(this.hamburgerBox).Click();
            }
            return this;
        }

        public NavigationDrawerPage CloseNavigationDrawer()
        {
            if (this.Driver.IsElementPresent(this.openedDrawer, BaseConfiguration.ShortTimeout))
            {
                this.Driver.GetElement(this.hamburgerBox).Click();
            }
            return this;
        }

        public bool IsOpenedDrawerMenuPresent()
        {
            var isPresent = false;
            Array values = Enum.GetValues(typeof(DrawerMenu));
            foreach (object value in values)
            {
                isPresent = this.Driver.IsElementPresent(this.menuItem.Format(string.Empty, value), BaseConfiguration.ShortTimeout);
                if (!isPresent)
                {
                    return false;
                }
            }
            return isPresent;
        }

        public void ClickDrawerMenuItem(string drawerMenuItem)
        {
            this.Driver.GetElement(this.menuItem.Format(string.Empty, drawerMenuItem)).Click();
        }

        public bool IsSubMenuVisible(string drawerMenuItem)
        {
            return this.Driver.IsElementPresent(this.menuItem.Format(this.subMenuItem.Value, drawerMenuItem),
                BaseConfiguration.ShortTimeout);
        }

        public NavigationDrawerPage ClickCreateButton()
        {
            this.Driver.GetElement(this.createButton).Click();
            return this;
        }

        private enum DrawerMenu
        {
            Contacts,
            Companies,
            Properties,
            Requirements
        }
    }
}
