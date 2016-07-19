namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateActivityPage : ProjectPageBase
    {
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activityStatusId");
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator vendor = new ElementLocator(Locator.CssSelector, "#activity-vendors-edit span.ng-binding");
        private readonly ElementLocator landlord = new ElementLocator(Locator.CssSelector, "#activity-landlords-edit span.ng-binding");
        // negotiators & departments locators
        private readonly ElementLocator negotiator = new ElementLocator(Locator.Id, "negotiator-");
        private readonly ElementLocator title = new ElementLocator(Locator.CssSelector, ".well > div:nth-of-type(1)");
        private readonly ElementLocator department = new ElementLocator(Locator.CssSelector, ".department-name");
        // property locators
        private readonly ElementLocator propertyNumber = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(1) >div:nth-of-type(1) span");
        private readonly ElementLocator propertyName = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(1) >div:nth-of-type(2) span");
        private readonly ElementLocator propertyLine2 = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(2) span");
        private readonly ElementLocator propertyPostCode = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(4) span");
        private readonly ElementLocator propertyCity = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(5) span");
        private readonly ElementLocator propertyCounty = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(6) span");
        // attendees locators
        private readonly ElementLocator attendees = new ElementLocator(Locator.CssSelector, "activity-attendees-edit-control label");
        private readonly ElementLocator attendeeToSelect = new ElementLocator(Locator.XPath, "//activity-attendees-edit-control//label[contains(.,'{0}')]");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.Id, "invitationTextId");
        // basic information locators
        private readonly ElementLocator source = new ElementLocator(Locator.Id, "sourceId");
        private readonly ElementLocator sourceDescription = new ElementLocator(Locator.Id, "sourceDescriptionId");
        private readonly ElementLocator sellingReason = new ElementLocator(Locator.Id, "sellingReasonId");
        private readonly ElementLocator pitchingThreats = new ElementLocator(Locator.Id, "pitchingThreatsId");
        private readonly ElementLocator disposalType = new ElementLocator(Locator.Id, "disposalTypeId");
        // additional information locators
        private readonly ElementLocator keyNumber = new ElementLocator(Locator.Id, "keyNumberId");
        private readonly ElementLocator accessArrangements = new ElementLocator(Locator.Id, "accessArrangementsId");
        // appraisal meeting locators
        private readonly ElementLocator date = new ElementLocator(Locator.Name, "meetingDate");
        private readonly ElementLocator startTime = new ElementLocator(Locator.CssSelector, "#meeting-start-time input");
        private readonly ElementLocator endTime = new ElementLocator(Locator.CssSelector, "#meeting-end-time input");
        // valuation information locators
        private readonly ElementLocator kfValuation = new ElementLocator(Locator.Id, "kfValuationPrice");
        private readonly ElementLocator vendorValuation = new ElementLocator(Locator.Id, "vendorValuationPrice");
        private readonly ElementLocator agreedinitialMarketingPrice = new ElementLocator(Locator.Id, "agreedInitialMarketingPrice");
        private readonly ElementLocator kfValuationShortLet = new ElementLocator(Locator.Id, "shortKfValuationPrice");
        private readonly ElementLocator kfValuationLongLet = new ElementLocator(Locator.Id, "longKfValuationPrice");
        private readonly ElementLocator vendorValuationShortLet = new ElementLocator(Locator.Id, "shortVendorValuationPrice");
        private readonly ElementLocator vendorValuationLongLet = new ElementLocator(Locator.Id, "longVendorValuationPrice");
        private readonly ElementLocator agreedInitialMarketingPriceShortLet = new ElementLocator(Locator.Id, "shortAgreedInitialMarketingPrice");
        private readonly ElementLocator agreedInitialMarketingPriceLongLet = new ElementLocator(Locator.Id, "longAgreedInitialMarketingPrice");
        private readonly ElementLocator serviceCharge = new ElementLocator(Locator.Id, "serviceChargeAmount");
        private readonly ElementLocator serviceChargeNote = new ElementLocator(Locator.Id, "serviceChargeNote");
        private readonly ElementLocator groundRent = new ElementLocator(Locator.Id, "groundRentAmount");
        private readonly ElementLocator groundRentNote = new ElementLocator(Locator.Id, "groundRentNote");
        //other locators
        private readonly ElementLocator decoration = new ElementLocator(Locator.Id, "decorationId");
        private readonly ElementLocator otherConditions = new ElementLocator(Locator.Id, "otherCondition");
        //Type: Open Market Letting,  Status: To Let Unavailable locators
        private readonly ElementLocator rent = new ElementLocator(Locator.Id, "rentPaymentPeriodId");
        private readonly ElementLocator askingRentShortWeekLet = new ElementLocator(Locator.Id, "shortAskingWeekRent");
        private readonly ElementLocator askingRentShortLetMatchFlexibility = new ElementLocator(Locator.Id, "shortMatchFlexibilityId");
        private readonly ElementLocator askingRentShortLetMatchFlexibilityWeekRentValue = new ElementLocator(Locator.Id, "shortMatchFlexWeekValue");
        private readonly ElementLocator askingRentShortLetMatchFlexibilityRentPercentage = new ElementLocator(Locator.Id, "shortMatchFlexPercentage");
        private readonly ElementLocator askingRentLongLet = new ElementLocator(Locator.Id, "longAskingWeekRent");
        private readonly ElementLocator askingRentLongLetMatchFlexibility = new ElementLocator(Locator.Id, "longMatchFlexibilityId");
        private readonly ElementLocator askingRentLongLetMatchFlexibilityRentWeekValue = new ElementLocator(Locator.Id, "longMatchFlexWeekValue");
        private readonly ElementLocator askingRentLongLetMatchFlexibilityRentPercentage = new ElementLocator(Locator.Id, "longMatchFlexPercentage");
        //Type: Freehold Sale & Long Leashold Sale, Status: For Sale Unavailable locators
        private readonly ElementLocator priceType = new ElementLocator(Locator.Id, "priceTypeId");
        private readonly ElementLocator price = new ElementLocator(Locator.Id, "activityPrice");
        private readonly ElementLocator matchFlexibility = new ElementLocator(Locator.Id, "matchFlexibilityId");
        private readonly ElementLocator matchFlexibilityValue = new ElementLocator(Locator.Id, "matchFlexValue");
        private readonly ElementLocator matchFlexibilityPercentage = new ElementLocator(Locator.Id, "matchFlexPercentage");

        public CreateActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string ActivityVendor => this.Driver.GetElement(this.vendor).Text;

        public string ActivityLandlord => this.Driver.GetElement(this.landlord).Text;

        public string ActivityNegotiator => this.Driver.GetElement(this.negotiator).Text;

        public string ActivityTitle => this.Driver.GetElement(this.title).Text;

        public string ActivityDepartment => this.Driver.GetElement(this.department).Text;

        public List<string> ActivityAttendees => this.Driver.GetElements(this.attendees).Select(x => x.Text).ToList();

        public ViewActivityPage SaveActivity()
        {
            this.Driver.Click(this.saveButton);
            return new ViewActivityPage(this.DriverContext);
        }

        public CreateActivityPage SelectActivityType(string activityType)
        {
            this.Driver.GetElement<Select>(this.type).SelectByText(activityType);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateActivityPage SelectActivityStatus(string activityStatus)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(activityStatus);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public Dictionary<string, string> GetPropertyAddress()
        {
            var propertyAddress = new Dictionary<string, string>
            {
                { "number", this.Driver.GetElement(this.propertyNumber).Text },
                { "name", this.Driver.GetElement(this.propertyName).Text },
                { "line2", this.Driver.GetElement(this.propertyLine2).Text },
                { "postCode", this.Driver.GetElement(this.propertyPostCode).Text },
                { "city", this.Driver.GetElement(this.propertyCity).Text },
                { "county", this.Driver.GetElement(this.propertyCounty).Text }
            };
            return propertyAddress;
        }

        public CreateActivityPage SelectSource(string sourceText)
        {
            this.Driver.GetElement<Select>(this.source).SelectByText(sourceText);
            return this;
        }

        public CreateActivityPage SetSourceDescription(string sourceDescriptionText)
        {
            this.Driver.SendKeys(this.sourceDescription, sourceDescriptionText);
            return this;
        }

        public CreateActivityPage SelectSellingReason(string reason)
        {
            this.Driver.GetElement<Select>(this.sellingReason).SelectByText(reason);
            return this;
        }

        public CreateActivityPage SetPitchingThreats(string threats)
        {
            this.Driver.SendKeys(this.pitchingThreats, threats);
            return this;
        }

        public CreateActivityPage SetKeyNumber(string key)
        {
            this.Driver.SendKeys(this.keyNumber, key);
            return this;
        }

        public CreateActivityPage SetAccessArangements(string arangements)
        {
            this.Driver.SendKeys(this.accessArrangements, arangements);
            return this;
        }

        public CreateActivityPage SetDate()
        {
            this.Driver.SendKeys(this.date, DateTime.Today.AddDays(1).ToString("dd-MM-yyyy"));
            return this;
        }

        public CreateActivityPage SetTime(string start, string end)
        {
            this.Driver.SendKeys(this.startTime, start);
            this.Driver.SendKeys(this.endTime, end);
            return this;
        }

        public CreateActivityPage SelectAttendee(string attendee)
        {
            this.Driver.GetElement(this.attendeeToSelect.Format(attendee)).Click();
            return this;
        }

        public CreateActivityPage SetInvitationText(string invitation)
        {
            this.Driver.SendKeys(this.invitationText, invitation);
            return this;
        }

        public CreateActivityPage SetKfValuation(string valuation)
        {
            this.Driver.SendKeys(this.kfValuation, valuation);
            return this;
        }

        public CreateActivityPage SetVendorValuation(string valuation)
        {
            this.Driver.SendKeys(this.vendorValuation, valuation);
            return this;
        }

        public CreateActivityPage SetAgreedInitialMarketingPrice(string text)
        {
            this.Driver.SendKeys(this.agreedinitialMarketingPrice, text);
            return this;
        }

        public CreateActivityPage SetKfValuationShortLet(string valuation)
        {
            this.Driver.SendKeys(this.kfValuationShortLet, valuation);
            return this;
        }

        public CreateActivityPage SetKfValuationLongLet(string valuation)
        {
            this.Driver.SendKeys(this.kfValuationLongLet, valuation);
            return this;
        }

        public CreateActivityPage SetVendorValuationShortLet(string valuation)
        {
            this.Driver.SendKeys(this.vendorValuationShortLet, valuation);
            return this;
        }

        public CreateActivityPage SetVendorValuationLongLet(string valuation)
        {
            this.Driver.SendKeys(this.vendorValuationLongLet, valuation);
            return this;
        }

        public CreateActivityPage SetAgreedInitialMarketingPriceShortLet(string text)
        {
            this.Driver.SendKeys(this.agreedInitialMarketingPriceShortLet, text);
            return this;
        }

        public CreateActivityPage SetAgreedInitialMarketingPriceLongLet(string text)
        {
            this.Driver.SendKeys(this.agreedInitialMarketingPriceLongLet, text);
            return this;
        }

        public CreateActivityPage SetServiceCharge(string charge)
        {
            this.Driver.SendKeys(this.serviceCharge, charge);
            return this;
        }

        public CreateActivityPage SetServiceChargeNote(string charge)
        {
            this.Driver.SendKeys(this.serviceChargeNote, charge);
            return this;
        }

        public CreateActivityPage SetGroundRent(string text)
        {
            this.Driver.SendKeys(this.groundRent, text);
            return this;
        }

        public CreateActivityPage SetGroundRentNote(string note)
        {
            this.Driver.SendKeys(this.groundRentNote, note);
            return this;
        }

        public CreateActivityPage SelectDecoration(string element)
        {
            this.Driver.GetElement<Select>(this.decoration).SelectByText(element);
            return this;
        }

        public CreateActivityPage SetOtherConditions(string conditions)
        {
            this.Driver.SendKeys(this.otherConditions, conditions);
            return this;
        }

        public CreateActivityPage SelectDisposalType(string disposal)
        {
            this.Driver.GetElement<Select>(this.disposalType).SelectByText(disposal);
            return this;
        }

        public CreateActivityPage SelectPriceType(string selectPrice)
        {
            this.Driver.GetElement<Select>(this.priceType).SelectByText(selectPrice);
            return this;
        }

        public CreateActivityPage SetPrice(string value)
        {
            this.Driver.SendKeys(this.price, value);
            return this;
        }

        public CreateActivityPage SelectMatchFlexibility(string text)
        {
            this.Driver.GetElement<Select>(this.matchFlexibility).SelectByText(text);
            return this;
        }

        public CreateActivityPage SetMatchFlexibilityValue(string type, string value)
        {
            this.Driver.SendKeys(type == "Minimum price" ? this.matchFlexibilityValue : this.matchFlexibilityPercentage, value);
            return this;
        }

        public CreateActivityPage SelectRent(string rentType)
        {
            this.Driver.GetElement<Select>(this.rent).SelectByText(rentType);
            return this;
        }

        public CreateActivityPage SetAskingRentShortLet(string shortLet)
        {
            this.Driver.SendKeys(this.askingRentShortWeekLet, shortLet);
            return this;
        }

        public CreateActivityPage SelectShortLetMatchFlexibility(string flexibility)
        {
            this.Driver.GetElement<Select>(this.askingRentShortLetMatchFlexibility).SelectByText(flexibility);
            return this;
        }

        public CreateActivityPage SetShortLetMatchFlexibilityRentValue(string value)
        {
            this.Driver.SendKeys(this.askingRentShortLetMatchFlexibilityWeekRentValue, value);
            return this;
        }

        public CreateActivityPage SetAskingRentLongLet(string shortLet)
        {
            this.Driver.SendKeys(this.askingRentLongLet, shortLet);
            return this;
        }

        public CreateActivityPage SelectLongLetMatchFlexibility(string flexibility)
        {
            this.Driver.GetElement<Select>(this.askingRentLongLetMatchFlexibility).SelectByText(flexibility);
            return this;
        }

        public CreateActivityPage SetLongLetMatchFlexibilityRentPercentage(string value)
        {
            this.Driver.SendKeys(this.askingRentLongLetMatchFlexibilityRentPercentage, value);
            return this;
        }

    }

    internal class ActivityDetails
    {
        public string Vendor { get; set; }

        public string Landlord { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Negotiator { get; set; }

        public string CreationDate { get; set; }

        public string ActivityTitle { get; set; }

        public string Department { get; set; }

        public string Attendees { get; set; }

        public string Source { get; set; }

        public string SourceDescription { get; set; }

        public string SellingReason { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArrangements { get; set; }

        public string InvitationText { get; set; }

        public string Decoration { get; set; }

        public string OtherConditions { get; set; }

        public string DisposalType { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Landlord { get; set; }
    }

    internal class ValuationInformation
    {
        public string KfValuation { get; set; }

        public string VendorValuation { get; set; }

        public string AgreedInitialMarketingPrice { get; set; }

        public string KfValuationShortLet { get; set; }

        public string KfValuationLongLet { get; set; }

        public string VendorValuationShortLet { get; set; }

        public string VendorValuationLongLet { get; set; }

        public string AgreedInitialMarketingPriceShortLet { get; set; }

        public string AgreedInitialMarketingPriceLongLet { get; set; }

        public string ServiceCharge { get; set; }

        public string ServiceChargeNote { get; set; }

        public string GroundRent { get; set; }

        public string GroundRentNote { get; set; }

        public string DisposalType { get; set; }
    }

    internal class Prices
    {
        public string PriceType { get; set; }

        public string Price { get; set; }

        public string MatchFlexibility { get; set; }

        public string MatchFlexibilityValue { get; set; }

        public string PriceFlexibility { get; set; }
    }

    internal class LettingRent
    {
        public string Rent { get; set; }

        public string RentShortLet { get; set; }

        public string RentShortLetMatchFlexibility { get; set; }

        public string RentShortLetMatchFlexibilityRentValue { get; set; }

        public string RentShortLetMatchFlexibilityRentPercentage { get; set; }

        public string RentLongLet { get; set; }

        public string RentLongLetMatchFlexibility { get; set; }

        public string RentLongLetMatchFlexibilityRentValue { get; set; }

        public string RentLongLetMatchFlexibilityRentPercentage { get; set; }

        public string RentShortLetMonth { get; set; }

        public string RentShortLetWeek { get; set; }

        public string RentShortMatchFlexibilityMonth { get; set; }

        public string RentShortMatchFlexibilityWeek { get; set; }

        public string RentLongLetMonth { get; set; }

        public string RentLongtLetWeek { get; set; }

        public string RentLongMatchFlexibilityPercentage { get; set; }

        public string RentLongMatchFlexibilityMonth { get; set; }

        public string RentLongMatchFlexibilityWeek { get; set; }

        public string RentShortMatchFlexibilityPercentage { get; set; }
    }
}
