/// <reference path="../typings/_all.d.ts" />

module Antares.Contact {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider){
        $stateProvider
            .state('app.contact-add', {
                url: '/contact/add',
                params : {},
                template: "<contact-add user-data='appVm.userData' contact='contact'></contact-add>"
            })
            .state('app.contact-edit', {
                url: '/contact/edit/:id',
                params: {},
                template: "<contact-edit user-data='appVm.userData' contact='contact'></contact-edit>",
                controller: "ContactRouteController",
                resolve: {
                    contact: ($stateParams: angular.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var contactId: string = $stateParams['id'];
                        return dataAccessService.getContactResource().get({ id: contactId }).$promise;
                    }
                }
            })
            .state('app.contact-view', {
                url: '/contact/:id',
                template: '<contact-view contact="contact"></contact-view>',
                controller: "ContactRouteController",
                resolve : {
                    contact : ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var contactId: string = $stateParams['id'];
                        return dataAccessService.getContactResource().get({ id : contactId }).$promise;
                    }
                }
            });
    }
}