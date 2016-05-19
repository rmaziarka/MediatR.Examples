///<reference path="../../../../typings/_all.d.ts"/>

module Antares {
   
    import runDescribe = TestHelpers.runDescribe;
    type TestCase = [boolean, string];

    describe('Given external link component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Common.Component.ExternalLinkController,
            createComponent = (url: string, showText: boolean) => { };

        var pageObjectSelector = {
            icon: '.fa-external-link',
            url: '[name=url]'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();

            createComponent = (url: string, showText: boolean)=> {
                element = $compile('<external-link url="' + url + '" show-text="' + showText + '"></external-link>')(scope);
                scope.$apply(); 

                controller = element.controller('externalLink');
            };
        }));

        it('the icon is displayed', () => {
            createComponent('www.test.com', true);
            var iconElements = element.find(pageObjectSelector.icon);

            expect(iconElements.length).toBe(1);
        });

        runDescribe('when showText')
            .data<TestCase>([
                [true, 'http://www.test.com'],
                [false, ''],
            ])
            .dataIt((data: TestCase) =>
                ` is set to "${data[0] ? 'true' : 'false'}" then url should ${data[0] ? '' : 'not'} be displayed`)
            .run((data: TestCase) => {
                var expectedUrl = data[1];

                createComponent(expectedUrl, data[0]);
                var urlElements = element.find(pageObjectSelector.url);

                //Assert
                expect(urlElements.length).toBe(1);
                expect(urlElements[0].innerText.trim()).toBe(expectedUrl);
            });
    });
}