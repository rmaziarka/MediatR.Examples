namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;

    using TechTalk.SpecFlow;

    [Binding]
    public class ActivitySteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;
        private readonly DateTime date = DateTime.UtcNow;

        public ActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Open market letting activity with (.*) status is defined")]
        public void CreateOpenMarketLettingActivity(string status)
        {
            Guid activityTypeId = this.dataContext.ActivityTypes.Single(i => i.EnumCode.Equals("OpenMarketLetting")).Id;
            Guid activityStatusId =
                this.dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(ActivityStatus)) && i.Code.Equals(nameof(ActivityStatus.ToLetUnavailable))).Id;
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                Contacts =
                    this.scenarioContext.ContainsKey("ContactsList")
                        ? this.scenarioContext.Get<List<Contact>>("ContactsList")
                        : new List<Contact>(),
                ActivityUsers = new List<ActivityUser>
                {
                    new ActivityUser
                    {
                        //TODO improve selecting lead negotiator
                        UserId = this.dataContext.Users.First().Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.LeadNegotiator))).Id,
                        CallDate = this.date.AddDays(14)
                    }
                },
                RentPaymentPeriodId =
                    this.dataContext.EnumTypeItems.Single(
                        e => e.EnumType.Code.Equals(nameof(RentPaymentPeriod)) && e.Code.Equals(nameof(RentPaymentPeriod.Weekly)))
                        .Id,
                ShortAskingWeekRent = 1000,
                ShortAskingMonthRent = 4334,
                ShortMatchFlexibilityId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ActivityMatchFlexRent)) &&
                            e.Code.Equals(nameof(ActivityMatchFlexRent.MinimumRent))).Id,
                ShortMatchFlexWeekValue = 900,
                ShortMatchFlexMonthValue = 3900,
                LongAskingWeekRent = 2000,
                LongAskingMonthRent = 8667,
                LongMatchFlexibilityId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ActivityMatchFlexRent)) &&
                            e.Code.Equals(nameof(ActivityMatchFlexRent.Percentage))).Id,
                LongMatchFlexPercentage = 2,
                ActivityDepartments = new List<ActivityDepartment>
                {
                    new ActivityDepartment
                    {
                        DepartmentId = this.dataContext.Users.First().DepartmentId,
                        DepartmentTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                    e.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                    }
                },
                SourceId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySource)) &&
                    e.Code.Equals(nameof(ActivitySource.KnightfrankGlobalSearch))).Id,
                AppraisalMeetingAttendees =
                    new List<ActivityAttendee>
                    {
                        new ActivityAttendee { ContactId = this.scenarioContext.Get<List<Contact>>("ContactsList").First().Id }
                    },
                AppraisalMeetingStart = this.date,
                AppraisalMeetingEnd = this.date.AddHours(1)
            };

            this.dataContext.Activities.Add(activity);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [Given(@"Long leasehold Sale activity with (.*) status is defined")]
        public void CreateSaleActivity(string status)
        {
            Guid activityTypeId = this.dataContext.ActivityTypes.Single(i => i.EnumCode.Equals("LongLeaseholdSale")).Id;
            Guid activityStatusId =
                this.dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(ActivityStatus)) && i.Code.Equals(nameof(ActivityStatus.ForSaleUnavailable))).Id;
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                Contacts =
                    this.scenarioContext.ContainsKey("ContactsList")
                        ? this.scenarioContext.Get<List<Contact>>("ContactsList")
                        : new List<Contact>(),
                ActivityUsers = new List<ActivityUser>
                {
                    new ActivityUser
                    {
                        //TODO improve selecting lead negotiator
                        UserId = this.dataContext.Users.First().Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.LeadNegotiator))).Id,
                        CallDate = this.date.AddDays(14)
                    }
                },
                ActivityDepartments = new List<ActivityDepartment>
                {
                    new ActivityDepartment
                    {
                        DepartmentId = this.dataContext.Users.First().DepartmentId,
                        DepartmentTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                    e.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                    }
                },
                PriceTypeId =
                    this.dataContext.EnumTypeItems.Single(
                        e =>
                            e.EnumType.Code.Equals(nameof(ActivityPriceType)) &&
                            e.Code.Equals(nameof(ActivityPriceType.AskingPrice))).Id,
                ActivityPrice = 2000000,
                MatchFlexibilityId =
                    this.dataContext.EnumTypeItems.Single(e => e.EnumType.Code.Equals(nameof(ActivityMatchFlexPrice)) &&
                                                               e.Code.Equals(nameof(ActivityMatchFlexPrice.Percentage))).Id,
                MatchFlexPercentage = 2,
                SourceId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySource)) &&
                    e.Code.Equals(nameof(ActivitySource.KnightfrankGlobalSearch))).Id,
                SellingReasonId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySellingReason)) &&
                    e.Code.Equals(nameof(ActivitySellingReason.Divorce))).Id,
                AppraisalMeetingAttendees =
                    new List<ActivityAttendee>
                    {
                        new ActivityAttendee { ContactId = this.scenarioContext.Get<List<Contact>>("ContactsList").First().Id }
                    },
                AppraisalMeetingStart = this.date,
                AppraisalMeetingEnd = this.date.AddHours(1)
            };

            this.dataContext.Activities.Add(activity);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [Given(@"Property (.*) activity is defined")]
        [When(@"Property (.*) activity is defined")]
        public void CreateActivity(string activityType)
        {
            Guid activityTypeId = this.dataContext.ActivityTypes.Single(i => i.Code.Equals(activityType)).Id;
            Guid activityStatusId =
                this.dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(ActivityStatus)) && i.Code.Equals(nameof(ActivityStatus.PreAppraisal))).Id;
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                Contacts =
                    this.scenarioContext.ContainsKey("ContactsList")
                        ? this.scenarioContext.Get<List<Contact>>("ContactsList")
                        : new List<Contact>(),
                ActivityUsers = new List<ActivityUser>
                {
                    new ActivityUser
                    {
                        //TODO improve selecting lead negotiator
                        UserId = this.dataContext.Users.First().Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.LeadNegotiator))).Id,
                        CallDate = this.date.AddDays(14)
                    }
                },
                ActivityDepartments = new List<ActivityDepartment>
                {
                    new ActivityDepartment
                    {
                        DepartmentId = this.dataContext.Users.First().DepartmentId,
                        DepartmentTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                    e.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                    }
                },
                SourceId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySource)) &&
                    e.Code.Equals(nameof(ActivitySource.KnightfrankGlobalSearch))).Id,
                SellingReasonId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySellingReason)) &&
                    e.Code.Equals(nameof(ActivitySellingReason.Divorce))).Id,
                AppraisalMeetingAttendees =
                    new List<ActivityAttendee>
                    {
                        new ActivityAttendee { ContactId = this.scenarioContext.Get<List<Contact>>("ContactsList").First().Id }
                    },
                AppraisalMeetingStart = this.date,
                AppraisalMeetingEnd = this.date.AddHours(1)
            };

            this.dataContext.Activities.Add(activity);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }

        [Given(@"Property (.*) activity with negotiators is defined")]
        public void CreateActivityWithNegotiators(string activityType)
        {
            Guid activityTypeId = this.dataContext.ActivityTypes.Single(i => i.Code.Equals(activityType)).Id;
            Guid activityStatusId =
                this.dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(ActivityStatus)) && i.Code.Equals(nameof(ActivityStatus.PreAppraisal))).Id;
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;

            var activity = new Activity
            {
                PropertyId = propertyId,
                ActivityTypeId = activityTypeId,
                ActivityStatusId = activityStatusId,
                CreatedDate = this.date,
                LastModifiedDate = this.date,
                Contacts = this.scenarioContext.Get<List<Contact>>("ContactsList"),
                ActivityUsers = new List<ActivityUser>
                {
                    new ActivityUser
                    {
                        //TODO improve selecting lead negotiator
                        UserId = this.dataContext.Users.First().Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.LeadNegotiator))).Id,
                        CallDate = this.date.AddDays(14)
                    },
                    new ActivityUser
                    {
                        UserId = this.dataContext.Users.First(u => u.FirstName.Equals("Eva") && u.LastName.Equals("Sandler")).Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.SecondaryNegotiator))).Id
                    },
                    new ActivityUser
                    {
                        UserId = this.dataContext.Users.First(u => u.FirstName.Equals("John") && u.LastName.Equals("Doe")).Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.SecondaryNegotiator))).Id
                    },
                    new ActivityUser
                    {
                        UserId =
                            this.dataContext.Users.First(u => u.FirstName.Equals("Martha") && u.LastName.Equals("Williams")).Id,
                        UserTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(UserType)) &&
                                    e.Code.Equals(nameof(UserType.SecondaryNegotiator))).Id
                    }
                },
                ActivityDepartments = new List<ActivityDepartment>
                {
                    new ActivityDepartment
                    {
                        DepartmentId = this.dataContext.Users.First().DepartmentId,
                        DepartmentTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                    e.Code.Equals(nameof(ActivityDepartmentType.Managing))).Id
                    },
                    new ActivityDepartment
                    {
                        DepartmentId =
                            this.dataContext.Users.First(u => u.FirstName.Equals("Eva") && u.LastName.Equals("Sandler"))
                                .DepartmentId,
                        DepartmentTypeId =
                            this.dataContext.EnumTypeItems.Single(
                                e =>
                                    e.EnumType.Code.Equals(nameof(ActivityDepartmentType)) &&
                                    e.Code.Equals(nameof(ActivityDepartmentType.Standard))).Id
                    }
                },
                SourceId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySource)) &&
                    e.Code.Equals(nameof(ActivitySource.KnightfrankGlobalSearch))).Id,
                SellingReasonId = this.dataContext.EnumTypeItems.Single(e =>
                    e.EnumType.Code.Equals(nameof(ActivitySellingReason)) &&
                    e.Code.Equals(nameof(ActivitySellingReason.Divorce))).Id,
                AppraisalMeetingAttendees =
                    new List<ActivityAttendee>
                    {
                        new ActivityAttendee { ContactId = this.scenarioContext.Get<List<Contact>>("ContactsList").First().Id }
                    },
                AppraisalMeetingStart = this.date,
                AppraisalMeetingEnd = this.date.AddHours(1)
            };

            this.dataContext.Activities.Add(activity);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(activity, "Activity");
        }
    }
}
