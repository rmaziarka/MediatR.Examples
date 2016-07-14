/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ContactAddController = Contact.ContactAddController;
    import Business = Common.Models.Business;

    describe('Given edit salutations is being added', () => {
        var scope: ng.IScope,
            element: ng.IAugmentedJQuery,
            controller: ContactAddController,
            $http: ng.IHttpBackendService,
            assertValidator: TestHelpers.AssertValidators;

        var pageObjectSelectors = {
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

        var salutationFormats: Common.Models.Dto.IEnumDictionary = {
            activityDepartmentType: [],
            activityDocumentType: [],
            activityStatus: [],
            userType: [],
            division: [],
            entityType: [],
            offerStatus: [],
            ownershipType: [],
            salutationFormat: [
                { id: '1', code: 'MrJohnSmith' },
                { id: '2', code: 'JohnSmithEsq' },
            ],
            rentPaymentPeriod: []
        };

        beforeEach(inject((
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService,
            enumProvider: Providers.EnumProvider) => {

            $http = $httpBackend;

            scope = $rootScope.$new();
            scope['defaultSalutationFormatId'] = '1';
            scope['contact'] = new Business.Contact();

            enumProvider.enums = salutationFormats;

            element = $compile("<edit-salutations contact='contact' first-name='contact.firstName' last-name='contact.lastName' title='selectedTitle' default-salutation-format-id='defaultSalutationFormatId' />")(scope);
            scope.$apply();
            controller = element.controller("EditSalutationsController");

            assertValidator = new TestHelpers.AssertValidators(element, scope);
        }
        ));

        ///////// mailingFormalSalutationSelector

        it('when mailing formal salutation value is too long then validation message should be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength + 1, false, pageObjectSelectors.mailingFormalSalutationSelector);
        });

        it('when mailing formal salutation value has max length then validation message should not be displayed', () => {
            var maxLength = 128;
            assertValidator.assertMaxLengthValidator(maxLength, true, pageObjectSelectors.mailingFormalSalutationSelector);
        });

        it('when mailing formal salutation value is empty then validation message should be displayed', () => {
            assertValidator.assertRequiredValidator("", false, pageObjectSelectors.mailingFormalSalutationSelector);
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

        it('when mailing semiformal salutation value is empty then validation message should be displayed', () => {
            assertValidator.assertRequiredValidator("", false, pageObjectSelectors.mailingSemiformalSalutationSelector);
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

        it('when mailing envelope salutation value is empty then validation message should be displayed', () => {
            assertValidator.assertRequiredValidator("", false, pageObjectSelectors.mailingEnvelopeSalutationSelector);
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