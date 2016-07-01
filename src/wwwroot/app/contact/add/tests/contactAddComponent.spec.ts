/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ContactAddController = Antares.Contact.ContactAddController;
    import Dto = Antares.Common.Models.Dto;

    describe('Given contact is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: ContactAddController,
            $http: ng.IHttpBackendService,
            assertValidator: Antares.TestHelpers.AssertValidators;

        var pageObjectSelectors = {
            titleSelector: 'input#title',
            titleSectionSelector: '#title-section',
            searchComponentCancelButton: 'input[type=button]',
            firstNameSelector: 'input#first-name',
            lastNameSelector: 'input#last-name',
            mailingFormalSalutationSelector: 'input#mailing-formal-salutation',
            mailingSemiformalSalutationSelector: 'input#mailing-semiformal-salutation',
            mailingInformalSalutationSelector: 'input#mailing-informal-salutation',
            mailingPersonalSalutationSelector: 'input#mailing-personal-salutation',
            mailingEnvelopeSalutationSelector: 'input#mailing-envelope-salutation',
            eventInviteSalutationSelector: 'input#event-invite-salutation',
            eventSemiformalSalutationSelector: 'input#event-semiformal-salutation',
            eventInformalSalutationSelector: 'input#event-informal-salutation',
            eventPersonalSalutationSelector: 'input#event-personal-salutation',
            eventEnvelopeSalutationSelector: 'input#event-envelope-salutation'
        };

        var mailingSalutations = [
            { id: '1', code: 'MailingFormal' },
            { id: '2', code: 'MailingSemiformal' },
            { id: '3', code: 'MailingInformal' },
            { id: '4', code: 'MailingPersonal' },
            { id: '5', code: 'MailingEnvelope' }
        ];

        var eventSalutations = [
            { id: '1', code: 'EventInvite' },
            { id: '2', code: 'EventSemiformal' },
            { id: '3', code: 'EventInformal' },
            { id: '4', code: 'EventPersonal' },
            { id: '5', code: 'EventEnvelope' }
        ];

        var salutationFormats = [
            { id: '1', code: 'MrJohnSmith' },
            { id: '2', code: 'JohnSmithEsq' }
        ];

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            enumService: Mock.EnumServiceMock) => {

            $http = $httpBackend;

            $http.expectGET('/api/contacts/titles').respond(() => {
                return [200, []];
            });

            scope = $rootScope.$new();
            scope["userData"] = <Dto.ICurrentUser>{
                division: <Dto.IEnumTypeItem>{ id: 'enumId', code: 'code' }
            };

            enumService.setEnum(Dto.EnumTypeCode.MailingSalutation.toString(), mailingSalutations);
            enumService.setEnum(Dto.EnumTypeCode.EventSalutation.toString(), eventSalutations);
            enumService.setEnum(Dto.EnumTypeCode.SalutationFormat.toString(), salutationFormats);

            element = $compile("<contact-add user-data='userData' contact='contact'></contact-add>")(scope);
            scope.$apply();
            controller = element.controller("contactAdd");

            assertValidator = new Antares.TestHelpers.AssertValidators(element, scope);
        }
        ));

        ///////// titleSelector
        describe('and title component is configured for handling title', () => {
            beforeEach(inject((
                $rootScope: ng.IRootScopeService,
                $compile: ng.ICompileService) => {

                scope = $rootScope.$new();

                scope["searchOptions"] = new Common.Component.SearchOptions({
                        minLength: 0,
                        isEditable: true,
                        nullOnSelect: false,
                        showCancelButton: false,
                        isRequired: true,
                        maxLength: 128,
                        modelOptions: {
                             debounce: {
                                 default: 0,
                                 blur: 0
                             }
                        }
                    });

                scope["contactTitleSelect"] = () => { };

                element = $compile("<search search-id='title' search-name='title' options='searchOptions' on-change-value='contactTitleSelect'></search>")(scope);
                scope.$apply();
                controller = element.controller("SearchController");

                assertValidator = new TestHelpers.AssertValidators(element, scope);
            }));

            it('then cancel button is not diaplyed', () => {
                var searchComponentCancelButtonElement = element.find(pageObjectSelectors.searchComponentCancelButton);

                expect(searchComponentCancelButtonElement.length).toBe(0);
            });

            it('when title value is missing then validation should no pass', () => {
                assertValidator.assertInputOnlyRequiredValidator(null, false, pageObjectSelectors.titleSelector);
            });

            it('when title value is present then validation should pass', () => {
                assertValidator.assertRequiredValidator('Miss', true, pageObjectSelectors.titleSelector);
            });

            it('when title value has max length then validation should pass', () => {
                var maxLength = 128;
                assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.titleSelector);
            });

            it('when title value is too long then validation should no pass', () => {
                var maxLength = 128;
                assertValidator.assertInputOnlyMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.titleSelector);
            });
        });

        ///////// firstNameSelector

        it('when first name value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.firstNameSelector);
        });

        it('when first name value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.firstNameSelector);
        });

        ///////// lastNameSelector

        it('when last name value is missing then required message should be displayed', () => {
            assertValidator.assertRequiredValidator(null, false, pageObjectSelectors.lastNameSelector);
        });

        it('when last name value is present then required message should not be displayed', () => {
            assertValidator.assertRequiredValidator('Name', true, pageObjectSelectors.lastNameSelector);
        });

        it('when last name value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.lastNameSelector);
        });

        it('when last name value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.lastNameSelector);
        });

        ///////// mailingFormalSalutationSelector

        it('when mailing formal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.mailingFormalSalutationSelector);
        });

        it('when mailing formal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.mailingFormalSalutationSelector);
        });

        ///////// mailingSemiformalSalutationSelector

        it('when mailing semiformal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.mailingSemiformalSalutationSelector);
        });

        it('when mailing semiformal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.mailingSemiformalSalutationSelector);
        });

        ///////// mailingInformalSalutationSelector

        it('when mailing informal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.mailingInformalSalutationSelector);
        });

        it('when mailing informal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.mailingInformalSalutationSelector);
        });

        ///////// mailingPersonalSalutationSelector

        it('when mailing personal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.mailingPersonalSalutationSelector);
        });

        it('when mailing personal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.mailingPersonalSalutationSelector);
        });

        ///////// mailingEnvelopeSalutationSelector

        it('when mailing envelope salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.mailingEnvelopeSalutationSelector);
        });

        it('when mailing envelope salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.mailingEnvelopeSalutationSelector);
        });

        ///////// eventInviteSalutationSelector

        it('when event invite salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.eventInviteSalutationSelector);
        });

        it('when event invite salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.eventInviteSalutationSelector);
        });

        ///////// eventSemiformalSalutationSelector

        it('when event semiformal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.eventSemiformalSalutationSelector);
        });

        it('when event semiformal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.eventSemiformalSalutationSelector);
        });

        ///////// eventInformalSalutationSelector

        it('when event informal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.eventInformalSalutationSelector);
        });

        it('when event informal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.eventInformalSalutationSelector);
        });

        ///////// eventPersonalSalutationSelector

        it('when event personal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.eventPersonalSalutationSelector);
        });

        it('when event personal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.eventPersonalSalutationSelector);
        });

        ///////// eventEnvelopeSalutationSelector

        it('when event envelope salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.eventEnvelopeSalutationSelector);
        });

        it('when event envelope salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.eventEnvelopeSalutationSelector);
        });
    });
}