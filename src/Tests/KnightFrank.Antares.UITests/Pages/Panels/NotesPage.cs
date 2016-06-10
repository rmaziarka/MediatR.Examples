namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class NotesPage : ProjectPageBase
    {
        private readonly ElementLocator listOfNotes = new ElementLocator(Locator.CssSelector, ".note-content");
        private readonly ElementLocator noteTextArea = new ElementLocator(Locator.Id, "note-description");
        private readonly ElementLocator saveNoteButton = new ElementLocator(Locator.Id, "note-add-button");

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
            this.Driver.Click(this.saveNoteButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public int GetNumberOfNotes()
        {
            try
            {
                return this.Driver.GetElements(this.listOfNotes).Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
