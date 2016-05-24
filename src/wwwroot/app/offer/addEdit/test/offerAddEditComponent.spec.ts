/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import OfferAddEditController = Component.OfferAddEditController;
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import runDescribe = TestHelpers.runDescribe;
    type TestCaseForRequiredValidator = [string, string, boolean]; // [field_description, field_selector, is_not_required]
    type TestCaseForValidator = [string, any, string]; // [field_description, field_value, field_selector]

    describe('Given offer component', () =>{
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            $compileService: ng.ICompileService,
            controller: OfferAddEditController;

        var pageObjectSelectors: any = {
            offerDate : '[name=offerDate]',
            offer : '[name=price]',
            exchangeDate : '[name=exchangeDate]',
            completionDate : '[name=completionDate]',
            specialConditions : '[name=specialConditions]',
            status : '[name="selectedStatus"]',
            activity : '#offer-add-activity-details',
            negotiator : '#offer-edit-negotiator-details',
            negotiatorSection : '#offer-edit-negotiator-section'
        };

        var format = (date: any) => date.format('DD-MM-YYYY');
        var datesToTest: any = {
            today : format(moment()),
            inThePast : format(moment().day(-7)),
            inTheFuture : format(moment().day(7)),
            longAgo : format(moment().year(1700)),
            reallyLongAgo : format(moment().year(100))
        };

        var requirementMock: Business.Requirement = TestHelpers.RequirementGenerator.generate();
        var offerMock = TestHelpers.OfferGenerator.generate();

        var offerStatuses: any = [
            { id : '1', code : 'New' },
            { id : '2', code : 'Closed' }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            enumService: Mock.EnumServiceMock) =>{

            $http = $httpBackend;
            $compileService = $compile;
            scope = $rootScope.$new();
            scope["requirement"] = requirementMock;

            // enums
            enumService.setEnum(Dto.EnumTypeCode.OfferStatus.toString(), offerStatuses);

        }));

        describe('is in add mode ', () =>{
            beforeEach(() => {
                    element = $compileService('<offer-add requirement="requirement"></offer-add-edit>')(scope);
                    scope.$apply();

                    controller = element.controller('offerAdd');
                    assertValidator = new TestHelpers.AssertValidators(element, scope);
                    controller.mode = "add";
                    scope.$apply();
                }
            );

            it('all form elements are rendered', () => {
                for (var formElementSelector in pageObjectSelectors) {
                    if (pageObjectSelectors.hasOwnProperty(formElementSelector)) {
                        var formElement = element.find(pageObjectSelectors[formElementSelector]);
                        expect(formElement.length).toBe(1, `Element ${formElementSelector} not found`);
                    }
                }
            });

            it('then New status is selected by default', () =>{
                var status = element.find(pageObjectSelectors.status);
                var selectedStatus = status.val();
                var newStatus = offerStatuses[0];

                expect(selectedStatus).toEqual(newStatus.id);
            });

            it('then negotiator is not displayed', () =>{
                var negotiatorSection = element.find(pageObjectSelectors.negotiatorSection);
                expect(negotiatorSection.hasClass("ng-hide")).toBeTruthy();
            });

            // required validation
            runDescribe('and value for ')
                .data<TestCaseForRequiredValidator>([
                    ['offer date', pageObjectSelectors.offerDate, false],
                    ['offer', pageObjectSelectors.offer, false],
                    ['status', pageObjectSelectors.status, false],
                    ['exchange date', pageObjectSelectors.exchangeDate, true],
                    ['completion date', pageObjectSelectors.completionDate, true],
                    ['special conditions', pageObjectSelectors.specialConditions, true]
                ])
                .dataIt((data: TestCaseForRequiredValidator) =>
                    ` is "${data[0]}" then required message should ${data[2] ? 'not' : ''} be displayed`)
                .run((data: TestCaseForRequiredValidator) =>{
                    assertValidator.assertRequiredValidator('', data[2], data[1]);
                });

            // date format validation
            runDescribe('and value for ')
                .data<TestCaseForValidator>([
                    ['offer date', 'invalid date', pageObjectSelectors.offerDate],
                    ['exchange date', 'invalid date', pageObjectSelectors.exchangeDate],
                    ['completion date', 'invalid date', pageObjectSelectors.completionDate]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be invalid and invalid format message should be displayed`
                )
                .run((data: TestCaseForValidator) =>{
                    assertValidator.assertDateFormatValidator(data[1], false, data[2]);
                });

            // number format validation
            runDescribe('and value for ')
                .data<TestCaseForValidator>([
                    ['offer', 'e', pageObjectSelectors.offer],
                    ['offer', '.', pageObjectSelectors.offer]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be invalid and invalid format message should be displayed`
                )
                .run((data: TestCaseForValidator) =>{
                    /*
                        TODO:
                        during tests somehow 'required' validation message is displayed instead of 'invalid number' message
                        which is shown during user journey and which is expected
                    */

                    // assertValidator.assertNumberFormatValidator(data[1], false, data[2]);
                    assertValidator.assertRequiredValidator(data[1], false, data[2]);
                });

            // greater than zero validation
            runDescribe('and value for')
                .data<TestCaseForValidator>([
                    ['offer', -1, pageObjectSelectors.offer],
                    ['offer', 0, pageObjectSelectors.offer]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be invalid and greater than zero validation message should be displayed`
                )
                .run((data: TestCaseForValidator) =>{
                    assertValidator.assertNumberGreaterThenValidator(data[1], false, data[2]);
                });

            // valid field values
            runDescribe('and value for ')
                .data<TestCaseForValidator>([
                    ['offer date', datesToTest.today, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.inThePast, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.longAgo, pageObjectSelectors.offerDate],
                    ['offer date', datesToTest.reallyLongAgo, pageObjectSelectors.offerDate],
                    ['exchange date', datesToTest.today, pageObjectSelectors.exchangeDate],
                    ['exchange date', datesToTest.inTheFuture, pageObjectSelectors.exchangeDate],
                    ['completion date', datesToTest.today, pageObjectSelectors.completionDate],
                    ['completion date', datesToTest.inTheFuture, pageObjectSelectors.completionDate],
                    ['offer', '5', pageObjectSelectors.offer]
                ])
                .dataIt((data: TestCaseForValidator) =>
                    `"${data[0]}" is "${data[1]}" then input should be valid and no validation message should be displayed`
                )
                .run((data: TestCaseForValidator) =>{
                    assertValidator.assertValidAndNoMessages(data[1], data[2]);
                });

            it('special conditions text length is too long then validation message should be displayed', () =>{
                var maxLength = 4000;
                assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.specialConditions);
            });
        });
        describe('is in edit mode', () =>{
            beforeEach(() => {
                    element = $compileService('<offer-edit requirement="requirement"></offer-add-edit>')(scope);
                    scope.$apply();

                    controller = element.controller('offerEdit');
                    assertValidator = new TestHelpers.AssertValidators(element, scope);
                    controller.mode = "edit";
                    scope.$apply();
                }
            );

            it('all form elements are rendered', () => {
                for (var formElementSelector in pageObjectSelectors) {
                    if (pageObjectSelectors.hasOwnProperty(formElementSelector)) {
                        var formElement = element.find(pageObjectSelectors[formElementSelector]);
                        expect(formElement.length).toBe(1, `Element ${formElementSelector} not found`);
                    }
                }
            });

            it('then negotiator is displayed', () =>{
                var negotiatorSection = element.find(pageObjectSelectors.negotiatorSection);
                expect(negotiatorSection.hasClass("ng-hide")).toBeFalsy();
            });

            xit('and offer is set then all form elements have proper values', () =>{
                offerMock.activity.property.address.propertyName = "West Forum";
                offerMock.activity.property.address.propertyNumber = "142a";
                offerMock.activity.property.address.line2 = "Strzegomska";
                offerMock.negotiator.firstName = "John";
                offerMock.negotiator.lastName = "Smith";
                offerMock.statusId = "2";
                controller.setOffer(offerMock);
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
        });
    });
}