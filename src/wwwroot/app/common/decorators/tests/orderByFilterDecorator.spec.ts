/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    interface IScope extends ng.IScope {
        list: Array<{number: number}>;
    }

    describe('orderBy filter with descending order', () =>{
        var scope: IScope,
            $compileService: ng.ICompileService;

        beforeEach(angular.mock.module(($provide: angular.auto.IProvideService) =>{
            $provide.decorator('orderByFilter', Antares.Common.Decorators.OrderByFilterDecorator.decoratorFunction);
        }));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) =>{

            $compileService = $compile;
            scope = <IScope>$rootScope.$new();
            scope.list = [
                { number: 1},
                { number: 0},
                { number: null}
            ];
            scope.$apply();
        }));

        it('when invertEmpties is undefined should put null values first', () =>{
            var listHtml: string = createListHtml();
            var element: ng.IAugmentedJQuery = $compileService(listHtml)(scope);
            scope.$apply();

            var htmlValues = getHtmlValuesFromElement(element);
            expect(htmlValues).toEqual(["", "1", "0"]);
        });

        it('when invertEmpties is true should put null values on the end', () => {
            var listHtml: string = createListHtml(true);
            var element: ng.IAugmentedJQuery = $compileService(listHtml)(scope);
            scope.$apply();

            var htmlValues = getHtmlValuesFromElement(element);
            expect(htmlValues).toEqual(["1", "0", ""]);
        });

        it('when invertEmpties is false should put null values first', () => {
            var listHtml: string = createListHtml(false);
            var element: ng.IAugmentedJQuery = $compileService(listHtml)(scope);
            scope.$apply();

            var htmlValues = getHtmlValuesFromElement(element);
            expect(htmlValues).toEqual(["", "1", "0"]);
        });
    });

    var createListHtml = (invertEmpties?: boolean) : string => {
        var invertEmptiesHtml: string = '';
        if (angular.isDefined(invertEmpties)) {
            if (invertEmpties) {
                invertEmptiesHtml = ': true';
            }
            else {
                invertEmptiesHtml = ': false';
            }
        }

        return '<div><span ng-repeat="e in list | orderBy : \'number\' : true ' + invertEmptiesHtml+ '">{{e.number}}</span></div>';
    }

    function getHtmlValuesFromElement(element: ng.IAugmentedJQuery){
        var spans: HTMLElement[] = element.find('span').toArray();

        var htmlValues = spans.map((el: HTMLElement) =>{
            return el.innerText;
        });

        return htmlValues;
    }
}