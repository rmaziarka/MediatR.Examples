/// <reference path="../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider){
        $stateProvider
            .state('app.offer-view', {
                url : '/offer/:id',
                params : {},
                template : '<offer-view offer="offer"></offer-view>',
                controller : ($scope: ng.IScope, offer: Dto.IOffer) =>{
                    var offerViewModel = new Business.Offer(<Dto.IOffer>offer);

                    $scope['offer'] = offerViewModel;
                },
                resolve : {
                    offer : ($stateParams: ng.ui.IStateParamsService, dataAccessService: Antares.Services.DataAccessService) =>{
                        var offerId: string = $stateParams['id'];
                        return dataAccessService.getOfferResource().get({ id : offerId }).$promise;
                    }
                }
            });
    };
}