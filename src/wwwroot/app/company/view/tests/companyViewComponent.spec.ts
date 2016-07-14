/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import CompanyViewController = Antares.Company.CompanyViewController;
    import Business = Antares.Common.Models.Business;

    describe('Given company view', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            compile: ng.ICompileService,
            controller: Company.CompanyViewController;

        var companyMock: Business.Company = TestHelpers.CompanyGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            enumService: Mock.EnumServiceMock) => {

            compile = $compile;

            scope = $rootScope.$new();
            element = compile('<company-view company="company"></company-view>')(scope);
            scope["company"] = companyMock;
            scope.$apply();

            controller = element.controller('companyView');
        }
        ));

        it('todo', () => {
            expect(1).toBe(1);
        });
       
    });
}