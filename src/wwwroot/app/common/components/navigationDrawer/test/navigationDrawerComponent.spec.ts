/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import NavigationDrawerController = Common.Component.NavigationDrawerController;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import LatestListEntry = Common.Models.Dto.ILatestListEntry;
    import AngularStatic = angular.IAngularStatic;

    var pageObjectElements = {
        recentRoot: '.recent',
        recentItem: '.recent-item'
    };

    describe('Given navigation drawer component loaded 10 elements', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NavigationDrawerController;

        var latestViews: LatestListEntry[] = [];

        var mockedPropertyNagivationDrawerComponent = '<navigation-drawer type="property"></navigation-drawer>';

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            latestViewsProvider: LatestViewsProvider,
            $compile: ng.ICompileService) => {

            latestViews = <LatestListEntry[]>[];
            for (var i = 0; i < 10; i++) {
                latestViews.push(<LatestListEntry>{
                    id: "37fea3ab-5829-e611-84bb-34e6d744328" + i,
                    name: "property name " + i,
                    url: "property url " + i
                });
            }

            latestViewsProvider.properties = latestViews;

            // init
            scope = $rootScope.$new();
            compile = $compile;

            element = compile(mockedPropertyNagivationDrawerComponent)(scope);
            controller = element.controller('navigationDrawer');

            scope.$apply();
        }));

        it('then 10 recent viewed properties are displayed', () => {
            var recentItems = element
                .find(pageObjectElements.recentRoot)
                .find(pageObjectElements.recentItem);

            expect(recentItems.length).toBe(10);
        });

        it('then recent drawer componenet displays correct data', () => {
            var recentItems = element
                .find(pageObjectElements.recentRoot)
                .find(pageObjectElements.recentItem);

            var expectedUrl = recentItems.first().find('a').attr('href');
            var expectedName = recentItems.first().find('a').text();

            expect(expectedUrl).toEqual(latestViews[0].url);
            expect(expectedName).toEqual(latestViews[0].name);
        });
    });

    describe("There is a navigation drawer for company type", () => {
        var mockedCompanyNagivationDrawerComponent = '<navigation-drawer type="company"></navigation-drawer>';

        var element: ng.IAugmentedJQuery;

        beforeEach(inject((
            // arrange
            $rootScope: ng.IRootScopeService,
            latestViewsProvider: LatestViewsProvider,
            $compile: ng.ICompileService) => {

            const scope = $rootScope.$new();

            latestViewsProvider.companies = [<LatestListEntry>{
                id: "37fea3ab-5829-e611-84bb-34e6d744328",
                name: "c1"
            }];

            // act
            element = $compile(mockedCompanyNagivationDrawerComponent)(scope);
            scope.$apply();
        }));

        it('Recent companies list is available through navigation-drawer', () => {
            var recentItems = element
                .find(pageObjectElements.recentRoot)
                .find(pageObjectElements.recentItem);

            // assert
            expect(recentItems.length).toBe(1);
        });
    });

    describe('Given contact navigation drawer component loaded 10 elements', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NavigationDrawerController;

        var latestViews: LatestListEntry[] = [];

        var mockedContactNagivationDrawerComponent = '<navigation-drawer type="contact"></navigation-drawer>';

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            latestViewsProvider: LatestViewsProvider,
            $compile: ng.ICompileService) => {

            latestViews = <LatestListEntry[]>[];
            for (var i = 0; i < 10; i++) {
                latestViews.push(<LatestListEntry>{
                    id: "37fea3ab-5829-e611-84bb-34e6d744328" + i,
                    name: "contact name " + i,
                    url: "contact url " + i
                });
            }

            latestViewsProvider.contacts = latestViews;

            // init
            scope = $rootScope.$new();
            compile = $compile;

            element = compile(mockedContactNagivationDrawerComponent)(scope);
            controller = element.controller('navigationDrawer');

            scope.$apply();
        }));

        it('then 10 recent viewed contacts are displayed', () => {
            var recentItems = element
                .find(pageObjectElements.recentRoot)
                .find(pageObjectElements.recentItem);

            expect(recentItems.length).toBe(10);
        });

        it('then recent contact drawer component displays correct data', () => {
            var recentItems = element
                .find(pageObjectElements.recentRoot)
                .find(pageObjectElements.recentItem);

            var expectedUrl = recentItems.first().find('a').attr('href');
            var expectedName = recentItems.first().find('a').text();

            expect(expectedUrl).toEqual(latestViews[0].url);
            expect(expectedName).toEqual(latestViews[0].name);
        });
    });
}