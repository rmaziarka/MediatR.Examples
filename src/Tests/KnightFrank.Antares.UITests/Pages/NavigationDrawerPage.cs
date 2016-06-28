namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using OpenQA.Selenium;

    public class NavigationDrawerPage : ProjectPageBase
    {
        private readonly ElementLocator createLink = new ElementLocator(Locator.CssSelector, "div.panel-open a[href *= 'add']");
        private readonly ElementLocator hamburgerBox = new ElementLocator(Locator.CssSelector, "span.hamburger-box");
        private readonly ElementLocator menuItem = new ElementLocator(Locator.XPath, "//nav[@class='drawer']//span[normalize-space(text()) = '{0}']{1}");
        private readonly ElementLocator openedDrawer = new ElementLocator(Locator.CssSelector, "div.drawer-open");
        private readonly ElementLocator subMenuItem = new ElementLocator(Locator.XPath, "//ancestor::div[contains(@class, 'panel-open')]//div[@class = 'panel-body']");
        private readonly ElementLocator latestEntities = new ElementLocator(Locator.CssSelector, "navigation-drawer[type = '{0}'] li a");
        private readonly ElementLocator latestEntity = new ElementLocator(Locator.CssSelector, "navigation-drawer[type = '{0}'] li:nth-of-type({1}) a");
        private readonly ElementLocator searchLink = new ElementLocator(Locator.CssSelector, "div.panel-open a[href *= 'search']");

        public NavigationDrawerPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NavigationDrawerPage OpenNavigationDrawer()
        {
            if (!this.Driver.IsElementPresent(this.openedDrawer, BaseConfiguration.ShortTimeout))
            {
                this.Driver.Click(this.hamburgerBox);
            }
            return this;
        }

        public NavigationDrawerPage CloseNavigationDrawer()
        {
            if (this.Driver.IsElementPresent(this.openedDrawer, BaseConfiguration.ShortTimeout))
            {
                this.Driver.Click(this.hamburgerBox);
            }
            return this;
        }

        public bool IsOpenedDrawerMenuPresent()
        {
            var isPresent = false;
            Array values = Enum.GetValues(typeof(DrawerMenu));
            foreach (object value in values)
            {
                isPresent = this.Driver.IsElementPresent(this.menuItem.Format(value, string.Empty), BaseConfiguration.ShortTimeout);
                if (!isPresent)
                {
                    return false;
                }
            }
            return isPresent;
        }

        public void ClickDrawerMenuItem(string drawerMenuItem)
        {
            if (!this.Driver.IsElementPresent(this.menuItem.Format(drawerMenuItem, this.subMenuItem.Value),
                BaseConfiguration.ShortTimeout))
            {
                this.Driver.Click(this.menuItem.Format(drawerMenuItem, string.Empty));
            }
        }

        public bool IsSubMenuVisible(string drawerMenuItem)
        {
            return this.Driver.IsElementPresent(this.menuItem.Format(drawerMenuItem, this.subMenuItem.Value),
                BaseConfiguration.ShortTimeout);
        }

        public NavigationDrawerPage ClickCreateButton()
        {
            this.Driver.Click(this.createLink);
            return this;
        }

        public List<string> GetLatestEntities(string entity)
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.GetElements(this.latestEntities.Format(entity)).Select(e => e.Text).ToList();
        }

        public NavigationDrawerPage ClickLatestEntity(string entity, string position)
        {
            this.Driver.WaitForAngularToFinish();
            this.Driver.Click(this.latestEntity.Format(entity, position));
            return this;
        }

        public NavigationDrawerPage ClickSearchLink()
        {
            this.Driver.Click(this.searchLink);
            return this;
        }

        // ReSharper disable UnusedMember.Local
        private enum DrawerMenu
        {
            Contacts,
            Companies,
            Properties,
            Requirements
        }

        internal class LatestViews
        {
            public string LatestData { get; set; }
        }
    }
}
