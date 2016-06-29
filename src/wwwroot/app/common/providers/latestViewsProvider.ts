/// <reference path="../../typings/_all.d.ts" />


module Antares.Providers {
    import LatestViewsService = Services.LatestViewsService;
    import Address = Common.Models.Business.Address;
    import Contact = Common.Models.Business.Contact;
    import Company = Common.Models.Business.Company;
    import ILatestListEntry = Common.Models.Dto.ILatestListEntry;
    import ILatestViewResultItem = Common.Models.Dto.ILatestViewResultItem;
    import EntityTypeEnum = Common.Models.Enums.EntityTypeEnum;
    import LatestViewCommand = Common.Models.Commands.ICreateLatestViewCommand;

    export class LatestViewsProvider {
        [key: string]: any;

        public properties: ILatestListEntry[];
        public activities: ILatestListEntry[];
        public requirements: ILatestListEntry[];
        public companies: ILatestListEntry[];

        constructor(private latestViewsService: LatestViewsService, private $state: angular.ui.IStateService) {
        }

        public refresh = (): ng.IPromise<void> => {
            return this.latestViewsService
                .get()
                .then(this.loadLatestData);
        }

        public addView = (command: LatestViewCommand) => {
            this.latestViewsService
                .post(command)
                .then(this.loadLatestData);
        }

        public loadLatestData = (result: angular.IHttpPromiseCallbackArg<ILatestViewResultItem[]>) => {
            this.loadProperties(result.data);
            this.loadActivities(result.data);
            this.loadRequirements(result.data);
            this.loadCompanies(result.data);
        }

        public loadActivities = (latestViewsItems: ILatestViewResultItem[]) => {
            var latestActivities = latestViewsItems
                .filter((item) => item.entityTypeCode === <any>EntityTypeEnum.Activity)[0];

            if (!latestActivities)
                return;

            var latestActivitiesViews = latestActivities.list;

            var activities = latestActivitiesViews.map(view =>
                <ILatestListEntry>{
                    id: view.id,
                    name: new Address(view.data).getAddressText() || '-',
                    url: this.$state.href("app.activity-view", { id: view.id })
                });

            this.activities = activities;
        }

        public loadProperties = (latestViewsItems: ILatestViewResultItem[]) =>{
            var latestProperties = latestViewsItems
                .filter((item) => item.entityTypeCode === <any>EntityTypeEnum.Property)[0];

            if (!latestProperties)
                return;

            var latestPropertiesViews = latestProperties.list;

            var properties = latestPropertiesViews.map(view =>
                <ILatestListEntry>{
                    id : view.id,
                    name : new Address(view.data).getAddressText() || '-',
                    url : this.$state.href("app.property-view", { id : view.id })
                });

            this.properties = properties;
        }

        public loadRequirements = (latestViewsItems: ILatestViewResultItem[]) =>{
            var latestRequirements = latestViewsItems
                .filter((item) => item.entityTypeCode === <any>EntityTypeEnum.Requirement)[0];

            if (!latestRequirements)
                return;

            var latestRequirementViews = latestRequirements.list;
            var requirements = latestRequirementViews.map(view =>
                <ILatestListEntry>{
                    id : view.id,
                    name : view.data.map((c: Contact) => new Contact(c).getName()).join(", "),
                    url : this.$state.href("app.requirement-view", { id : view.id })
                });

            this.requirements = requirements;
        }

        public loadCompanies = (latestViewsItems: ILatestViewResultItem[]) => {
            var latestCompanies = latestViewsItems
                .filter((item) => item.entityTypeCode === <any>EntityTypeEnum.Company)[0];

            if (!latestCompanies)
                return;

            var companies = latestCompanies.list.map(view =>
                <ILatestListEntry>{
                    id: view.id,
                    name: new Company(view.data).name,
                    url: this.$state.href("app.company-view", { id: view.id })
                });

            this.companies = companies;
        }
    }

    angular.module('app').service('latestViewsProvider', LatestViewsProvider);
}