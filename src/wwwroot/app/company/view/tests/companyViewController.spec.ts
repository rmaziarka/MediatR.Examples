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

        var pageObjectSelectors = {
            nameSelector : 'div#name',
            listItemsSelector : '#list-contacts list-item',
            listItemSelector: (contactId: string) => { return '#list-contacts list-item#list-item-contact-' + contactId; }
        };

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

        it('when contacts exists then should be displayed', () =>{
          
            // asserts
            var listItems = element.find(pageObjectSelectors.listItemsSelector);
            expect(listItems.length).toBe(controller.company.contacts.length);

            _.forEach(controller.company.contacts, (contact: Business.Contact, key: any) =>{
                var listItem = element.find(pageObjectSelectors.listItemSelector(contact.id));
                expect(listItem.text()).toBe(contact.getName());
            });
        });

        it('when displyed then should have correct details shown', () =>{
            // arrange
            var companyName = element.find(pageObjectSelectors.nameSelector);
          
            // asserts
            expect(companyName.html()).toContain(controller.company.name);
     
        });

    });
}