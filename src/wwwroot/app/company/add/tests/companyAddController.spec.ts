/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import CompanyAddController = Antares.Company.CompanyAddController;
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;
    import TestHelpers = Antares.TestHelpers;

    describe('Given company is being added', () => {
        var scope: ng.IScope,
            controller: CompanyAddController,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService;

        var pageObjectSelectors = {
            nameSelector: 'input#name',
            noContactsHelpBlockSelector: 'p#no-contacts-help-block',
            contactsAreRequiredHelpBlockSelector: 'p#contacts-are-required-help-block',
            listItemsSelector: '#list-contacts list-item',
            listItemSelector: (contactId: string) => { return '#list-contacts list-item#list-item-contact-' + contactId; },
            companySaveBtnSelector: "button#company-save-btn",
            websiteSelector: 'input#website'
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {

            $http = $httpBackend;
            scope = $rootScope.$new();
            element = $compile('<company-add></company-add>')(scope);            
            scope.$apply();
            controller = element.controller("companyAdd");

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        it('when name value is missing then required message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.nameSelector);
        });

        it('when name value is present then required message should not be displayed', () => {
            assertValidator.assertRequiredValidator('nameValueMock', true, pageObjectSelectors.nameSelector);
        });

        it('when name value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.nameSelector);
        });

        it('when name value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.nameSelector);
        });

        it('when website value has max length then validation message should not be displayed', () => {
            var maxLength = 1024;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.websiteSelector);
        });

        it('when website value has more than max length then validation message should be displayed', () => {
            var maxLength = 1024;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.websiteSelector);
        });

        it('when contacts are not selected then only information message should be displayed', () => {
            assertValidator.assertShowElement(false, pageObjectSelectors.noContactsHelpBlockSelector);
            assertValidator.assertShowElement(true, pageObjectSelectors.contactsAreRequiredHelpBlockSelector);            
        });

        it('when contacts are not selected and save button is clicked then only error message should be displayed', () => {
            // arrange
            var button = element.find(pageObjectSelectors.companySaveBtnSelector);

            // act
            button.click();

            // asserts
            assertValidator.assertShowElement(true, pageObjectSelectors.noContactsHelpBlockSelector);
            assertValidator.assertShowElement(false, pageObjectSelectors.contactsAreRequiredHelpBlockSelector);    
        });
        
        it('when contacts exists then should be displayed', () => {
            // arrange                 
            controller.company = TestHelpers.CompanyGenerator.generate();
            
            // act
            scope.$apply();
            
            // asserts
            var listItems = element.find(pageObjectSelectors.listItemsSelector);            
            expect(listItems.length).toBe(controller.company.contacts.length);

            _.forEach(controller.company.contacts, (contact: Business.Contact, key: any) => {
                var listItem = element.find(pageObjectSelectors.listItemSelector(contact.id));
                expect(listItem.text()).toBe(contact.getName());
            });
        });
        
        it('when form filled and save then should be send data', () => {
            // arrange
            var button = element.find(pageObjectSelectors.companySaveBtnSelector);
            var requestData: Dto.ICreateCompanyResource;
            var company = TestHelpers.CompanyGenerator.generate();            
            var expectedContactIds = company.contacts.map((contact: Dto.IContact) => { return contact.id });

            controller.company = company;

            $http.expectPOST(/\/api\/companies/, (data: string) => {
                requestData = JSON.parse(data);
                return true;
            }).respond(201, new Business.Company());

            // act
            scope.$apply();
            button.click();
            $http.flush();

            // asserts
            expect(requestData).toBeDefined();
            expect(requestData.name).toEqual(company.name);            
            expect(angular.equals(requestData.contactIds, expectedContactIds)).toBe(true);            
        });

    });
}