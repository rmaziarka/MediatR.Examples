/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import CompanyViewController = Company.CompanyViewController;
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    describe('Given company is being viewed', () =>{
        var scope: ng.IScope,
            controller: CompanyViewController,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService,
            companyMock: Dto.ICompany;

        const dynamicTranslationsKey = "DYNAMICTRANSLATIONS.";

        var pageObjectSelectors = {
            nameSelector: 'div#name',
            companyCategory: "#company-category",
            companyType: "#company-type",
            description: "#description",
            website: "#website-url a",
            clientCareUrl: "#client-care-page-url a",
            clientCareStatus: "#client-care-status",
            relationshipManager: "#relationship-manager",

            websiteEmptySign: "#website-url .ng-binding",
            clientCareUrlEmptySign: "#client-care-page-url .ng-binding",

            listItemsSelector : '#list-contacts list-item',
            listItemSelector: (contactId: string) => { return '#list-contacts list-item#list-item-contact-' + contactId; }
        };

        describe('with all values', () =>{
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {
                $http = $httpBackend;

                companyMock = TestHelpers.CompanyGenerator.generate();

                scope = $rootScope.$new();
                scope['company'] = companyMock;
                element = $compile('<company-view company="company"></company-view>')(scope);
                scope.$apply();

                controller = element.controller("companyView");
            }));

            it('when contacts exists then should be displayed', () => {
                // asserts
                var listItems = element.find(pageObjectSelectors.listItemsSelector);
                expect(listItems.length).toBe(controller.company.contacts.length);

                _.forEach(controller.company.contacts, (contact: Business.Contact, key: any) => {
                    var listItem = element.find(pageObjectSelectors.listItemSelector(contact.id));
                    expect(listItem.text()).toBe(contact.getName());
                });
            });

            it('when displayed then should have correct details shown', () => {
                // arrange
                var companyName = element.find(pageObjectSelectors.nameSelector).text().trim();
                var companyCategory = element.find(pageObjectSelectors.companyCategory).text().trim();
                var companyType = element.find(pageObjectSelectors.companyType).text().trim();
                var description = element.find(pageObjectSelectors.description).text().trim();
                var website = element.find(pageObjectSelectors.website).text().trim();
                var clientCareUrl = element.find(pageObjectSelectors.clientCareUrl).text().trim();
                var clientCareStatus = element.find(pageObjectSelectors.clientCareStatus).text().trim();
                var relationshipManager = element.find(pageObjectSelectors.relationshipManager).text().trim();

                // asserts
                expect(companyName).toBe(controller.company.name);
                expect(companyCategory).toBe(dynamicTranslationsKey + controller.company.companyCategoryId);
                expect(companyType).toBe(dynamicTranslationsKey + controller.company.companyTypeId);
                expect(description).toBe(controller.company.description);
                expect(website).toBe(controller.company.websiteUrl);
                expect(clientCareUrl).toBe(controller.company.clientCarePageUrl);
                expect(clientCareStatus).toBe(dynamicTranslationsKey + controller.company.clientCareStatusId);
                expect(relationshipManager).toBe(controller.company.relationshipManager.getName());
            });
        });

        describe('with only mandatory values', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $controller: any,
                $compile: ng.ICompileService,
                $httpBackend: ng.IHttpBackendService) => {
                $http = $httpBackend;

                companyMock = new Business.Company();
                companyMock.name = "testName";
                companyMock.contacts = TestHelpers.ContactGenerator.generateMany(3);

                scope = $rootScope.$new();
                scope['company'] = companyMock;
                element = $compile('<company-view company="company"></company-view>')(scope);
                scope.$apply();

                controller = element.controller("companyView");
            }));
            
            it('than non-mandatory fields are empty with "-"', () => {
                // arrange
                var companyName = element.find(pageObjectSelectors.nameSelector).text().trim();
                var companyCategory = element.find(pageObjectSelectors.companyCategory).text().trim();
                var companyType = element.find(pageObjectSelectors.companyType).text().trim();
                var description = element.find(pageObjectSelectors.description).text().trim();
                var website = element.find(pageObjectSelectors.websiteEmptySign).text().trim();
                var clientCareUrl = element.find(pageObjectSelectors.clientCareUrlEmptySign).text().trim();
                var clientCareStatus = element.find(pageObjectSelectors.clientCareStatus).text().trim();
                var relationshipManager = element.find(pageObjectSelectors.relationshipManager).text().trim();

                // asserts
                expect(companyName).toBe(controller.company.name);
                expect(companyCategory).toBe("-");
                expect(companyType).toBe("-");
                expect(description).toBe("-");
                expect(website).toBe("-");
                expect(clientCareUrl).toBe("-");
                expect(clientCareStatus).toBe("-");
                expect(relationshipManager).toBe("-");
            });
        });
    });
}