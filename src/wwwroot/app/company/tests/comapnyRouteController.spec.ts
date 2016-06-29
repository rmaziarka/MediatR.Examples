/// <reference path="../../typings/_all.d.ts" />

module Antares {
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    describe('Given company route controller', () => {
        var scope: ng.IScope;
        var controllerProvider: ng.IControllerService;
        var latestViewsProviderSpy: LatestViewsProvider;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            latestViewsProvider: LatestViewsProvider) => {

            spyOn(latestViewsProvider, 'addView');
            latestViewsProviderSpy = latestViewsProvider;

            scope = $rootScope.$new();

            controllerProvider = $controller;
        }));

        describe('when constructor is executed', () => {
            it('then addView of last views provider is called', () => {
                //Arrange
                var companyMock = TestHelpers.CompanyGenerator.generate();
                var parametrs = { $scope: scope, company: companyMock, latestViewsProvider: latestViewsProviderSpy };

                //Act
                controllerProvider('CompanyRouteController', parametrs);
                
                //Assert
                expect(latestViewsProviderSpy.addView).toHaveBeenCalledWith({ entityId: companyMock.id, entityType: EntityType.Company });
            });
        });
    });
}