/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    describe('Given list item component', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery;

        var mockedComponentHtml = '<list-item>'
            + '<p id="test-item">Test</p>'
            + '</list-item>';

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
            var itemElement = element.find('p#test-item');

            expect(itemElement.length).toBe(1);
        });
    });
}