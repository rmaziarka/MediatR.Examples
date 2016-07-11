/// <reference path="../../typings/_all.d.ts" />

module Antares {
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    describe('Given contact route controller', () => {
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
                var contactMock = TestHelpers.ContactGenerator.generate();
                var parametrs = { $scope: scope, contact: contactMock, latestViewsProvider: latestViewsProviderSpy };

                //Act
                controllerProvider('ContactRouteController', parametrs);
                
                //Assert
                expect(latestViewsProviderSpy.addView).toHaveBeenCalledWith({ entityId: contactMock.id, entityType: EntityType.Contact });
            });
        });
    });
}