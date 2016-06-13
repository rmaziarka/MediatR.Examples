/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import OfferEditController = Offer.OfferEditController;
    import KfMessageService = Services.KfMessageService;
    type TestCaseForRequiredValidator = [string, string, boolean]; // [field_description, field_selector, is_not_required]
    type TestCaseForValidator = [string, any, string]; // [field_description, field_value, field_selector]

    describe('Given offer component', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            state: ng.ui.IStateService,
            assertValidator: TestHelpers.AssertValidators,
            q: ng.IQService,
            $http: ng.IHttpBackendService,
            $compileService: ng.ICompileService,
            messageService: KfMessageService,
            controller: OfferEditController;

        var pageObjectSelectors: any = {
            offerDate: '[name=offerDate]',
            offer: '[name=price]',
            exchangeDate: '[name=exchangeDate]',
            completionDate: '[name=completionDate]',
            specialConditions: '[name=specialConditions]',
            status: 'select[name=selectedStatus]',
            activity: '#offer-edit-activity-details .card-item',
            negotiator: '#offer-edit-negotiator-details',
            negotiatorSection: '#offer-edit-negotiator-section'
        };

        var format = (date: any) => date.format('DD-MM-YYYY');
        var datesToTest: any = {
            today: format(moment()),
            inThePast: format(moment().day(-7)),
            inTheFuture: format(moment().day(7))
        };

        var offerMock = TestHelpers.OfferGenerator.generate();
        offerMock.activity.property.address.propertyName = "West Forum";
        offerMock.activity.property.address.propertyNumber = "142a";
        offerMock.activity.property.address.line2 = "Strzegomska";
        offerMock.negotiator.firstName = "John";
        offerMock.negotiator.lastName = "Smith";
        offerMock.statusId = "2";
        offerMock.createdDate = moment().toDate();

        var offerStatuses: any = [
            { id: '1', code: 'New' },
            { id: '2', code: 'Closed' }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            $q: ng.IQService,
            $state: ng.ui.IStateService,
            kfMessageService: KfMessageService,
            enumService: Mock.EnumServiceMock) => {

            state = $state;
            q = $q;
            messageService = kfMessageService;
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
            controller.setDefaultStatuses();
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

        describe('when fields are', () => {

            it('invalid then validation messages should appear', () => {
                assertValidator.assertDateGreaterThenValidator(datesToTest.inThePast, false, pageObjectSelectors.exchangeDate);
                assertValidator.assertDateGreaterThenValidator(datesToTest.inThePast, false, pageObjectSelectors.completionDate);
                assertValidator.assertDateLowerThenValidator(datesToTest.inTheFuture, false, pageObjectSelectors.offerDate);
                assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.status);
            });

            it('valid then validation messages should not appear', () => {
                assertValidator.assertDateGreaterThenValidator(datesToTest.inTheFuture, true, pageObjectSelectors.exchangeDate);
                assertValidator.assertDateGreaterThenValidator(datesToTest.inTheFuture, true, pageObjectSelectors.completionDate);
                assertValidator.assertDateLowerThenValidator(datesToTest.inThePast, true, pageObjectSelectors.offerDate);
                assertValidator.assertRequiredValidator("2", true, pageObjectSelectors.status);
            });
        });

        describe('when valid data and save button is clicked', () => {
            it('then save method is called', () => {
                spyOn(controller, 'save');
                scope.$apply();

                var button = element.find('button#offer-edit-save');
                button.click();

                expect(controller.save).toHaveBeenCalled();
            });

            it('then put request is called and redirect to view page with success message', () => {
                var requestData: any;
                $http.expectPUT(/\/api\/offers/, (data: string) => {
                    requestData = JSON.parse(data);

                    return true;
                }).respond(() => {
                    return [200, {}];
                });

                var stateDeffered = q.defer();
                spyOn(state, 'go').and.callFake((routeName: string, property: Business.Property) => {
                    return stateDeffered.promise;
                });

                spyOn(messageService, 'showSuccessByCode');

                stateDeffered.resolve();
                scope.$apply();

                var button = element.find('button#offer-edit-save');
                button.click();
                $http.flush();

                expect(state.go).toHaveBeenCalled();
                expect(messageService.showSuccessByCode).toHaveBeenCalledWith('OFFER.EDIT.OFFER_EDIT_SUCCESS');
            });
        });

    });
}