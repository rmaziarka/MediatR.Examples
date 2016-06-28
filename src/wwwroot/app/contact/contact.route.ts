/// <reference path="../typings/_all.d.ts" />

module Antares.Contact {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider){
        $stateProvider
            .state('app.contact-add', {
                url: '/contact/add',
                params : {},
                template: "<contact-add user-data='appVm.userData'></contact-add>"
            })
            .state('app.contact-view', {
                url: '/contact/:id',
                template: '<contact-view contact="contact"></contact-view>',
                controller: ($scope: ng.IScope, contact: Dto.IContact) =>{
                    $scope['contact'] = new Business.Contact(contact);
                },
                resolve : {
                    contact : ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) => {
                        var contactId: string = $stateParams['id'];
                        return dataAccessService.getContactResource().get({ id : contactId }).$promise;
                    }
                }
            });
    }
}