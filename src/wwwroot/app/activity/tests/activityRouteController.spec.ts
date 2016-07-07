/// <reference path="../../typings/_all.d.ts" />

module Antares {
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import IActivityViewConfig = Antares.Activity.IActivityViewConfig;

    describe('Given activity route controller', () =>{
        var scope: ng.IScope;
        var controllerProvider: ng.IControllerService;
        var latestViewsProviderSpy: LatestViewsProvider;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $controller: any,
            latestViewsProvider: LatestViewsProvider) =>{

            spyOn(latestViewsProvider, 'addView');
            latestViewsProviderSpy = latestViewsProvider;

            scope = $rootScope.$new();

            controllerProvider = $controller;
        }));

        describe('when constructor is executed', () => {
            it('then addView of last views provider is called', () => {
                //Arrange
                var activityMock = TestHelpers.ActivityGenerator.generate();
                var config = { vendors: {} } as IActivityViewConfig;
                var parametrs = { $scope: scope, activity: activityMock, latestViewsProvider: latestViewsProviderSpy, config: config };

                //Act
                controllerProvider('ActivityRouteController', parametrs);

                //Assert
                expect(latestViewsProviderSpy.addView).toHaveBeenCalledWith({ entityId: activityMock.id, entityType: EntityType.Activity });
            });
        });
    });
}