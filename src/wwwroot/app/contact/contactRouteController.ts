/// <reference path="../typings/_all.d.ts" />

module Antares.Contact {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class ContactRouteController {

        constructor($scope: ng.IScope, contact: Dto.IContact, latestViewsProvider: LatestViewsProvider) {
            var contactViewModel = new Business.Contact(contact);

            $scope['contact'] = contactViewModel;      

            latestViewsProvider.addView({
                entityId: contact.id,
                entityType : EntityType.Contact
            });
        }
    }

    angular.module('app').controller('ContactRouteController', ContactRouteController);
};