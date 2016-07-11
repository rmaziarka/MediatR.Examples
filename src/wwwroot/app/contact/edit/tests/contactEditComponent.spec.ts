/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import Business = Common.Models.Business;
    import ContactEditController = Contact.ContactEditController;

    describe('Given contactEdit component', () =>{
        var element: ng.IAugmentedJQuery;
        var assertValidator: TestHelpers.AssertValidators;
        var controller: ContactEditController;

        var pageObjectSelectors: any = {
            basicInformationSectionHeader: ".page-header[translate='CONTACT.BASIC_INFORMATION']",
            titleControl: '#title-section input',
            firstNameInput: '#first-name',
            lastNameInput: '#last-name',
            cancelButton: '#cancelBtn',
            saveButton: '#saveBtn'
        };
        
        var contactMock: Business.Contact = TestHelpers.ContactGenerator.generate();
       
        beforeEach(angular.mock.module(($compileProvider: ng.ICompileProvider) => {
            $compileProvider.directive('editSalutations', () => {
                return {
                    priority: 1000,
                    terminal: true,
                    template: '<div class="mock editSalutations"></div>'
                };
            });

            $compileProvider.directive('contactNegotiatorsEditControl', () => {
                return {
                    priority: 1000,
                    terminal: true,
                    template: '<div class="mock contactNegotiatorsEditControl"></div>'
                };
            });
        }));

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            $state: ng.ui.IStateService) => {

            var scope = $rootScope.$new();
            scope["contact"] = contactMock;
            scope["userData"] = TestHelpers.CurrentUserGenerator.generateDto({
                salutationFormatId: 123
            });
            
            spyOn($state, 'go').and.callFake((state: any, params: any) => {
                return {
                    then() { }
                }
            });

            $httpBackend.whenGET(/\/api\/contacts\/titles/).respond(() => {
                return [200, {}];
            });
            
            element = $compile('<contact-edit contact="contact" user-data="userData"></contact-edit>')(scope);
            controller = element.controller('contactEdit');
            scope.$apply();

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }));

        it('basic information - all fields of the section are displayed', () => {
            _.forOwn(pageObjectSelectors, (selector) => {
                let formElement = element.find(selector);

                expect(formElement.length).toBe(1, selector + ' doesnt exist');
            });
        });

        it('basic information - firstName is set', () => {
            let field = element.find(pageObjectSelectors.firstNameInput);
            let fieldValue = (<HTMLInputElement>field[0]).value;
            expect(fieldValue).toBe(contactMock.firstName);
        });

        it('basic information - lastName is set', () => {
            let field = element.find(pageObjectSelectors.lastNameInput);
            let fieldValue = (<HTMLInputElement>field[0]).value;
            expect(fieldValue).toBe(contactMock.lastName);
        });
        
        it('basic information - firstName is not required', () => {
            // bug cover - there was a bug that firstName had required validator, but it shouldn't
            let lastNameValidator = assertValidator
                .set("", pageObjectSelectors.firstNameInput)
                .checkRequired();

            lastNameValidator.assertValid();
        });
        
        it('basic information - lastName is required and validator is triggered', () => {
            let lastNameValidator = assertValidator
                .set("", pageObjectSelectors.lastNameInput)
                .checkRequired();

            lastNameValidator.assertInvalid();
        });
    });
}