/// <reference path="../../../../typings/_all.d.ts" />

module Antares {
    import NavigationDrawerController = Antares.Common.Component.NavigationDrawerController;
    import LatestViewsProvider = Antares.Providers.LatestViewsProvider;
    import LatestListEntry = Antares.Common.Models.Dto.ILatestListEntry;

    describe('Given navigation drawer component loaded 10 elements', () => {

        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            controller: NavigationDrawerController,
            latestViewsProvider: LatestViewsProvider;

        var latestViews = [];

        var mockedPropertyNagivationDrawerComponent = '<navigation-drawer type="property"></navigation-drawer>';

        var pageObjectElements = {
            recentRoot: '.recent',
            recentItem: '.recent-item'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            latestViewsProvider: LatestViewsProvider,
            $compile: ng.ICompileService) => {

            latestViews = [];
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
}