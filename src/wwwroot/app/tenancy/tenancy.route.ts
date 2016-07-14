/// <reference path="../typings/_all.d.ts" />

module Antares.Tenancy {
    import Dto = Antares.Common.Models.Dto;
    import Business = Antares.Common.Models.Business;
    import PageTypeEnum = Antares.Common.Models.Enums.PageTypeEnum;

    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.tenancy-view', {
                url: '/tenancy/view/:id',
                template: "<tenancy-view tenancy='tenancy' config='config'></tenancy-view>",
                controller: ($scope: ng.IScope, tenancy: Dto.ITenancy, config: Antares.Tenancy.ITenancyEditConfig) => {
                    $scope['tenancy'] = new Business.TenancyViewModel(tenancy);
                    $scope['config'] = config;
                },
                resolve: {
                    tenancy: ($stateParams: ng.ui.IStateParamsService, tenancyService: Antares.Services.TenancyService) => {
                        return tenancyService.getTenancy($stateParams['id']);
                    },
                    config: (tenancy: Dto.ITenancy, configService: Services.ConfigService) => {
                        var entity = new Business.TenancyViewModel(tenancy);

                        return configService.getTenancy(PageTypeEnum.Details,
                            tenancy.requirement.requirementTypeId,
                            tenancy.tenancyTypeId,
                            entity);
                    }
                }
            })
            .state('app.tenancy-edit', {
                url: '/tenancy/edit/:id',
                template: "<tenancy-edit tenancy='tenancy' config='config'></tenancy-edit>",
                controller: ($scope: ng.IScope, tenancy: Dto.ITenancy, config: Antares.Tenancy.ITenancyEditConfig) => {
                    $scope['tenancy'] = new Business.TenancyEditModel(tenancy);
                    $scope['config'] = config;
                },
                resolve: {
                    tenancy: ($stateParams: ng.ui.IStateParamsService, tenancyService: Antares.Services.TenancyService) => {
                        return tenancyService.getTenancy($stateParams['id']);
                    },
                    config: (tenancy: Dto.ITenancy, configService: Services.ConfigService) => {
                        var entity = new Business.TenancyEditModel(tenancy);

                        return configService.getTenancy(PageTypeEnum.Update,
                            tenancy.requirement.requirementTypeId,
                            tenancy.tenancyTypeId,
                            new Common.Models.Commands.Tenancy.TenancyEditCommand(entity));
                    }
                }
            }).state('app.tenancy-add', {
                url: '/activity/:activityId/requirement/:requirementId/tenancy/add',
                template: "<tenancy-edit tenancy='tenancy' config='config'></tenancy-edit>",
                controller: ($scope: ng.IScope, requirement: Dto.IRequirement, activity: Dto.IActivity, config: Antares.Tenancy.ITenancyEditConfig) => {
                    var tenancy = new Business.TenancyEditModel();
                    tenancy.activity = new Business.ActivityPreviewModel(activity);
                    tenancy.requirement = new Business.RequirementPreviewModel(requirement);

                    $scope['tenancy'] = tenancy;
                    $scope['config'] = config;
                },
                resolve: {
                    tenancyTypes: (tenancyService: Antares.Services.TenancyService) => {
                        return tenancyService.getTenancyTypes();
                    },  
                    activity: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var activityId: string = $stateParams['activityId'];

                        return dataAccessService.getActivityResource().get({ id: activityId }).$promise;
                    },
                    requirement: ($stateParams: ng.ui.IStateParamsService, dataAccessService: Services.DataAccessService) => {
                        var requirementId = $stateParams['requirementId'];
                        return dataAccessService.getRequirementResource().get({ id: requirementId }).$promise;
                    },
                    config: (configService: Services.ConfigService, requirement: Dto.IRequirement, tenancyTypes: Dto.IResourceType[]) => {
                        var lettingType =_.find(tenancyTypes, (type: Dto.IResourceType) => { return type.enumCode === Antares.Common.Models.Enums.TenancyType[Antares.Common.Models.Enums.TenancyType.ResidentialLetting]; });

                        var entity = new Common.Models.Commands.Tenancy.TenancyAddCommand();
                        return configService.getTenancy(PageTypeEnum.Create,
                            requirement.requirementTypeId,
                            lettingType.id,
                            entity);
                    },
                }
            });;
    }
}