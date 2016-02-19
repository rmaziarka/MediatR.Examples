/*
The MIT License (MIT)

Copyright (c) 2015 Objectivity Bespoke Software Specialists

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using Objectivity.Test.Automation.Common;
using Objectivity.Test.Automation.Common.Logger;
using TechTalk.SpecFlow;
using Xunit;

namespace KnightFrank.Antares.UITests
{
    /// <summary>
    /// The base class for all tests
    /// </summary>
    [Binding]
    public class ProjectTestBase : TestBase
    {
        private readonly ScenarioContext scenarioContext;
        private readonly DriverContext driverContext = new DriverContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectTestBase"/> class.
        /// </summary>
        public ProjectTestBase(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null) throw new ArgumentNullException(nameof(scenarioContext));
            this.scenarioContext = scenarioContext;
        }

        /// <summary>
        /// Logger instance for driver
        /// </summary>
        public TestLogger LogTest
        {
            get
            {
                return DriverContext.LogTest;
            }

            set
            {
                DriverContext.LogTest = value;
            }
        }

        /// <summary>
        /// The browser manager
        /// </summary>
        protected DriverContext DriverContext => driverContext;

        /// <summary>
        /// Before the class.
        /// </summary>
        [BeforeFeature]
        public static void BeforeClass()
        {
            StartPerformanceMeasure();
        }

        /// <summary>
        /// After the class.
        /// </summary>
        [AfterFeature]
        public static void AfterClass()
        {
            StopPerfromanceMeasure();
        }

        /// <summary>
        /// Before the test.
        /// </summary>
        [Before]
        public void BeforeTest()
        {
            DriverContext.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DriverContext.TestTitle = scenarioContext.ScenarioInfo.Title;
            LogTest.LogTestStarting(driverContext);
            DriverContext.Start();
            scenarioContext["DriverContext"] = DriverContext;
        }

        /// <summary>
        /// After the test.
        /// </summary>
        [After]
        public void AfterTest()
        {
            DriverContext.IsTestFailed = scenarioContext.TestError != null;
            SaveTestDetailsIfTestFailed(driverContext);
            DriverContext.Stop();
            LogTest.LogTestEnding(driverContext);
            if (IsVerifyFailedAndClearMessages(driverContext))
            {
                Assert.True(false);
            }
        }
    }
}
