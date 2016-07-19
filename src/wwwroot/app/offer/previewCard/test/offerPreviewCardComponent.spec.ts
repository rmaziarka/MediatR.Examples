/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import runDescribe = TestHelpers.runDescribe;

    describe('Given offerPreviewCardComponent', () => {
        var scope: ng.IScope,
            $compileService: ng.ICompileService;

        var pageObjectSelectors = {
            editButton: '#offer-preview-card-edit-button',
            activity: '#offer-preview-activity',
            requirement: '#offer-preview-requirement'
        }

        var messagePartResolver = (d: boolean) => d ? 'visible' : 'invisible';

        beforeEach(() => {
            angular.mock.module(($provide: any) => {
                $provide.service('addressFormsProvider', Mock.AddressFormsProviderMock);
            });
        });

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService) => {

            $compileService = $compile;

            scope = $rootScope.$new();
        }));

        describe('when is initialized', () => {
            type TestCase = [boolean, number];
            runDescribe('then edit button')
                .data<TestCase>([
                    [true, 1],
                    [false, 0]
                ])
                .dataIt((data: TestCase) =>
                    `should be ${messagePartResolver(data[0])} based on binding (${data[0]})`)
                .run((data: TestCase) => {
                    var element = $compileService(`<offer-preview-card can-edit="${data[0]}"></offer-preview-card>`)(scope);
                    scope.$apply();
                    
                    var editButton = element.find(pageObjectSelectors.editButton);

                    expect(editButton.length).toBe(data[1]);
                });
        });

        describe('when is initialized', () => {
            type TestCase = [boolean, number];
            runDescribe('then activity section')
                .data<TestCase>([
                    [true, 1],
                    [false, 0]
                ])
                .dataIt((data: TestCase) =>
                    `should be ${messagePartResolver(data[0])} based on binding (${data[0]})`)
                .run((data: TestCase) => {
                    var offer = TestHelpers.OfferGenerator.generate();
                    scope['offer'] = offer;

                    var element = $compileService(`<offer-preview-card offer="offer" show-activity="${data[0]}"></offer-preview-card>`)(scope);
                    scope.$apply();

                    var activitySection = element.find(pageObjectSelectors.activity);

                    expect(activitySection.length).toBe(data[1]);
                });
        });

        describe('when is initialized', () => {
            type TestCase = [boolean, number];
            runDescribe('then requirement section')
                .data<TestCase>([
                    [true, 1],
                    [false, 0]
                ])
                .dataIt((data: TestCase) =>
                    `should be ${messagePartResolver(data[0])} based on binding (${data[0]})`)
                .run((data: TestCase) => {
                    var element = $compileService(`<offer-preview-card show-requirement="${data[0]}"></offer-preview-card>`)(scope);
                    scope.$apply();

                    var requirementSection = element.find(pageObjectSelectors.requirement);

                    expect(requirementSection.length).toBe(data[1]);
                });
        });
    });
}