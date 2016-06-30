/// <reference path="../../../typings/_all.d.ts" />

module Antares {
    import ContactAddController = Antares.Contact.ContactAddController;
    import Business = Antares.Common.Models.Business;
    import Dto = Antares.Common.Models.Dto;

    // TODO - Mock / Add as per necessary
    var mailingSalutationSemiFormalEnumItem: Dto.IEnumItem = { id: "mailingSalutationId", code: "MailingSemiformal" };
    var eventSalutationSemiFormalEnumItem: Dto.IEnumItem = { id: "eventSalutationId", code: "EventSemiformal" };
    var salutationFormatEnumItem: Dto.IEnumItem = { id: "1", code: "MrJohnSmith" };

    xdescribe('Given contact is being added', () =>{
        var scope: ng.IScope,
            controller: ContactAddController,
            element: ng.IAugmentedJQuery,
            $http: ng.IHttpBackendService;

        beforeEach(inject((
            enumService: Mock.EnumServiceMock,
            contactTitleService: Mock.ContactTitleServiceMock,
            $rootScope: ng.IRootScopeService,
            $compile: ng.ICompileService,
            $httpBackend: ng.IHttpBackendService) => {
            
            // init

            // TODO - Mock / Add as per necessary
            enumService.setEnum(Dto.EnumTypeCode.MailingSalutation.toString(), [mailingSalutationSemiFormalEnumItem]);
            enumService.setEnum(Dto.EnumTypeCode.EventSalutation.toString(), [eventSalutationSemiFormalEnumItem]);
            enumService.setEnum(Dto.EnumTypeCode.SalutationFormat.toString(), [salutationFormatEnumItem]);

            contactTitleService.setTitles([
                TestHelpers.ContactTitleGenerator.generate('Mr', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Mrs', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Ms', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Miss', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Master', 'en'),
                TestHelpers.ContactTitleGenerator.generate('Frau', 'de')
            ]);

            $http = $httpBackend;
            $http.expectGET('/api/contacts/titles').respond(() => {
                return [200, []];
            });

            $http = $httpBackend;
            scope = $rootScope.$new();
            element = $compile("<contact-add user-data='userData' contact='contact'></contact-add>")(scope);
            scope["userData"] = <Dto.ICurrentUser>{
                id: '1234',
                division: <Dto.IEnumTypeItem>{ id: 'enumId', code: 'code' },
                salutationFormatId: '1',
                locale : <Dto.ILocale>{ id: '', isoCode: 'en' }
            };
            scope["contactTitleService"] = contactTitleService;
            scope["enumService"] = enumService;
            scope.$apply();
            controller = element.controller("contactAdd");
        }));

        it('when get contact titles is called with blank string then only locale specific titles returned', () =>{
            
            var result = controller.getContactTitles('');
            expect(result.length).toBe(5);
        });
        
    });
}