/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ContactAddController = Contact.ContactAddController;
    import Dto = Common.Models.Dto;
    import ContactTitleService = Services.ContactTitleService;

    //// TODO - Mock / Add as per necessary
    var mailingSalutationSemiFormalEnumItem: Dto.IEnumItem = { id: "mailingSalutationId", code: "MailingSemiformal" };
    var eventSalutationSemiFormalEnumItem: Dto.IEnumItem = { id: "eventSalutationId", code: "EventSemiformal" };
    var salutationFormatEnumItem: Dto.IEnumItem = { id: "1", code: "MrJohnSmith" };

    describe('Given contact add controller', () => {
        var scope: ng.IScope,
            controller: ContactAddController,
            element: ng.IAugmentedJQuery,
            q: ng.IQService,
            $http: ng.IHttpBackendService;

        var returnedValue = {
            data: [
                TestHelpers.ContactTitleGenerator.generate('Mr', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Mrs', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Ms', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Miss', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Master', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Frau', 'de')
            ]
        };

        beforeEach(inject((
            enumService: Mock.EnumServiceMock,
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            contactTitleService: ContactTitleService,
            $q: ng.IQService,
            $httpBackend: ng.IHttpBackendService,
            enumProvider: Providers.EnumProvider) => {

            $http = $httpBackend;
            q = $q;

            enumService.setEnum(Dto.EnumTypeCode.MailingSalutation.toString(), [mailingSalutationSemiFormalEnumItem]);
            enumService.setEnum(Dto.EnumTypeCode.EventSalutation.toString(), [eventSalutationSemiFormalEnumItem]);
            enumService.setEnum(Dto.EnumTypeCode.SalutationFormat.toString(), [salutationFormatEnumItem]);

            $http.expectGET('/api/contacts/titles').respond(() => {
                return [200, []];
            });

            spyOn(contactTitleService, 'get').and.callFake(() => {
                var deffered = q.defer();
                deffered.resolve(returnedValue);
                return deffered.promise;
            });

            scope = $rootScope.$new();
            scope["enumService"] = enumService;
            scope["userData"] = <Dto.ICurrentUser>{
                id: '1234',
                firstName: 'Test',
                lastName: 'User',
                division: <Dto.IEnumTypeItem>{ id: 'enumId', code: 'code' },
                salutationFormatId: '1',
                locale: <Dto.ILocale>{ id: '', isoCode: 'en' }
            };

            enumProvider.enums = TestHelpers.Contacts.salutationFormatsDictionary;

            element = $compile("<contact-add user-data='userData' contact='contact'></contact-add>")(scope);
            scope.$apply();

            controller = element.controller("contactAdd");
        }));

        it('when controller is first loaded then lead negotiator is set to the current user', () => {
            expect(controller.contact.leadNegotiator.user.id).toBe('1234');
            expect(controller.contact.leadNegotiator.user.firstName).toBe('Test');
            expect(controller.contact.leadNegotiator.user.lastName).toBe('User');
        });

        it('when get contact titles is called with blank string then only locale specific titles returned', () => {
            var result = controller.getContactTitles('');

            expect(result.length).toBe(5);
        });

        it('when get contact titles is called with specified other string then no titles is returend', () => {
            var result = controller.getContactTitles('other title');

            expect(result.length).toBe(0);
        });

        it('when get contact titles is called with specified string then matched locale specific titles returned', () => {
            var result = controller.getContactTitles('Mrs');

            expect(result.length).toBe(1);
        });

        it('when get contact titles is called with specified string in other language then matched titles returned', () => {
            var result = controller.getContactTitles('Frau');

            expect(result.length).toBe(1);
        });

        it('when get contact titles is called with specified string then matched locale specific titles returned ordered alpabetically', () => {
            var typedValue = 'a';
            var result = controller.getContactTitles(typedValue);

            var filteredTitles = returnedValue.data.filter((items) => { return items.title.toLowerCase().indexOf(typedValue.toLowerCase()) > -1; });
            var sortedTitles = _.sortBy(filteredTitles, (item) => item.title).map((items) => items.title);

            expect(result).toEqual(sortedTitles);
        });

        it('when get contact titles is called with blank string then only locale specific titles returned ordered by priority and then alpabetically ', () => {
            var result = controller.getContactTitles('');

            var filteredTitles = returnedValue.data.filter((items) => { return items.locale.isoCode === 'en' });
            var sortedTitles = _.sortBy(filteredTitles, (item) => [item.priority || Number.POSITIVE_INFINITY, item.title]).map((items) => items.title);

            expect(result).toEqual(sortedTitles);
        });

        it('when contactTitleSelect is called with some specified string then setSalutations() method is called', () => {
            spyOn(controller, 'setSalutations');

            controller.contactTitleSelect('some title');

            expect(controller.setSalutations).toHaveBeenCalled();
        });

        ///// mailing semi formal salutation

        it('when title and first name and last name are entered then mailing semi formal salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingSemiformalSalutation).toEqual('Mr Smith');
        });

        it('when only first name and last name are entered then mailing semi formal salutation is updated correctly', () => {
            controller.selectedTitle = '';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingSemiformalSalutation).toEqual('Smith');
        });

        it('when only title and first name are entered then mailing semi formal salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = '';

            controller.setSalutations();

            expect(controller.contact.mailingSemiformalSalutation).toEqual('Mr');
        });

        ///// mailing informal salutation

        it('when multi-character first name is entered then mailing informal salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingInformalSalutation).toEqual('John');
        });

        it('when single-character first name is entered then mailing informal salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'J';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingInformalSalutation).toEqual('Mr Smith');
        });

        ///// mailing formal salutation

        it('when mr is entered as a title then mailing formal salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingFormalSalutation).toEqual('Sir');
        });

        it('when mrs is entered as a title then mailing formal salutation is updated correctly', () => {
            controller.selectedTitle = 'Mrs';
            controller.contact.firstName = 'Jane';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingFormalSalutation).toEqual('Madam');
        });

        it('when dr is entered as a title then mailing formal salutation is updated correctly', () => {
            controller.selectedTitle = 'Dr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';

            controller.setSalutations();

            expect(controller.contact.mailingFormalSalutation).toEqual('Dr Smith');
        });

        ///// mailing envelope salutation

        it('when mr is entered as a title and salutation preference is esq then mailing envelope salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';
            controller.defaultSalutationFormat = 'JohnSmithEsq';

            controller.setSalutations();

            expect(controller.contact.mailingEnvelopeSalutation).toEqual('John Smith, Esq');
        });

        it('when mrs is entered as a title and salutation preference is esq then mailing envelope salutation is updated correctly', () => {
            controller.selectedTitle = 'Mrs';
            controller.contact.firstName = 'Jane';
            controller.contact.lastName = 'Smith';
            controller.defaultSalutationFormat = 'JohnSmithEsq';

            controller.setSalutations();

            expect(controller.contact.mailingEnvelopeSalutation).toEqual('Mrs Jane Smith');
        });

        it('when mr is entered as a title and salutation preference is not esq then mailing envelope salutation is updated correctly', () => {
            controller.selectedTitle = 'Mr';
            controller.contact.firstName = 'John';
            controller.contact.lastName = 'Smith';
            controller.defaultSalutationFormat = 'MrJohnSmith';

            controller.setSalutations();

            expect(controller.contact.mailingEnvelopeSalutation).toEqual('Mr John Smith');
        });

    });
}