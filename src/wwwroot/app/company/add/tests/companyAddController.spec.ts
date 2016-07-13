/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import CompanyAddController = Antares.Company.CompanyAddController;
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    describe('Given company is being added', () =>{
        var scope: ng.IScope,
            controller: CompanyAddController,
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

        const enumProviderMockedValues = <Dto.IEnumDictionary>{
            companyCategory: [{ id: "123", "code": "big" }],
            companyType: [{ id: "1234", "code": "red" }]
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            $state: ng.ui.IStateService,
            enumProvider: Providers.EnumProvider) => {

            $http = $httpBackend;
            state = $state;

            enumProvider.enums = enumProviderMockedValues;

            scope = $rootScope.$new();
            element = $compile('<company-add></company-add>')(scope);
            scope.$apply();
            controller = element.controller("companyAdd");

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
            var requestData: Dto.ICreateCompanyResource;
            var company = TestHelpers.CompanyGenerator.generate();
            var expectedContactIds = company.contacts.map((contact: Dto.IContact) =>{ return contact.id });

            spyOn(state, 'go');
            controller.company = company;

            $http.expectPOST(/\/api\/companies/, (data: string) =>{
                requestData = JSON.parse(data);
                return true;
            }).respond(200, new Business.Company());

            // act
            scope.$apply();
            controller.createCompany();
            $http.flush();

            // asserts
            expect(state.go).toHaveBeenCalled();
            expect(requestData).toBeDefined();
            expect(requestData.name).toEqual(company.name);
            expect(requestData.websiteUrl).toEqual(company.websiteUrl);
            expect(requestData.clientCarePageUrl).toEqual(company.clientCarePageUrl);
            expect(requestData.clientCareStatusId).toEqual(company.clientCareStatusId);
            expect(angular.equals(requestData.contactIds, expectedContactIds)).toBe(true);
        });

    });
}