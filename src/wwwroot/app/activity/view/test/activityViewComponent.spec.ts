/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ActivityViewController = Activity.View.ActivityViewController;
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    describe('Given view activity page is loaded', () => {
        var scope: ng.IScope,
            compile: ng.ICompileService,
            element: ng.IAugmentedJQuery,
            filter: ng.IFilterService,
            $http: ng.IHttpBackendService,
            controller: ActivityViewController;

        var pageObjectSelectors = {
            common: {
                address: 'address-form-view',
                detailsLink: 'a'
            },
            well: {
                main: '#activity-view-well',
                createdDate: '#activity-view-well #createdDate',
                status: '#activity-view-well #activityStatus'
            },
            prices: {
                main: '#activity-view-prices',
                marketAppraisalPrice: '#activity-view-prices #marketAppraisalPrice',
                recommendedPrice: '#activity-view-prices #recommendedPrice',
                vendorEstimatedPrice: '#activity-view-prices #vendorEstimatedPrice'
            },
            property: {
                card: '#activity-view-property card#card-property'
            },
            vendorList: {
                list: "#activity-view-vendors list#list-vendors",
                header: '#activity-view-vendors list#list-vendors list-header',
                noItems: '#activity-view-vendors list#list-vendors list-no-items',
                items: '#activity-view-vendors list#list-vendors list-item',
                item: '#activity-view-vendors list#list-vendors list-item#list-item-'
            },
            propertyPreview: {
                main: 'property-preview',
                addressSection: '#property-preview-address'
            }
        };

        describe('when activity is loaded', () => {
            var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate();

            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $filter: ng.IFilterService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;
                filter = $filter;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                scope['activity'] = activityMock;
                element = $compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();

                controller = element.controller('activityView');
            }));

            it('then card component for property is set up', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.property.card);

                expect(cardElement.length).toBe(1);
                expect(cardElement[0].getAttribute('card-template-url')).toBe("'app/property/templates/propertyCard.html'");
                expect(cardElement[0].getAttribute('item')).toBe("avvm.activity.property");
                expect(cardElement[0].getAttribute('show-item-details')).toBe("avvm.showPropertyPreview");
            });

            it('then list component for vendors is set up', () => {
                // assert
                var listElement = element.find(pageObjectSelectors.vendorList.list),
                    listHeaderElement = element.find(pageObjectSelectors.vendorList.header),
                    listHeaderElementContent = listHeaderElement.find('[translate="ACTIVITY.VIEW.VENDORS"]'),
                    listNoItemsElement = element.find(pageObjectSelectors.vendorList.noItems),
                    listNoItemsElementContent = listNoItemsElement.find('[translate="ACTIVITY.VIEW.NO_VENDORS"]');

                expect(listElement.length).toBe(1);
                expect(listHeaderElement.length).toBe(1);
                expect(listHeaderElementContent.length).toBe(1);
                expect(listNoItemsElement.length).toBe(1);
                expect(listNoItemsElementContent.length).toBe(1);
            });

            it('then activity summary component for activity is displayed and should have proper data', () => {
                // assert
                var wellElement = element.find(pageObjectSelectors.well.main);
                var createdDateElement = element.find(pageObjectSelectors.well.createdDate);
                var activityStatusElement = element.find(pageObjectSelectors.well.status);

                var formattedDate = filter('date')(activityMock.createdDate, 'dd-MM-yyyy');
                expect(wellElement.length).toBe(1);
                expect(createdDateElement.length).toBe(1);
                expect(createdDateElement.text()).toBe(formattedDate);
                expect(activityStatusElement.length).toBe(1);
                expect(activityStatusElement.text()).toBe('ENUMS.' + activityMock.activityStatusId);
            });

            it('and valuation prices are null then valuation prices for activity are not displayed', () => {
                // arrange / act
                controller.activity.marketAppraisalPrice = null;
                controller.activity.recommendedPrice = null;
                controller.activity.vendorEstimatedPrice = null;

                // assert
                var pricesElement = element.find(pageObjectSelectors.prices.main);
                var marketAppraisalPriceElement = element.find(pageObjectSelectors.prices.marketAppraisalPrice);
                var recomendedPriceElement = element.find(pageObjectSelectors.prices.recommendedPrice);
                var vendorEstimatedPriceElement = element.find(pageObjectSelectors.prices.vendorEstimatedPrice);

                expect(pricesElement.length).toBe(1);
                expect(marketAppraisalPriceElement.length).toBe(0);
                expect(recomendedPriceElement.length).toBe(0);
                expect(vendorEstimatedPriceElement.length).toBe(0);
            });

            it('and valuation prices are not null then valuation prices for activity are displayed and should have proper data', () => {
                // arrange / act
                controller.activity.marketAppraisalPrice = 99;
                controller.activity.recommendedPrice = 1.1;
                controller.activity.vendorEstimatedPrice = 55.05;
                scope.$apply();

                // assert
                var pricesElement = element.find(pageObjectSelectors.prices.main);
                var marketAppraisalPriceElement = element.find(pageObjectSelectors.prices.marketAppraisalPrice);
                var recomendedPriceElement = element.find(pageObjectSelectors.prices.recommendedPrice);
                var vendorEstimatedPriceElement = element.find(pageObjectSelectors.prices.vendorEstimatedPrice);

                expect(pricesElement.length).toBe(1);
                expect(marketAppraisalPriceElement.length).toBe(1);
                expect(marketAppraisalPriceElement.text()).toBe('99.00 GBP');
                expect(recomendedPriceElement.length).toBe(1);
                expect(recomendedPriceElement.text()).toBe('1.10 GBP');
                expect(vendorEstimatedPriceElement.length).toBe(1);
                expect(vendorEstimatedPriceElement.text()).toBe('55.05 GBP');
            });
        });

        describe('and vendors are loaded', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;
            }));

            it('when no vendors then "no items" element should be visible', () => {
                // arrange
                var activityMock: Business.Activity = TestHelpers.ActivityGenerator.generate({ contacts: [] });
                scope['activity'] = activityMock;

                // act
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
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
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
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
                element = compile('<activity-view activity="activity"></activity-view>')(scope);
                scope.$apply();
                $http.flush();

                // assert
                var vendorsItemsElement1 = element.find(pageObjectSelectors.vendorList.item + contact1Mock.id);
                var vendorsItemsElement2 = element.find(pageObjectSelectors.vendorList.item + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText).toBe(contact2Mock.getName());
            });
        });

        describe('and property is loaded', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) =>{

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() =>{
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;

                scope['activity'] = TestHelpers.ActivityGenerator.generate({ contacts: [] });
                element = compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();
            }));

            it('when existing property then property card should have address element visible', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.property.card);
                var addressElement = cardElement.find(pageObjectSelectors.common.address);

                expect(addressElement.length).toBe(1);
                expect(addressElement[0].getAttribute('address')).toBe("cvm.item.address");
            });
        });

        describe('and property details is clicked', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {

                $http = $httpBackend;

                Antares.Mock.AddressForm.mockHttpResponce($http, 'a1', [200, Antares.Mock.AddressForm.AddressFormWithOneLine]);
                $http.whenGET(/\/api\/enums\/.*\/items/).respond(() => {
                    return [];
                });

                scope = $rootScope.$new();
                compile = $compile;

                scope['activity'] = TestHelpers.ActivityGenerator.generate({ contacts: [] });
                element = compile('<activity-view activity="activity"></activity-view>')(scope);

                scope.$apply();
                $http.flush();
            }));

            it('then property preview are visible on property preview panel', () => {
                // act
                var cardElement = element.find(pageObjectSelectors.property.card);
                cardElement.find(pageObjectSelectors.common.detailsLink).click();

                // assert
                var propertyPreviewPanel = element.find(pageObjectSelectors.propertyPreview.main);
                var addressElement = propertyPreviewPanel.find(pageObjectSelectors.propertyPreview.addressSection);
                var addressValueElement = addressElement.find(pageObjectSelectors.common.address);

                expect(addressElement.length).toBe(1);
                expect(addressValueElement.length).toBe(1);

                //TODO: test viewving property type when it is ready
                //var typeElement = propertyPreviewPanel.find('pageObjectSelectors.propertyPreview.type');
                //expect(typeElement.text()).toBe('ENUMS.456');
            });
        });
    });
}