/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import CompanyEditController = Antares.Company.CompanyEditController;
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    describe('Given company is being edited', () => {
       
        var scope: ng.IScope,
            controller: CompanyEditController,
            element: ng.IAugmentedJQuery,
            assertValidator: TestHelpers.AssertValidators,
            $http: ng.IHttpBackendService,
            state: ng.ui.IStateService;

        var pageObjectSelectors = {
            nameSelector: 'input#name',
            companyCategory: "#category select",
            companyType: "#type select",
            description: "#description",
            clientCareStatus: "#client-care-status select",
            relationshipManagerPreview: "[name='vm.relationshipManager'] card .panel-item",

            noContactsHelpBlockSelector : 'p#no-contacts-help-block',
            contactsAreRequiredHelpBlockSelector : 'p#contacts-are-required-help-block',
            listItemsSelector : '#list-contacts list-item',
            listItemSelector : (contactId: string) =>{ return '#list-contacts list-item#list-item-contact-' + contactId; },
            companySaveBtnSelector : "button#company-save-btn",
            websiteSelector : "input#website",
            clientCarePageSelector : "input#clientcareurl"
        };

        var companyMock: Business.Company = null;

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            $state: ng.ui.IStateService) => {

            $http = $httpBackend;
            state = $state;

            companyMock = TestHelpers.CompanyGenerator.generateWithoutContacts();

            scope = $rootScope.$new();
            element = $compile('<company-edit company="company"></company-edit>')(scope);
            scope["company"] = companyMock;
            scope.$apply();

            controller = element.controller("companyEdit");

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        afterEach(() => {
            $http.verifyNoOutstandingExpectation();
        });

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
            assertValidator.assertElementHasHideClass(false, pageObjectSelectors.noContactsHelpBlockSelector);
            assertValidator.assertElementHasHideClass(true, pageObjectSelectors.contactsAreRequiredHelpBlockSelector);
        });

        it('when contacts are not selected and save button is clicked then only error message should be displayed', () =>{
            // arrange
            var button = element.find(pageObjectSelectors.companySaveBtnSelector);

            // act
            button.click();

            // asserts
            assertValidator.assertElementHasHideClass(true, pageObjectSelectors.noContactsHelpBlockSelector);
            assertValidator.assertElementHasHideClass(false, pageObjectSelectors.contactsAreRequiredHelpBlockSelector);
        });

        it('when displayed then current values should be shown', () => {
            // arrange
            var companyName = element.find(pageObjectSelectors.nameSelector).val();
            var companyCategory = element.find(pageObjectSelectors.companyCategory).val();
            var companyType = element.find(pageObjectSelectors.companyType).val();
            var description = element.find(pageObjectSelectors.description).val();
            var website = element.find(pageObjectSelectors.websiteSelector).val();
            var clientCareUrl = element.find(pageObjectSelectors.clientCarePageSelector).val();
            var clientCareStatus = element.find(pageObjectSelectors.clientCareStatus).val();
            var relationshipManager = element.find(pageObjectSelectors.relationshipManagerPreview).text();
            
            // asserts
            expect(companyName).toBe(controller.company.name);
            expect(companyCategory).toContain(controller.company.companyCategoryId);
            expect(companyType).toContain(controller.company.companyTypeId);
            expect(description).toBe(controller.company.description);
            expect(website).toBe(controller.company.websiteUrl);
            expect(clientCareUrl).toBe(controller.company.clientCarePageUrl);
            expect(clientCareStatus).toContain(controller.company.clientCareStatusId);
            expect(relationshipManager).toBe(controller.company.relationshipManager.getName());
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
            var requestData: Dto.IEditCompanyResource;
            var company = TestHelpers.CompanyGenerator.generate();
            var expectedContactsIds = company.contacts.map((contact: Dto.IContact) =>{ return { id : contact.id }; });

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
            expect(requestData.clientCareStatusId).toEqual(company.clientCareStatusId);
            expect(requestData.description).toEqual(company.description);
            expect(requestData.companyCategoryId).toEqual(company.companyCategoryId);
            expect(requestData.companyTypeId).toEqual(company.companyTypeId);
            expect(requestData.relationshipManagerId).toEqual(company.relationshipManager.id);
            expect(requestData.valid).toEqual(company.valid);
            expect(angular.equals(requestData.contacts, expectedContactsIds)).toBe(true);
        });

    });
}