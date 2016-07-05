/// <reference path="../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import Enums = Common.Models.Enums;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider){
        $stateProvider
            .state('app.offer-view', {
                url : '/offer/:id',
                params : {},
                template: '<offer-view offer="offer" config="config"></offer-view>',
                controller: ($scope: ng.IScope, offer: Dto.IOffer, config: IOfferViewConfig) =>{
                    var offerViewModel = new Business.Offer(<Dto.IOffer>offer);

                    $scope['offer'] = offerViewModel;
                    $scope['config'] = config;
                },
                resolve : {
                    offer : ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) =>{
                        var offerId: string = $stateParams['id'];
                        return dataAccessService.getOfferResource().get({ id : offerId }).$promise;
                    },
                    config: (offer: Dto.IOffer, configService: Services.ConfigService) => {
                        return configService.getOffer(Enums.PageTypeEnum.Details, offer.requirement.requirementTypeId, offer.offerTypeId, offer);
                    }
                }
            })
            .state('app.offer-edit', {
                url: '/offer/edit/:id',
                template: "<offer-edit offer='offer' config='config'></offer-edit>",
                controller: ($scope: ng.IScope, offer: Dto.IOffer, config: IOfferEditConfig) => {
                    var offerViewModel = new Business.Offer(<Dto.IOffer>offer);

                    $scope['offer'] = offerViewModel;
                    $scope['config'] = config;
                },
                resolve: {
                    offer: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var offerId: string = $stateParams['id'];
                        return dataAccessService.getOfferResource().get({ id: offerId }).$promise;
                    },
                    config: (offer: Dto.IOffer, configService: Services.ConfigService) =>{
                        return configService.getOffer(Enums.PageTypeEnum.Update, offer.requirement.requirementTypeId, offer.offerTypeId, offer);
                    }
                }
            });
    };
}