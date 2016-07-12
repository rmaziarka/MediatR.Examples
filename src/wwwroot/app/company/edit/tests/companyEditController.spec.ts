/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import CompanyEditController = Antares.Company.CompanyEditController;
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    describe('Given company is being edited', () => {
       
        var scope: ng.IScope,
            controller: CompanyEditController,
            element: ng.IAugmentedJQuery,
            assertValidator: Antares.TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            state: ng.ui.IStateService;

        var pageObjectSelectors = {
            nameSelector : 'input#name',
            noContactsHelpBlockSelector : 'p#no-contacts-help-block',
            contactsAreRequiredHelpBlockSelector : 'p#contacts-are-required-help-block',
            listItemsSelector : '#list-contacts list-item',
            listItemSelector : (contactId: string) =>{ return '#list-contacts list-item#list-item-contact-' + contactId; },
            companySaveBtnSelector : "button#company-save-btn",
            websiteSelector : "input#website",
            clientCarePageSelector : "input#clientcareurl"
        };

        var companyMock: Business.Company = TestHelpers.CompanyGenerator.generate();

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            $state: ng.ui.IStateService) => {

            $http = $httpBackend;
            state = $state;
            scope = $rootScope.$new();
            element = $compile('<company-edit company="company"></company-edit>')(scope);
            scope["company"] = companyMock;
            scope.$apply();

            controller = element.controller("companyEdit");

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }));

        it('when name value is missing then required message should be displayed', () =>{
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.nameSelector);
           
        });

        it('when name value is present then required message should not be displayed', () =>{
            assertValidator.assertRequiredValidator('nameValueMock', true, pageObjectSelectors.nameSelector);
        });

        it('when name value is too long then validation message should be displayed', () =>{
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.nameSelector);
        });

        it('when name value has max length then validation message should not be displayed', () =>{
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.nameSelector);
        });

        it('when website value has max length then validation message should not be displayed', () =>{
            var maxLength = 2500;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.websiteSelector);
        });

        it('when website value has more than max length then validation message should be displayed', () =>{
            var maxLength = 2500;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.websiteSelector);
        });

        it('when client care page url value has max length then validation message should not be displayed', () =>{
            var maxLength = 2500;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.clientCarePageSelector);
        });

        it('when client care page url value has more than max length then validation message should be displayed', () =>{
            var maxLength = 2500;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.clientCarePageSelector);
        });

        it('when contacts are not selected then only information message should be displayed', () =>{
            assertValidator.assertShowElement(false, pageObjectSelectors.noContactsHelpBlockSelector);
            assertValidator.assertShowElement(true, pageObjectSelectors.contactsAreRequiredHelpBlockSelector);
        });

        it('when contacts are not selected and save button is clicked then only error message should be displayed', () =>{
            // arrange
            var button = element.find(pageObjectSelectors.companySaveBtnSelector);

            // act
            button.click();

            // asserts
            assertValidator.assertShowElement(true, pageObjectSelectors.noContactsHelpBlockSelector);
            assertValidator.assertShowElement(false, pageObjectSelectors.contactsAreRequiredHelpBlockSelector);
        });

        it('when contacts exists then should be displayed', () =>{
            // arrange                 
            controller.company = TestHelpers.CompanyGenerator.generate();

            // act
            scope.$apply();

            // asserts
            var listItems = element.find(pageObjectSelectors.listItemsSelector);
            expect(listItems.length).toBe(controller.company.contacts.length);

            _.forEach(controller.company.contacts, (contact: Business.Contact, key: any) =>{
                var listItem = element.find(pageObjectSelectors.listItemSelector(contact.id));
                expect(listItem.text()).toBe(contact.getName());
            });
        });

        it('when form filled and save then should be send data', () =>{
            // arrange
            var button = element.find(pageObjectSelectors.companySaveBtnSelector);
            var requestData: Dto.ICompany;
            var company = TestHelpers.CompanyGenerator.generate();
            var expectedContacts = company.contacts.map((contact: Dto.IContact) =>{ return contact });

            spyOn(state, 'go');
            controller.company = company;
           
            $http.expectPUT(/\/api\/companies/, (data: string) =>{
                requestData = JSON.parse(data);
                return true;
            }).respond(200, new Business.Company());

            // act
            scope.$apply();
            controller.updateCompany();
            $http.flush();

            // asserts
            expect(state.go).toHaveBeenCalled();
            expect(requestData).toBeDefined();
            expect(requestData.name).toEqual(company.name);
            expect(requestData.websiteUrl).toEqual(company.websiteUrl);
            expect(requestData.clientCarePageUrl).toEqual(company.clientCarePageUrl);
            expect(requestData.clientCareStatusId).toEqual(company.clientCareStatus.id);
            expect(angular.equals(requestData.contacts, expectedContacts)).toBe(true);
        });

    });
}