///<reference path="../../../../typings/_all.d.ts"/>

module Antares {
   
    import runDescribe = TestHelpers.runDescribe;
    type TestCaseShowText = [boolean, string];
    type TestCaseProtocolCheck = [string,string];

    describe('Given external link component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: Common.Component.ExternalLinkController,
            createComponent = (url: string, showText: boolean) => { };

        var pageObjectSelector = {
            iconAsLink: 'a .fa-external-link[style!="display:none"]',
            iconNotAsLink: '.fa-external-link[style!="display:none"]',
            url: '[name=url]'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            scope = $rootScope.$new();
   
            createComponent = (url: string, showText: boolean) => {
                scope['url'] = url;
                element = $compile('<external-link url="url" show-text="' + showText + '"></external-link>')(scope);
                scope.$apply(); 

                controller = element.controller('externalLink');
            };
        }));

        it('the icon is displayed as a link when url is not displayed', () => {
            createComponent('www.test.com', false);
            var iconElements = element.find(pageObjectSelector.iconAsLink);

            expect(iconElements.length).toBe(1);
        });

        it('the icon is not displayed as a link when url is displayed', () => {
            createComponent('www.test.com', true);
            var iconElements = element.find(pageObjectSelector.iconNotAsLink);

            expect(iconElements.length).toBe(1);
        });

        runDescribe('when showText')
            .data<TestCaseShowText>([
                [true, 'http://www.test.com'],
                [false, ''],
            ])
            .dataIt((data: TestCaseShowText) =>
                ` is set to "${data[0] ? 'true' : 'false'}" then url text should ${data[0] ? '' : 'not'} be displayed`)
            .run((data: TestCaseShowText) => {
                var expectedUrl = data[1];

                createComponent(expectedUrl, data[0]);
                var urlElements = element.find(pageObjectSelector.url);

                //Assert
                expect(urlElements.length).toBe(1);
                expect(urlElements[0].innerText.trim()).toBe(expectedUrl);
            });

        runDescribe('when url is')
            .data<TestCaseProtocolCheck>([
                ["http://www.test.com/index.html", "http://www.test.com/index.html"],
                ["www.test.com/", "http://www.test.com/"],
                ["https://www.test.com/", "https://www.test.com/"]
            ])
            .dataIt((data: TestCaseProtocolCheck) =>
                ` set to "${data[0]}" then href of a tag should ${data[1]}`)
            .run((data: TestCaseProtocolCheck) => {
                var expectedUrl = data[1];

                createComponent(data[0], false);
                var urlElements = element.find(pageObjectSelector.url);
                var anchorElement = <HTMLAnchorElement>urlElements[0];

                //Assert
               expect(anchorElement.href).toBe(expectedUrl);
              
            });
    });
}