/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Dto = Common.Models.Dto;
    import OfferEditController = Offer.OfferEditController;
    type TestCaseForRequiredValidator = [string, string, boolean]; // [field_description, field_selector, is_not_required]
    type TestCaseForValidator = [string, any, string]; // [field_description, field_value, field_selector]

    describe('Given offer component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            $compileService: ng.ICompileService,
            controller: OfferEditController;

        var pageObjectSelectors: any = {
            offerDate: '[name=offerDate]',
            offer: '[name=price]',
            exchangeDate: '[name=exchangeDate]',
            completionDate: '[name=completionDate]',
            specialConditions: '[name=specialConditions]',
            status: '[name="selectedStatus"]',
            activity: '#offer-edit-activity-details .card-item',
            negotiator: '#offer-edit-negotiator-details',
            negotiatorSection: '#offer-edit-negotiator-section'
        };

        var offerMock = TestHelpers.OfferGenerator.generate();
        offerMock.activity.property.address.propertyName = "West Forum";
        offerMock.activity.property.address.propertyNumber = "142a";
        offerMock.activity.property.address.line2 = "Strzegomska";
        offerMock.negotiator.firstName = "John";
        offerMock.negotiator.lastName = "Smith";
        offerMock.statusId = "2";

        var offerStatuses: any = [
            { id: '1', code: 'New' },
            { id: '2', code: 'Closed' }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            enumService: Mock.EnumServiceMock) => {

            $http = $httpBackend;
            $compileService = $compile;
            scope = $rootScope.$new();
            scope["offer"] = offerMock;

            // enums
            enumService.setEnum(Dto.EnumTypeCode.OfferStatus.toString(), offerStatuses);

            element = $compileService('<offer-edit offer="offer"></offer-edit>')(scope);
            scope.$apply();

            controller = element.controller('offerEdit');
            assertValidator = new TestHelpers.AssertValidators(element, scope);
            scope.$apply();

        }));

        it('all form elements are rendered', () => {
            for (var formElementSelector in pageObjectSelectors) {
                if (pageObjectSelectors.hasOwnProperty(formElementSelector)) {
                    var formElement = element.find(pageObjectSelectors[formElementSelector]);
                    expect(formElement.length).toBe(1, `Element ${formElementSelector} not found`);
                }
            }
        });

        it('then negotiator is displayed', () => {
            var negotiatorSection = element.find(pageObjectSelectors.negotiatorSection);
            expect(negotiatorSection.hasClass("ng-hide")).toBeFalsy();
        });

        it('and offer is set then all form elements have proper values', () => {
            controller.setSelectedStatus();
            scope.$apply();

            var activity = element.find(pageObjectSelectors.activity);
            var status = element.find(pageObjectSelectors.status);
            var offer = element.find(pageObjectSelectors.offer);
            var offerDate = element.find(pageObjectSelectors.offerDate);
            var specialConditions = element.find(pageObjectSelectors.specialConditions);
            var negotiator = element.find(pageObjectSelectors.negotiator);
            var exchangeDate = element.find(pageObjectSelectors.exchangeDate);
            var completionDate = element.find(pageObjectSelectors.completionDate);

            expect(activity.text()).toBe(offerMock.activity.property.address.getAddressText());
            expect(status.val()).toBe(offerMock.statusId);
            expect(offer.val()).toBe(offerMock.price.toString());
            expect(specialConditions.val()).toBe(offerMock.specialConditions);
            expect(negotiator.text()).toBe(offerMock.negotiator.getName());
            expect(offerDate.val()).toEqual(moment(offerMock.offerDate).format("DD-MM-YYYY"));
            expect(exchangeDate.val()).toEqual(moment(offerMock.exchangeDate).format("DD-MM-YYYY"));
            expect(completionDate.val()).toEqual(moment(offerMock.completionDate).format("DD-MM-YYYY"));
        });

        describe('when activity details is clicked', () => {
            it('then activity should be added to property activity list', () => {
                $http.expectPOST(/\/api\/latestviews/, () => {
                    return true;
                }).respond(200, []);

                controller.showActivityPreview(offerMock);

                expect($http.flush).not.toThrow();
            });
        });
    });
}