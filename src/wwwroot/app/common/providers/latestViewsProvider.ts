/// <reference path="../../typings/_all.d.ts" />


module Antares.Providers {
    import LatestViewsService = Services.LatestViewsService;
    import Address = Common.Models.Business.Address;
    import ILatestListEntry = Common.Models.Dto.ILatestListEntry;
    import ILatestViewResultItem = Common.Models.Dto.ILatestViewResultItem;
    import EntityTypeEnum = Common.Models.Enums.EntityTypeEnum;
    import LatestViewCommand = Common.Models.Commands.ICreateLatestViewCommand;

    export class LatestViewsProvider {
        [key: string]: any;

        public properties: ILatestListEntry[];
        public activities: ILatestListEntry[];

        constructor(private latestViewsService: LatestViewsService, private $state: angular.ui.IStateService) {
        }

        public refresh = (): ng.IPromise<void> => {
            return this.latestViewsService
                .get()
                .then(this.loadLatestData);
        }

        public addViewing = (command: LatestViewCommand) => {
            this.latestViewsService
                .post(command)
                .then(this.loadLatestData);
        }

        public loadLatestData = (result: angular.IHttpPromiseCallbackArg<ILatestViewResultItem[]>) => {
            this.loadProperties(result.data);
            this.loadActivities(result.data);
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
    }

    angular.module('app').service('latestViewsProvider', LatestViewsProvider);
}