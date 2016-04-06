/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    describe('Given list component', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery;

        var mockedComponentHtml = '<list>'
            + '<list-header><p id="test-header">Header test</p></list-header>'
            + '<list-no-items><p id="test-no-items">Test</p></list-no-items>'
            + '<p id="test-item">Test</p>'
            + '</list>';

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            // init
            scope = $rootScope.$new();
            compile = $compile;

            element = compile(mockedComponentHtml)(scope);
            scope.$apply();
        }));

        it('when loaded then all component transcluded elements should be visible', () => {
            var headerElement = element.find('p#test-header');
            var noItemsElement = element.find('p#test-no-items');
            var itemElement = element.find('p#test-item');

            expect(headerElement.length).toBe(1);
            expect(noItemsElement.length).toBe(1);
            expect(itemElement.length).toBe(1);
        });
    });
}