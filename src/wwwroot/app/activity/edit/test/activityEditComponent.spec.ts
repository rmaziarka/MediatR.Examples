/// <reference path="../../../typings/_all.d.ts" />
module Antares {
    import Business = Common.Models.Business;
    import Dto = Common.Models.Dto;
    import ActivityEditController = Activity.ActivityEditController;
    import Enums = Common.Models.Enums;
    
    describe('Given edit activity page is loaded', () => {
        beforeEach(() => {
            angular.mock.module(($provide: any) => {
                $provide.service('addressFormsProvider', Mock.AddressFormsProviderMock);
            });
        });

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

        xdescribe('when vendors are loaded', () => {
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

                // assert
                var vendorsItemsElement1 = element.find(pageObjectSelectors.vendorList.item + contact1Mock.id);
                var vendorsItemsElement2 = element.find(pageObjectSelectors.vendorList.item + contact2Mock.id);

                expect(vendorsItemsElement1[0].innerText.trim()).toBe(contact1Mock.getName());
                expect(vendorsItemsElement2[0].innerText.trim()).toBe(contact2Mock.getName());
            });
        });

        xdescribe('when property is loaded', () => {
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

                // compile
                scope['activity'] = TestHelpers.ActivityGenerator.generate();
                element = compile('<activity-edit activity="activity"></activity-edit>')(scope);

                scope.$apply();
            }));

            it('then card component should have address data', () => {
                // assert
                var cardElement = element.find(pageObjectSelectors.property.card);
                var addressElement = cardElement.find(pageObjectSelectors.common.address);

                expect(addressElement.length).toBe(1);
            });
        });

    });
}