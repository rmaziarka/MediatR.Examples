/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferViewController = Component.OfferViewController;
	import Dto = Common.Models.Dto;

    describe('Given offer view component is loaded', () =>{
	    var scope: ng.IScope,
	        element: ng.IAugmentedJQuery,
	        controller: OfferViewController,
	        state: ng.ui.IStateService,
	        $http: ng.IHttpBackendService,
	        compile: ng.ICompileService;

        var pageObjectSelectors = {
            vendorSection: '#section-vendor',
            applicantSection: '#section-applicant',
            pageHeader: '#view-offer-header',
            pageHeaderTitle: '.offer-title',
            sectionBasicInformation: '#section-basic-information',
            panelBody: '.panel-body',
            progressSection: '#offer-progress-section',
            activityCard: '.offer-view-activity',
            requirementCard: '.offer-view-requirement'
        }

		var offerStatuses: Common.Models.Dto.IEnumItem[] = [
            { id: '1', code: 'New' },
            { id: '2', code: 'Closed' },
            { id: '3', code: 'Accepted' }
        ];

		var offerMock = TestHelpers.OfferGenerator.generate();

        beforeEach(() => {
            angular.mock.module(($provide: any) => {
                $provide.factory('addressFormViewDirective', () => { return {}; });
            });
        });

        beforeEach(inject(($rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $state: ng.ui.IStateService,
            $httpBackend: ng.IHttpBackendService,
			enumService: Mock.EnumServiceMock) => {

            $http = $httpBackend;
            compile = $compile;
			state = $state;
            scope = $rootScope.$new();
            scope["offer"] = offerMock;
            scope["config"] = TestHelpers.ConfigGenerator.generateOfferViewConfig();

            enumService.setEnum(Dto.EnumTypeCode.OfferStatus.toString(), offerStatuses);

            element = compile('<offer-view offer="offer" config="config"></offer-view>')(scope);
            scope.$apply();
			
			controller = element.controller('offerView');
        }));

        describe('when data are retrived form the server', () => {
            it('page header displays applicant names seppareted by comma', () => {
			    //Add two contacts
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());
                offerMock.requirement.contacts.push(TestHelpers.ContactGenerator.generate());

                scope['offer'] = offerMock;
                scope.$apply();

                var expectedHeaderNamesText = offerMock.requirement.getContactNames();
                var currentHeaderNames = element.find(pageObjectSelectors.pageHeader).find(pageObjectSelectors.pageHeaderTitle).text();
                
                expect(currentHeaderNames).toEqual(expectedHeaderNamesText);
            });

            it('activity card is displayed', () =>{
                var offerCard = element.find(pageObjectSelectors.activityCard);
                expect(offerCard.length).toBe(1);
            });

            it('requirement card is displayed', () => {
                var requirementCard = element.find(pageObjectSelectors.requirementCard);
                expect(requirementCard.length).toBe(1);
            });
        });        

        describe('when details button is clicked', () => {
            it('on context menu within vendor card then redirect to activity is performed', () => {
                var activityMock = TestHelpers.ActivityGenerator.generate();

                spyOn(state, 'go');

                controller.navigateToActivity(activityMock);
                scope.$apply();

                var vendorDetailsLink = element.find(pageObjectSelectors.vendorSection).children("a");
                vendorDetailsLink.click();
                
                expect(state.go).toHaveBeenCalledWith('app.activity-view', { id: activityMock.id });
            });

            

            it('on context menu within applicant card then redirect to requiremen is performed', () => {
                var requirementMock = TestHelpers.RequirementGenerator.generate();

                spyOn(state, 'go');

                controller.navigateToRequirement(requirementMock);
                scope.$apply();

                var applicantDetailsLink = element.find(pageObjectSelectors.applicantSection).children("a");
                applicantDetailsLink.click();

                expect(state.go).toHaveBeenCalledWith('app.requirement-view', { id: requirementMock.id });
            });
        });

        describe('when activity details is clicked', () => {
            it('then activity should be added to property activity list', () => {
                $http.expectPOST(/\/api\/latestviews/, () => {
                    return true;
                }).respond(200, []);

                var activityMock = TestHelpers.ActivityGenerator.generate();
                
                offerMock.activity = activityMock;
                scope.$apply();

                controller.showActivityPreview(offerMock);

                expect($http.flush).not.toThrow();
            });
        });
    });
}