/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IListNotEmptyScope extends ng.IScope {
        model: Array<any>;
        form: any;
    }

    describe('Given input rendered with list-not-empty attribute', () => {
        var scope: IListNotEmptyScope,
            element: ng.IAugmentedJQuery;

        beforeEach(angular.mock.module('app'));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = <IListNotEmptyScope>$rootScope.$new();
            element = $compile('<form name="form"><list-not-empty ng-model="model" name="model" /></form>')(scope);
            scope.$apply();
        }));

        it('when model is empty form should be invalid', () => {
            scope.model = [];
            scope.$apply();
            
            expect(scope.form.$invalid).toBe(true);
            expect(scope.form.model.$invalid).toBe(true);
            expect(scope.form.model.$dirty).toBe(true);
        });

        it('when model is not empty form should be valid', () =>{
            scope.model = [1, 2];
            scope.$apply();

            expect(scope.form.$valid).toBe(true);
            expect(scope.form.model.$valid).toBe(true);
            expect(scope.form.model.$dirty).toBe(true);
        });
    });
}