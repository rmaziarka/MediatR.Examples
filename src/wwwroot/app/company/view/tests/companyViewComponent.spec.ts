/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    describe('Given company view', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            controller: Company.CompanyViewController;

        var pageObjectSelectors = {
           
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            compile = $compile;

            scope = $rootScope.$new();
            element = compile('<company-view></company-view>')(scope);
            scope.$apply();

            controller = element.controller('companyView');
        }
        ));

        it('todo', () => { });
       
    });
}