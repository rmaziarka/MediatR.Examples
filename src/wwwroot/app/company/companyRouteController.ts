/// <reference path="../typings/_all.d.ts" />

module Antares.Company {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class CompanyRouteController {

        constructor($scope: ng.IScope, company: Dto.ICompany, latestViewsProvider: LatestViewsProvider) {
            var companyViewModel = new Business.Company(company);

            $scope['company'] = companyViewModel;      

            latestViewsProvider.addView({
                entityId : company.id,
                entityType : EntityType.Company
            });
        }
    }

    angular.module('app').controller('CompanyRouteController', CompanyRouteController);
};