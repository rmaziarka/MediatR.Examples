/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import ActivityEditController = Activity.ActivityEditController;
    import Enums = Common.Models.Enums;

    describe('Given edit activity page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            state: ng.ui.IStateService,
            assertValidator: TestHelpers.AssertValidators,
            controller: ActivityEditController;

        var pageObjectSelectors = {
            common: {
                address: 'address-form-view'
            },
            basic: {
                status: '#activity-edit-basic #activityStatus select'
            },
            prices: {
                main: '#activity-edit-prices',
                marketAppraisalPrice: '#market-appraisal-price',
                recomendedPrice: '#recommended-price',
                vendorEstimatedPrice: '#vendor-estimated-price'
            },
            property: {
                card: '#activity-edit-property card#card-property'
            },
            vendorList: {
                list: "#activity-edit-vendors list#list-vendors",
                header: '#activity-edit-vendors list#list-vendors list-header',
                noItems: '#activity-edit-vendors list#list-vendors list-no-items',
                items: '#activity-edit-vendors list#list-vendors list-item',
                item: '#activity-edit-vendors list#list-vendors list-item#list-item-'
            },
            negotiators: {
                component: '#activity-edit-negotiators negotiators-edit',
                items: '#activity-edit-negotiators card[id^="card-secondary-negotiator"]'
            },
            actions: {
                save: 'button#activity-edit-save',
                cancel: 'button#activity-edit-cancel'
            }
        };

        var activityStatuses = [
            { id: "111", code: "PreAppraisal" },
            { id: "testStatus222", code: "MarketAppraisal" },
            { id: "333", code: "NotSelling" }
        ];

        describe('when activity is loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({
                activityStatusId: activityStatuses[1].id,
                marketAppraisalPrice: 99,
                recommendedPrice: 1.1,
                vendorEstimatedPrice: 55.05,
                activityUsers: TestHelpers.ActivityUserGenerator.generateManyDtos(3, Enums.NegotiatorTypeEnum.SecondaryNegotiator)
            });

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                enumService: Mock.EnumServiceMock,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                // enums
                enumService.setEnum(Dto.EnumTypeCode.ActivityStatus.toString(), activityStatuses);

                // compile
                scope['activity'] = activityMock;
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            describe('and has data', () => {
                it('then card component for property is set up', () => {
                    // assert
                    var propertyCardElement = element.find(pageObjectSelectors.property.card);

                    expect(propertyCardElement.length).toBe(1);
                    expect(propertyCardElement[0].getAttribute('item')).toBe("aevm.activity.property");
                    expect(propertyCardElement[0].hasAttribute('show-item-details')).toBe(false);
                });

                it('then list component for vendors is set up', () => {
                    // assert
                    var listElement = element.find(pageObjectSelectors.vendorList.list),
                        listHeaderElement = element.find(pageObjectSelectors.vendorList.header),
                        listHeaderElementContent = listHeaderElement.find('[translate="ACTIVITY.EDIT.VENDORS"]'),
                        listNoItemsElement = element.find(pageObjectSelectors.vendorList.noItems),
                        listNoItemsElementContent = listNoItemsElement.find('[translate="ACTIVITY.EDIT.NO_VENDORS"]');

                    expect(listElement.length).toBe(1);
                    expect(listHeaderElement.length).toBe(1);
                    expect(listHeaderElementContent.length).toBe(1);
                    expect(listNoItemsElement.length).toBe(1);
                    expect(listNoItemsElementContent.length).toBe(1);
                });

                it('then negotiators edit component is set up', () => {
                    var component = element.find(pageObjectSelectors.negotiators.component);
                    var items = element.find(pageObjectSelectors.negotiators.items);
                    expect(component.length).toBe(1);
                    expect(items.length).toBe(3);
                });

                it('then status for activity is displayed and should have proper data', () => {
                    // assert
                    var activityStatusElement = element.find(pageObjectSelectors.basic.status);
                    var activityStatusSelectedElement = activityStatusElement.find("option:selected");

                    expect(activityStatusElement.length).toBe(1);
                    expect(activityStatusSelectedElement.val()).toBe(activityMock.activityStatusId);
                });

                it('then valuation prices for activity are displayed and should have proper data', () => {
                    // assert
                    var pricesElement = element.find(pageObjectSelectors.prices.main);
                    var marketAppraisalPriceElement = pricesElement.find(pageObjectSelectors.prices.marketAppraisalPrice);
                    var recomendedPriceElement = pricesElement.find(pageObjectSelectors.prices.recomendedPrice);
                    var vendorEstimatedPriceElement = pricesElement.find(pageObjectSelectors.prices.vendorEstimatedPrice);

                    expect(pricesElement.length).toBe(1);
                    expect(marketAppraisalPriceElement.length).toBe(1);
                    expect(marketAppraisalPriceElement.val()).toBe(activityMock.marketAppraisalPrice.toString());
                    expect(recomendedPriceElement.length).toBe(1);
                    expect(recomendedPriceElement.val()).toBe(activityMock.recommendedPrice.toString());
                    expect(vendorEstimatedPriceElement.length).toBe(1);
                    expect(vendorEstimatedPriceElement.val()).toBe(activityMock.vendorEstimatedPrice.toString());
                });
            });
        });

        describe('when vendors are loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);
            }));

            it('when no vendors then "no items" element should be visible', () => {
                // arrange
                var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({ contacts: [] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find(pageObjectSelectors.vendorList.noItems);
                var listItemElements = element.find(pageObjectSelectors.vendorList.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeFalsy();
                expect(listItemElements.length).toBe(0);
            });

            it('when existing vendors then list item components should be visible', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
                var activityMock = TestHelpers.ActivityGenerator.generate({ contacts: [contact1Mock, contact2Mock] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var noItemsElement = element.find(pageObjectSelectors.vendorList.noItems);
                var listItemElements = element.find(pageObjectSelectors.vendorList.items);

                expect(noItemsElement.hasClass('ng-hide')).toBeTruthy();
                expect(listItemElements.length).toBe(2);
            });

            it('when existing vendors then list item components should have proper data', () => {
                // arrange
                var contact1Mock = TestHelpers.ContactGenerator.generate();
                var contact2Mock = TestHelpers.ContactGenerator.generate();
                var activityMock = TestHelpers.ActivityGenerator.generate({ contacts: [contact1Mock, contact2Mock] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var vendorsItemsElement1 = element.find(pageObjectSelectors.vendorList.item + contact1Mock.id);
                var vendorsItemsElement2 = element.find(pageObjectSelectors.vendorList.item + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText.trim()).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText.trim()).toBe(contact2Mock.getName());
            });
        });

        describe('when property is loaded', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                // compile
                scope['activity'] = TestHelpers.ActivityGenerator.generate();
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);

                scope.$apply();
                $httpBackend.flush();
            }));

            it('then card component should have address data', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.property.card);
                var addressElement = cardElement.find(pageObjectSelectors.common.address);

                expect(addressElement.length).toBe(1);
            });
        });

        describe('when valid data and price values are being filled', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                // compile
                scope['activity'] = TestHelpers.ActivityGenerator.generate();
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);

                scope.$apply();
                $httpBackend.flush();

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            // Price Validations
            it('when marketAppraisalPrice value is lower than minimum value then validation message should be displayed', () => {
                // act / assert
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.prices.marketAppraisalPrice);
            });

            it('when marketAppraisalPrice value is not lower than minimum value then validation message should not be displayed', () => {
                // act / assert
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.prices.marketAppraisalPrice);
            });

            it('when marketAppraisalPrice value is empty then validation message should not be displayed', () => {
                // act / assert
                assertValidator.assertRequiredValidator(null, true, pageObjectSelectors.prices.marketAppraisalPrice);
            });

            it('when recomendedPrice value is lower than minimum value then validation message should be displayed', () => {
                // act / assert
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.prices.recomendedPrice);
            });

            it('when recomendedPrice value is not lower than minimum value then validation message should not be displayed', () => {
                // act / assert
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.prices.recomendedPrice);
            });

            it('when recomendedPrice value is empty then validation message should not be displayed', () => {
                // act / assert
                assertValidator.assertRequiredValidator(null, true, pageObjectSelectors.prices.recomendedPrice);
            });

            it('when vendorEstimatedPrice value is lower than minimum value then validation message should be displayed', () => {
                // act / assert
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue - 1, false, pageObjectSelectors.prices.vendorEstimatedPrice);
            });

            it('when vendorEstimatedPrice value is not lower than minimum value then validation message should not be displayed', () => {
                // act / assert
                var minValue = 0;
                assertValidator.assertMinValueValidator(minValue, true, pageObjectSelectors.prices.vendorEstimatedPrice);
            });

            it('when vendorEstimatedPrice value is empty then validation message should not be displayed', () => {
                // act / assert
                assertValidator.assertRequiredValidator(null, true, pageObjectSelectors.prices.vendorEstimatedPrice);
            });
        });

        describe('when form action is called', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $state: ng.ui.IStateService,
                $httpBackend: ng.IHttpBackendService) => {

                // init
                scope = $rootScope.$new();
                compile = $compile;
                state = $state;
                $http = $httpBackend;

                // http backend
                Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Mock.AddressForm.AddressFormWithOneLine]);

                // compile
                scope['activity'] = activityMock;
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);

                scope.$apply();
                $httpBackend.flush();

                controller = element.controller('activityEdit');
            }));

            describe('when valid data is provided and save button is clicked', () => {
                it('then save method is called', () => {
                    // arrange
                    spyOn(controller, 'save');

                    // act
                    var button = element.find(pageObjectSelectors.actions.save);
                    button.click();

                    // assert
                    expect(controller.save).toHaveBeenCalled();
                });

                it('then put request is is called with valid data and redirect to view page', () => {
                    // arrange
                    var activityId: string;
                    var requestData: Dto.IUpdateActivityResource;
                    var activityFromServerMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

                    activityMock.secondaryNegotiator = TestHelpers.ActivityUserGenerator.generateMany(3, Enums.NegotiatorTypeEnum.SecondaryNegotiator);

                    spyOn(state, 'go').and.callFake((routeName: string, activity: Business.Activity) => {
                        activityId = activity.id;
                    });

                    $http.expectPUT(/\/api\/activities/, (data: string) => {
                        requestData = JSON.parse(data);
                        return true;
                    }).respond(201, activityFromServerMock);

                    // act
                    var button = element.find(pageObjectSelectors.actions.save);
                    button.click();
                    $http.flush();

                    // assert
                    expect(state.go).toHaveBeenCalled();
                    expect(requestData.id).toEqual(activityMock.id);
                    expect(requestData.activityStatusId).toEqual(activityMock.activityStatusId);
                    expect(requestData.marketAppraisalPrice).toEqual(activityMock.marketAppraisalPrice);
                    expect(requestData.recommendedPrice).toEqual(activityMock.recommendedPrice);
                    expect(requestData.vendorEstimatedPrice).toEqual(activityMock.vendorEstimatedPrice);
                    expect(requestData.leadNegotiator.id).toEqual(activityMock.leadNegotiator.userId);
                    expect(requestData.secondaryNegotiators.map((negotiator) => negotiator.id)).toEqual(activityMock.secondaryNegotiator.map((negotiator) => negotiator.userId));

                    expect(activityId).toEqual(activityFromServerMock.id);
                });
            });

            describe('when cancel action is called', () => {
                it('then redrection to activity view page should be triggered', () => {
                    // arrange
                    var cancelBtn = element.find(pageObjectSelectors.actions.cancel);
                    spyOn(state, 'go');

                    // act
                    cancelBtn.click();

                    // assert
                    expect(state.go).toHaveBeenCalledWith('app.activity-view', { id: activityMock.id });
                });
            });

        });
    });
}