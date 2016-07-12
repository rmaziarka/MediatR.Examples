/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Activity implements Dto.IActivity {
        id: string = '';
        propertyId: string = '';
        activityStatusId: string = '';
        activityTypeId: string = '';
        activityType: Dto.IActivityType = null;
        contacts: Contact[] = [];
        attachments: Attachment[] = [];
        property: PreviewProperty = null;
        createdDate: Date = null;
        viewingsByDay: ViewingGroup[];
        viewings: Viewing[];
        leadNegotiator: ActivityUser = null;
        secondaryNegotiator: ActivityUser[] = [];
        activityUsers: ActivityUser[] = [];
        activityDepartments: ActivityDepartment[] = [];
        offers: Offer[];
        askingPrice: number = null;
        shortLetPricePerWeek: number = null;
        solicitor: Contact = null;
        solicitorCompany: Company = null;
        solicitorCompanyContact: CompanyContactRelation = null;
        sourceId: string = null;
        sellingReasonId: string = null;
        appraisalMeetingStart: string = null;
        appraisalMeetingEnd: string = null;
        appraisalMeetingInvitationText: string = null;
        keyNumber: string = null;
        accessArrangements: string = null;
        appraisalMeetingAttendees: Dto.IActivityAttendee[];
        kfValuationPrice: number = null;
        agreedInitialMarketingPrice: number = null;
        vendorValuationPrice: number = null;
        shortKfValuationPrice: number;
        shortVendorValuationPrice: number;
        shortAgreedInitialMarketingPrice: number;
        longKfValuationPrice: number;
        longVendorValuationPrice: number;
        longAgreedInitialMarketingPrice: number;  
        disposalTypeId: string = '';
        decorationId: string = '';
        serviceChargeAmount: number = null;
        serviceChargeNote: string = '';
        groundRentAmount: number = null;
        groundRentNote: string = '';
        otherCondition: string = '';
        chainTransactions: Business.ChainTransaction[] = [];
         
        constructor(activity?: Dto.IActivity) {
            if (activity) {
                angular.extend(this, activity);
                this.createdDate = Core.DateTimeUtils.convertDateToUtc(activity.createdDate);
                if (activity.contacts) {
                    this.contacts = activity.contacts.map((contact: Dto.IContact) => { return new Contact(contact) });
                }
                this.property = new PreviewProperty(activity.property);

                var activityleadNegotiator = _.find(activity.activityUsers,
                    (user: Dto.IActivityUser) => user.userType.code === Enums.NegotiatorTypeEnum[Enums.NegotiatorTypeEnum.LeadNegotiator]);
                this.leadNegotiator = new ActivityUser(activityleadNegotiator);

                this.secondaryNegotiator = _.filter(activity.activityUsers,
                    (user: Dto.IActivityUser) => user.userType.code === Enums.NegotiatorTypeEnum[Enums.NegotiatorTypeEnum.SecondaryNegotiator])
                    .map((user: Dto.IActivityUser) => new ActivityUser(user));

                if (activity.activityDepartments) {
                    this.activityDepartments = activity.activityDepartments.map((department: Dto.IActivityDepartment) => { return new Business.ActivityDepartment(department) });
                }

                if (activity.attachments) {
                    this.attachments = activity.attachments.map((attachment: Dto.IAttachment) => { return new Business.Attachment(attachment) });
                }
                else {
                    this.attachments = [];
                }

                if (activity.viewings) {
                    this.viewings = activity.viewings.map((item) => new Viewing(item));
                    this.groupViewings(this.viewings);
                }

                if (activity.offers) {
                    this.offers = activity.offers.map((item) => new Offer(item));
                }

                if (activity.solicitor && activity.solicitorCompany) {
                    this.solicitor = new Contact(activity.solicitor);
                    this.solicitorCompany = new Company(activity.solicitorCompany);
                    this.solicitorCompanyContact = new CompanyContactRelation(this.solicitor, this.solicitorCompany);
                }

                if (activity.chainTransactions) {
                    this.chainTransactions = activity.chainTransactions.map((chainTransaction: Dto.IChainTransaction) => { return new Business.ChainTransaction(chainTransaction) });
                }
                else {
                    this.chainTransactions = [];
                }

                this.secondaryNegotiator = this.secondaryNegotiator || [];
                this.leadNegotiator = this.leadNegotiator || new ActivityUser();

                this.createChainTransactionMock();
            }
        }

        groupViewings(viewings: Viewing[]) {
            this.viewingsByDay = [];
            var groupedViewings: _.Dictionary<Viewing[]> = _.groupBy(viewings, (i: Viewing) => { return i.day; });
            this.viewingsByDay = <ViewingGroup[]>_.map(groupedViewings, (items: Viewing[], key: string) => { return new ViewingGroup(key, items); });
        }

        createChainTransactionMock(){
            // TODO remove after backend ready
            var parent = new ChainTransaction();            
            parent.id = "{E3AE5C64-6EDF-44ED-9572-9E6C31E6B5EE}";
            parent.activity = <Activity>{};
            parent.property = <Business.PreviewProperty>{
                ownerships: []
            };
            parent.property.address = <Business.Address>
            {
                id: "{B6CF58E1-5E99-4CCD-808D-17E78282A8AB}",
                propertyName : "West Forum",
                propertyNumber : "142",
                line2 : "Strzegomska",
                postcode: "123",
                countryId: "B17DDE86-0348-E611-8115-00155D038C01"
                }
            parent.property.address = new Business.Address(parent.property.address);
            parent.vendor = 'Mark Bower';
            parent.agentUser = <Business.User> {
                id: "{BB34F76F-0A43-4902-BDE3-6024FC80FE34}",
                firstName: "Agent",
                lastName: "User"
            };
            parent.solicitorContact = <Business.Contact> {
                id: "{D1152CD5-DB0A-4F7F-97BB-A1CA28661191}",
                firstName: "Kim",
                lastName: "West",
                title: "Mr"
            };
            parent.solicitorCompany = <Business.Company> {
                id: "{DE89525B-FDCA-4414-97DB-258172EE58D5}",
                name: "West Company"
            };
            parent.mortgage = new Business.EnumTypeItem();
            parent.mortgageId = 'e0994e69-3a44-e611-8299-8cdcd42baca7';
            parent.survey = new Business.EnumTypeItem();
            parent.surveyId = 'e8994e69-3a44-e611-8299-8cdcd42baca7';
            parent.searches = new Business.EnumTypeItem();
            parent.searchesId = 'ec994e69-3a44-e611-8299-8cdcd42baca7';
            parent.enquiries = new Business.EnumTypeItem();
            parent.enquiriesId = 'f0994e69-3a44-e611-8299-8cdcd42baca7';
            parent.contractAgreed = new Business.EnumTypeItem();
            parent.contractAgreedId = 'f3994e69-3a44-e611-8299-8cdcd42baca7';
            parent.surveyDate = new Date();

            var child = new ChainTransaction();
            child.id = "{9B58E45A-5347-47EE-89EF-DFB0ECA33C47}";
            child.activity = <Activity>{
                
            };
            child.property = <Business.PreviewProperty>{
                ownerships: []
            };
            child.property.address = <Business.Address>
                {
                id: "{9B58E45A-5347-47EE-89EF-DFB0ECA33C47}",
                    propertyName: "Sky Tower",
                    propertyNumber: "123",
                    line2: "Powstancow Slaskich",
                    postcode: "1232",
                    countryId: "B17DDE86-0348-E611-8115-00155D038C01"
                }
            child.property.address = new Business.Address(child.property.address);
            child.vendor = 'Gilberto Lyons';
            child.agentContact = <Business.Contact>{
                id: "{CCF43460-41DB-42A3-8DE8-6D7D71BCBFF1}",
                firstName: "Krista",
                lastName: "Porter",
                title: "Mr"
            };
            child.agentCompany = <Business.Company>{
                id: "{EA577562-88D3-4A1D-BD23-F08B36DE3171}",
                name: "Agent Company"
            };
            child.solicitorContact = <Business.Contact>{
                id: "{767460B5-191A-49DD-9F8F-5FB268237AC5}",
                firstName: "Tyhtrone",
                lastName: "Jonston",
                title: "Mr"
            };
            child.solicitorCompany = <Business.Company>{
                id: "{1B56ED97-C25B-40FF-A547-FE105B244326}",
                name: "East West"
            };
            child.mortgage = new Business.EnumTypeItem();
            child.mortgageId = 'e0994e69-3a44-e611-8299-8cdcd42baca7';
            child.survey = new Business.EnumTypeItem();
            child.surveyId = 'e8994e69-3a44-e611-8299-8cdcd42baca7';
            child.searches = new Business.EnumTypeItem();
            child.searchesId = 'ec994e69-3a44-e611-8299-8cdcd42baca7';
            child.enquiries = new Business.EnumTypeItem();
            child.enquiriesId = 'f0994e69-3a44-e611-8299-8cdcd42baca7';
            child.contractAgreed = new Business.EnumTypeItem();
            child.contractAgreedId = 'f3994e69-3a44-e611-8299-8cdcd42baca7';
            child.surveyDate = new Date();

            var child2 = new ChainTransaction();
            child2.id = "{959BEBA7-F46C-4B7F-93DA-EB532FEEB1AB}";
            child2.activity = <Activity>{
            };            
            child2.property = <Business.PreviewProperty>{
                ownerships: []
            };
            child2.property.address = <Business.Address>
                {
                id: "{2828AB31-CB63-40A9-AF95-E45543FF535F}",
                    propertyName: "Water Tower",
                    propertyNumber: "1",
                    line2: "Aleja Wisniowa",
                    postcode: "12",
                    countryId: "B17DDE86-0348-E611-8115-00155D038C01"
                }
            child2.property.address = new Business.Address(child2.property.address);
            child2.vendor = 'Faye Castro';
            child2.agentUser = <Business.User>{
                id: "{5345C209-CECF-44F8-98AC-C3ED90469DFB}",
                firstName: "John",
                lastName: "Smith"
            };
            child2.solicitorContact = <Business.Contact>{
                id: "{9CD195CE-B516-4C56-BEB8-632B04162443}",
                firstName: "Casey",
                lastName: "Delgado",
                title: "Mr"
            };
            child2.solicitorCompany = <Business.Company>{
                id: "{2207BBF3-2C28-4643-B9F4-1E62FEB98677}",
                name: "Delgado Company"
            };
            child2.mortgage = new Business.EnumTypeItem();
            child2.mortgageId = 'e0994e69-3a44-e611-8299-8cdcd42baca7';
            child2.survey = new Business.EnumTypeItem();
            child2.surveyId = 'e8994e69-3a44-e611-8299-8cdcd42baca7';
            child2.searches = new Business.EnumTypeItem();
            child2.searchesId = 'ec994e69-3a44-e611-8299-8cdcd42baca7';
            child2.enquiries = new Business.EnumTypeItem();
            child2.enquiriesId = 'f0994e69-3a44-e611-8299-8cdcd42baca7';
            child2.contractAgreed = new Business.EnumTypeItem();
            child2.contractAgreedId = 'f3994e69-3a44-e611-8299-8cdcd42baca7';
            child2.surveyDate = new Date();

            child.parent = parent;
            child2.parent = child;

            this.chainTransactions.push(parent);
            this.chainTransactions.push(child);
            this.chainTransactions.push(child2);
        }
    }
}