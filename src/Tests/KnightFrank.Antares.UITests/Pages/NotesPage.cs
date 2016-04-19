namespace KnightFrank.Antares.UITests.Pages
{
    using System;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class NotesPage : ProjectPageBase
    {
        private readonly ElementLocator listOfNotes = new ElementLocator(Locator.CssSelector, ".note-content.ng-binding");
        private readonly ElementLocator noteTextArea = new ElementLocator(Locator.CssSelector, ".form-control");
        private readonly ElementLocator saveNoteButton = new ElementLocator(Locator.CssSelector, ".btn.btn-primary");

        public NotesPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NotesPage SetNoteText(string noteText)
        {
            this.Driver.SendKeys(this.noteTextArea, noteText);
            return this;
        }

        public NotesPage SaveNote()
        {
            this.Driver.GetElement(this.saveNoteButton).Click();
            this.Driver.WaitForAngular();
            return this;
        }

        public int GetNumberOfNotes()
        {
            try
            {
                //TODO check timeout settings for getelements when there are no elements
                return this.Driver.GetElements(this.listOfNotes).Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
