/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import TenancyEditController = Tenancy.TenancyEditController;
    import Enums = Common.Models.Enums;
    import Commands = Common.Models.Commands;
    import runDescribe = TestHelpers.runDescribe;

    describe('Given edit tenancy controller', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            $state: ng.ui.IStateService,
            controller: TenancyEditController,
            $q: ng.IQService,
            tenancyService: Services.TenancyService,
            $scope: ng.IScope;

        beforeEach(inject((_$state_: ng.ui.IStateService) => {
            $state = _$state_;
            spyOn($state, 'go').and.callFake(() => { });
        }));

        describe('when is in add mode', () => {
            var deferred: ng.IDeferred<any>;
            var requestData: Commands.Tenancy.TenancyAddCommand;
            var tenancyFromService: Dto.ITenancy;
            var tenancy: Business.TenancyEditModel;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: ng.IControllerService,
                $httpBackend: ng.IHttpBackendService,
                _$q_: ng.IQService,
                _tenancyService_: Services.TenancyService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;
                tenancyService = _tenancyService_;
                $q = _$q_;

                $scope = $rootScope.$new();
                controller = <TenancyEditController>$controller('TenancyEditController', { $scope: $scope });
                tenancy = TestHelpers.TenancyGenerator.generateEditModel();
                tenancy.id = null;

                controller.tenancy = tenancy;

                tenancyFromService = TestHelpers.TenancyGenerator.generateDto();

                deferred = $q.defer();
                spyOn(tenancyService, 'addTenancy').and.callFake((tenancyCommand: Commands.Tenancy.TenancyAddCommand) => {
                    requestData = tenancyCommand;
                    return deferred.promise;
                });
            }));

            it('when initializing then default landlords should be set', () => {
                // act
                controller.$onInit();

                // assert
                expect(controller.tenancy.landlords).toEqual(tenancy.activity.landlords);
            });

            it('when initializing then default tenants should be set', () => {
                // act
                controller.$onInit();

                // assert
                expect(controller.tenancy.tenants).toEqual(tenancy.requirement.contacts);
            });

            it('when save then correct data should be sent to API', () => {
                // act
                controller.save();
                deferred.resolve(tenancyFromService);
                $scope.$apply();

                // assert
                expect(requestData.activityId).toBe(tenancy.activity.id);
                expect(requestData.requirementId).toBe(tenancy.requirement.id);
                expect(requestData.term.agreedRent).toBe(tenancy.agreedRent);
                expect(requestData.term.startDate).toEqual(Core.DateTimeUtils.createDateAsUtc(tenancy.startDate));
                expect(requestData.term.endDate).toEqual(Core.DateTimeUtils.createDateAsUtc(tenancy.endDate));
                expect(requestData.landlordContacts).toEqual(tenancy.landlords.map((contact: Business.Contact) => { return contact.id }));
                expect(requestData.tenantContacts).toEqual(tenancy.tenants.map((contact: Business.Contact) => { return contact.id }));
            });
        });

        describe('when is in edit mode', () => {
            var deferred: ng.IDeferred<any>;
            var requestData: Commands.Tenancy.TenancyEditCommand;
            var tenancyFromService: Dto.ITenancy;
            var tenancy: Business.TenancyEditModel;

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: ng.IControllerService,
                $httpBackend: ng.IHttpBackendService,
                _$q_: ng.IQService,
                _tenancyService_: Services.TenancyService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;
                tenancyService = _tenancyService_;
                $q = _$q_;

                $scope = $rootScope.$new();
                controller = <TenancyEditController>$controller('TenancyEditController', { $scope: $scope });
                tenancy = TestHelpers.TenancyGenerator.generateEditModel();

                controller.tenancy = tenancy;

                tenancyFromService = TestHelpers.TenancyGenerator.generateDto();

                deferred = $q.defer();
                spyOn(tenancyService, 'updateTenancy').and.callFake((tenancyCommand: Commands.Tenancy.TenancyEditCommand) => {
                    requestData = tenancyCommand;
                    return deferred.promise;
                });
            }));

            it('when save then correct data should be sent to API', () => {
                // act
                controller.save();
                deferred.resolve(tenancyFromService);
                $scope.$apply();

                // assert
                expect(requestData.tenancyId).toBe(tenancy.id);
                expect(requestData.term.agreedRent).toBe(tenancy.agreedRent);
                expect(requestData.term.startDate).toEqual(Core.DateTimeUtils.createDateAsUtc(tenancy.startDate));
                expect(requestData.term.endDate).toEqual(Core.DateTimeUtils.createDateAsUtc(tenancy.endDate));
            });
        });
    });
}