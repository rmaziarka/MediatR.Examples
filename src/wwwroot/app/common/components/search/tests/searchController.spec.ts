/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import SearchController = Common.Component.SearchController;

    describe('Given search controller', () => {
        var $scope: ng.IScope,
            $http: ng.IHttpBackendService,
            controller: SearchController;

        describe('when select is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { onSelectItem: () => {}, onCancel: () => {} };
                controller = <SearchController>$controller('SearchController', {}, bindings);
            }));

            it('then onSelectItem is called', () => {
                // arrange
                spyOn(controller, 'onSelectItem');

                //act
                var testItem = {};
                controller.select(testItem);

                // assert
                expect(controller.onSelectItem).toHaveBeenCalledWith(testItem);
            });

            it('then selectedItem is cleared', () => {
                // arrange
                controller.selectedItem = {};

                //act
                controller.select({});

                // assert
                expect(controller.selectedItem).toBeNull();
            });
        });

        describe('when cancel is called', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                $scope = $rootScope.$new();
                $http = $httpBackend;

                var bindings = { onSelectItem: () => { }, onCancel: () => { } };
                controller = <SearchController>$controller('SearchController', {}, bindings);
            }));

            it('then onCancel is called', () => {
                // arrange
                spyOn(controller, 'onCancel');

                //act
                controller.cancel();

                // assert
                expect(controller.onCancel).toHaveBeenCalled();
            });

            it('then selectedItem is cleared', () => {
                // arrange
                controller.selectedItem = {};

                //act
                controller.cancel();

                // assert
                expect(controller.selectedItem).toBeNull();
            });
        });
	});
}