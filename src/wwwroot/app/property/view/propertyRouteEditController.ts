/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class PropertyRouteEditController {

        constructor(private $scope: ng.IScope, property: Dto.IProperty, private latestViewsProvider: LatestViewsProvider) {
            var propertyViewModel = new Business.Property(property);

            $scope['property'] = propertyViewModel;

            this.saveRecentViewdProperty(propertyViewModel.id);
        }

        private saveRecentViewdProperty(propertyId: string){
            this.latestViewsProvider.addView({
                entityId : propertyId,
                entityType : EntityType.Property
            });
        }
    }

    angular.module('app').controller('PropertyRouteEditController', PropertyRouteEditController);
};